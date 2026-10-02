using UnityEngine;

public static class DandelionGeometry
{
    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette)
    {
        mesh.SetInk(palette.ink);
        if (settings.grey >= 1.0f)
        {
            Clock.Build(mesh, settings, palette, settings.phase);
        }
        else
        {
            Bloom.Build(mesh, settings, palette, settings.open);
        }

        Involucre.Build(mesh, settings, palette, settings.open);
    }
}
