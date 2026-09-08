using System;
using System.Numerics;
using Dalamud.Configuration;
using Newtonsoft.Json;

namespace SlideGuide;

// Persisted appearance and timing; the threshold is a user-calibrated estimate.
public sealed class Configuration : IPluginConfiguration
{
    private static readonly Vector4 DefaultWaiting = new(0.95f, 0.45f, 0.25f, 1);
    private static readonly Vector4 DefaultReady = new(0.25f, 0.95f, 0.5f, 1);

    public int Version { get; set; } = 1;

    [JsonProperty("WindowMs")]
    public int ThresholdMs = 500;

    public float Width = 3;
    public float ExtraHeight = 0;
    public Vector4 WaitingColor = DefaultWaiting;
    public Vector4 ReadyColor = DefaultReady;

    public void Normalize()
    {
        ThresholdMs = Math.Clamp(ThresholdMs, 0, 1000);
        Width = float.IsFinite(Width) ? Math.Clamp(Width, 1, 10) : 3;
        ExtraHeight = float.IsFinite(ExtraHeight) ? Math.Clamp(ExtraHeight, 0, 20) : 0;
        WaitingColor = Sanitize(WaitingColor, DefaultWaiting);
        ReadyColor = Sanitize(ReadyColor, DefaultReady);
    }

    private static Vector4 Sanitize(Vector4 value, Vector4 fallback) =>
        float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z) && float.IsFinite(value.W)
            ? Vector4.Clamp(value, Vector4.Zero, Vector4.One) : fallback;
}
