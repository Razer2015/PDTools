using System;
using System.Linq;

namespace PDTools.Structures.PS3;

public class MColorObject(float red, float green, float blue)
{

    public float Blue { get; } = blue;

    public float Green { get; } = green;

    public float Red { get; } = red;

    public float Normalize => ComputeHueAdjustment();
    
    public static MColorObject FromRgb(int bgrColor)
    {
        return new MColorObject((float)(((bgrColor >> 16) & 0xFF) / 255.0),
            (float)(((bgrColor >> 8) & 0xFF) / 255.0), (float)(((bgrColor >> 0) & 0xFF) / 255.0));
    }

    public static MColorObject FromBgr(int bgrColor)
    {
        return new MColorObject((float)(((bgrColor >> 0) & 0xFF) / 255.0),
            (float)(((bgrColor >> 8) & 0xFF) / 255.0), (float)(((bgrColor >> 16) & 0xFF) / 255.0));
    }

    private float ComputeHueAdjustment()
    {
        Rgb2Hsv(Red, Green, Blue, out var hue, out var saturation, out var value);

        float result;
        if (saturation < 0.25f)
        {
            result = 1.0f - value;
        }
        else
        {
            result = hue + 0.33333334f;
            if (result > 1.0f)
            {
                result -= 1.0f;
            }

            result = result * 3.0f + 1.0f;
        }

        return result * 0.25f;
    }

    private static void Rgb2Hsv(float r, float g, float b, out float hue, out float saturation, out float value)
    {
        var max = Math.Max(b, Math.Max(g, r));
        var min = Math.Min(b, Math.Min(g, r));
        value = max;

        var delta = max - min;
        saturation = max == 0 ? 0 : delta / max;

        if (delta == 0)
        {
            hue = 0;
        }
        else if (max == b)
        {
            hue = (g - r) / delta % 6;
        }
        else if (max == g)
        {
            hue = (r - b) / delta + 2;
        }
        else
        {
            hue = (b - g) / delta + 4;
        }

        hue *= 60;
        if (hue < 0)
        {
            hue += 360;
        }

        hue /= 360;
    }

}