using UnityEngine;

public static class DandelionGeometry
{
    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette)
    {
        mesh.SetInk(palette.ink);
        Seed.Build(mesh, settings, palette, new SeedPlan
        {
            root = Vector3.zero,
            heading = Vector3.up,
            scale = 1.0f,
            roll = 0.0f,
            open = 1.0f
        });
    }
}
