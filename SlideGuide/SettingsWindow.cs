using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using MirageUI;
using MirageUI.Theme;

namespace SlideGuide;

// MirageUI supplies headings/theme; native ImGui supplies the scalar editors.
internal sealed class SettingsWindow : Window
{
    private readonly Configuration config;
    private readonly Action save;
    private ImRaii.ColorDisposable? theme;

    internal SettingsWindow(Configuration config, Action save) : base("SlideGuide settings")
    {
        this.config = config;
        this.save = save;
        Size = new Vector2(470, 320);
        SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override void PreDraw() => theme = MirageTheme.PushCustom(MirageTheme.ResolveAppliedColors());
    public override void PostDraw()
    {
        MirageTheme.Pop(theme!);
        theme = null;
    }

    public override void Draw()
    {
        MirageUi.SubHeader("Threshold", pushDown: false);
        var changed = ImGui.SliderInt("Time (ms)", ref config.ThresholdMs, 0, 1000);
        MirageUi.Text("An estimated movement threshold. Adjust to your connection; this does not detect server acceptance.");
        MirageUi.SubHeader("Appearance");
        changed |= ImGui.SliderFloat("Line width", ref config.Width, 1, 10, "%.0f");
        changed |= ImGui.SliderFloat("Extra height", ref config.ExtraHeight, 0, 20, "%.0f");
        changed |= ImGui.ColorEdit4("Before threshold", ref config.WaitingColor);
        changed |= ImGui.ColorEdit4("After threshold", ref config.ReadyColor);
        if (changed)
        {
            config.Normalize();
            save();
        }
    }
}
