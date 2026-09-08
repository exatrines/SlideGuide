using System;

namespace SlideGuide;

// Shared timing and geometry for the live marker.
internal static class MarkerMath
{
    // CastBar.uld node 11 uses Parameter_Gauge.tex parts group 2 index 2 (160x20).
    // The opaque fill is 148x8; 6px on each side is unused padding / glow.
    internal const float StandardFillPad = 6f;

    internal static bool TryEvaluate(float totalSeconds, float elapsedSeconds, int thresholdMs,
        out float fraction, out bool ready)
    {
        fraction = 0;
        ready = false;
        if (!float.IsFinite(totalSeconds) || !float.IsFinite(elapsedSeconds) ||
            totalSeconds <= 0 || elapsedSeconds < 0 || elapsedSeconds >= totalSeconds)
            return false;
        float threshold = Math.Max(0, totalSeconds - Math.Clamp(thresholdMs, 0, 1000) / 1000f);
        fraction = threshold / totalSeconds;
        ready = elapsedSeconds >= threshold;
        return true;
    }

    internal static float ScaledPad(float nodeHeight, float partHeight) =>
        partHeight > 0 ? StandardFillPad * nodeHeight / partHeight : StandardFillPad;

    internal static float HorizontalPosition(float nodeLeft, float nodeWidth, float pad, float fraction,
        float scale) =>
        nodeLeft + (pad + Math.Max(0, nodeWidth - 2 * pad) * fraction) * scale;

    // Extra height is added beyond the visible bar; extra 0 is flush with barTop..barTop+barHeight.
    internal static void VerticalSpan(float barTop, float barHeight, float extra, float scale,
        out float top, out float bottom)
    {
        top = barTop - extra * scale;
        bottom = barTop + (barHeight + extra) * scale;
    }

    internal static void VerticalBodySpan(float nodeTop, float nodeHeight, float pad, float extra,
        float scale, out float top, out float bottom) =>
        VerticalSpan(nodeTop + pad * scale, Math.Max(0, nodeHeight - 2 * pad), extra, scale, out top, out bottom);
}
