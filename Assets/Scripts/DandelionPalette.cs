using UnityEngine;

public sealed class DandelionPalette
{
    public Color achene;
    public Color beak;
    public Color pappus;
    public Color ink;
    public Color background;

    public static readonly DandelionPalette Meadow = new DandelionPalette
    {
        achene = Hex(0x8a7249),
        beak = Hex(0xc8bda0),
        pappus = Hex(0xe4e0d2),
        ink = Hex(0x2a2722),
        background = Hex(0xf6f3ec)
    };

    private static Color Hex(uint value)
    {
        float red = ((value >> 16) & 0xff) / 255.0f;
        float green = ((value >> 8) & 0xff) / 255.0f;
        float blue = (value & 0xff) / 255.0f;
        return new Color(red, green, blue, 1.0f);
    }
}
