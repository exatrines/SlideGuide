using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using MirageUI;

namespace SlideGuide;

// Reads native HUD geometry every draw; never retains or writes native pointers.
public sealed class Plugin : IDalamudPlugin
{
    private const uint TrackNodeId = 10;
    private const uint FillNodeId = 11;

    private readonly IDalamudPluginInterface pluginInterface;
    private readonly ICommandManager commands;
    private readonly IGameGui gameGui;
    private readonly IObjectTable objects;
    private readonly Configuration config;
    private readonly WindowSystem windows = new("SlideGuide");
    private readonly SettingsWindow settings;

    public Plugin(IDalamudPluginInterface pluginInterface, ICommandManager commands,
        IGameGui gameGui, IObjectTable objects, ITextureProvider textures, IPluginLog log)
    {
        this.pluginInterface = pluginInterface;
        this.commands = commands;
        this.gameGui = gameGui;
        this.objects = objects;
        config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        config.Normalize();
        MirageUi.Init(pluginInterface, textures, log);
        settings = new SettingsWindow(config, () => pluginInterface.SavePluginConfig(config));
        windows.AddWindow(settings);
        commands.AddHandler("/slideguide", new CommandInfo((_, _) => ToggleSettings())
        { HelpMessage = "Toggle SlideGuide settings." });
        pluginInterface.UiBuilder.Draw += Draw;
        pluginInterface.UiBuilder.OpenConfigUi += ToggleSettings;
        pluginInterface.UiBuilder.OpenMainUi += ToggleSettings;
    }

    private void ToggleSettings() => settings.Toggle();

    private unsafe void Draw()
    {
        DrawMarker();
        windows.Draw();
    }

    private unsafe void DrawMarker()
    {
        var player = objects.LocalPlayer;
        if (gameGui.GameUiHidden || player == null || !player.IsCasting ||
            !MarkerMath.TryEvaluate(player.TotalCastTime, player.CurrentCastTime, config.ThresholdMs,
                out var fraction, out var ready))
            return;

        var handle = gameGui.GetAddonByName("_CastBar");
        if (handle.IsNull || !handle.IsReady || !handle.IsVisible)
            return;

        var addon = (AddonCastBar*)handle.Address;
        if (addon == null || addon->RootNode == null || !addon->RootNode->IsVisible() || addon->Alpha == 0 ||
            addon->UldManager.LoadedState != AtkLoadState.Loaded || !addon->IsFullyLoaded())
            return;

        // Node 10 (Image) is the full-width track used for X. Node 11 (NineGrid) is the
        // animated fill used for Y; its width follows progress, so do not use it for X.
        var track = addon->GetNodeById(TrackNodeId);
        var fill = addon->GetNodeById(FillNodeId);
        if (track == null || track->Type != NodeType.Image || track->Width == 0 ||
            fill == null || fill->Type != NodeType.NineGrid || fill->Height == 0 ||
            !fill->IsVisible() || fill->Rotation != 0)
            return;

        var padX = MarkerMath.StandardFillPad;
        var padY = FillBodyPadY(fill);
        if (track->Width <= padX * 2 || fill->Height <= padY * 2)
            return;

        var sx = 1f;
        var sy = 1f;
        var opacity = addon->Alpha / 255f;
        var depth = 0;
        for (var node = track; node != null; node = node->ParentNode)
        {
            if (++depth > 32 || !node->IsVisible() || node->Rotation != 0)
                return;
            sx *= node->ScaleX;
            opacity *= node->Color.A / 255f;
        }
        depth = 0;
        for (var node = fill; node != null; node = node->ParentNode)
        {
            if (++depth > 32)
                return;
            sy *= node->ScaleY;
        }
        if (!float.IsFinite(sx) || !float.IsFinite(sy) || sx <= 0 || sy <= 0 || opacity <= 0)
            return;

        var viewport = ImGui.GetMainViewport();
        var x = MarkerMath.HorizontalPosition(viewport.Pos.X + track->ScreenX, track->Width, padX, fraction, sx);
        MarkerMath.VerticalBodySpan(viewport.Pos.Y + fill->ScreenY, fill->Height, padY, config.ExtraHeight, sy,
            out var y0, out var y1);
        if (!float.IsFinite(x) || !float.IsFinite(y0) || !float.IsFinite(y1) || y1 <= y0)
            return;

        var color = ready ? config.ReadyColor : config.WaitingColor;
        color.W *= opacity;
        var halfWidth = config.Width * sx / 2;
        ImGui.GetBackgroundDrawList(viewport).AddRectFilled(new Vector2(x - halfWidth, y0),
            new Vector2(x + halfWidth, y1), ImGui.ColorConvertFloat4ToU32(color));
    }

    private static unsafe float FillBodyPadY(AtkResNode* fill)
    {
        var nine = fill->GetAsAtkNineGridNode();
        if (nine == null || nine->PartsList == null || nine->PartsList->Parts == null ||
            nine->PartId >= nine->PartsList->PartCount)
            return MarkerMath.StandardFillPad;
        var partHeight = nine->PartsList->Parts[nine->PartId].Height;
        return partHeight > 0 ? MarkerMath.ScaledPad(fill->Height, partHeight) : MarkerMath.StandardFillPad;
    }

    public void Dispose()
    {
        pluginInterface.UiBuilder.Draw -= Draw;
        pluginInterface.UiBuilder.OpenConfigUi -= ToggleSettings;
        pluginInterface.UiBuilder.OpenMainUi -= ToggleSettings;
        commands.RemoveHandler("/slideguide");
        windows.RemoveAllWindows();
        MirageUi.Dispose();
    }
}
