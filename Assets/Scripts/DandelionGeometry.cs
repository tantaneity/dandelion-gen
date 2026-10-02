using UnityEngine;

public static class DandelionGeometry
{
    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette)
    {
        mesh.SetInk(palette.ink);
        Gust[] gusts = Breeze.Gusts(settings);
        Pose pose = Stalk.Build(mesh, settings, palette, gusts, settings.phase);

        if (settings.grey >= 1.0f)
        {
            Clock.Build(mesh, settings, palette, pose, settings.phase);
        }
        else
        {
            Bloom.Build(mesh, settings, palette, pose, settings.open);
        }

        Involucre.Build(mesh, settings, palette, pose, settings.open);
    }
}
