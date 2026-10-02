using UnityEngine;

public static class Receptacle
{
    private const int Rings = 6;
    private const int Sides = 22;
    private const float FullTurn = 360.0f;
    private const float SkirtDrop = 0.72f;

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette,
        Pose pose, float flatten)
    {
        float radius = settings.receptacleRadius;
        int centre = mesh.VertexCount;
        mesh.AddVertex(pose.Place(Vector3.up * (radius * flatten)), Vector3.zero, Vector4.zero,
            palette.receptacle, StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
            Shading.Surface(pose.Aim(Vector3.up)));

        for (int ring = 1; ring <= Rings; ring++)
        {
            float pitch = Mathf.PI * 0.5f * ring / Rings;
            float height = Mathf.Cos(pitch) * radius * flatten;
            float girth = Mathf.Sin(pitch) * radius;

            for (int side = 0; side < Sides; side++)
            {
                float angle = FullTurn * side / Sides * Mathf.Deg2Rad;
                Vector3 place = new Vector3(Mathf.Cos(angle) * girth, height, Mathf.Sin(angle) * girth);
                Vector3 normal = new Vector3(place.x, place.y / Mathf.Max(flatten, 0.1f), place.z).normalized;
                mesh.AddVertex(pose.Place(place), Vector3.zero, Vector4.zero, palette.receptacle,
                    StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
                    Shading.Surface(pose.Aim(normal)));
            }
        }

        int skirt = mesh.VertexCount;
        for (int side = 0; side < Sides; side++)
        {
            float angle = FullTurn * side / Sides * Mathf.Deg2Rad;
            Vector3 rim = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle)) * radius * 0.54f;
            Vector3 place = rim - Vector3.up * (radius * SkirtDrop);
            mesh.AddVertex(pose.Place(place), Vector3.zero, Vector4.zero, palette.receptacle,
                StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
                Shading.Surface(pose.Aim(rim.normalized)));
        }

        for (int side = 0; side < Sides; side++)
        {
            mesh.AddTriangle(centre, centre + 1 + side, centre + 1 + (side + 1) % Sides);
        }

        for (int ring = 1; ring < Rings; ring++)
        {
            int inner = centre + 1 + (ring - 1) * Sides;
            int outer = inner + Sides;
            for (int side = 0; side < Sides; side++)
            {
                int next = (side + 1) % Sides;
                mesh.AddQuad(inner + side, inner + next, outer + next, outer + side);
            }
        }

        int brim = centre + 1 + (Rings - 1) * Sides;
        for (int side = 0; side < Sides; side++)
        {
            int next = (side + 1) % Sides;
            mesh.AddQuad(brim + side, brim + next, skirt + next, skirt + side);
        }

        int floor = mesh.VertexCount;
        mesh.AddVertex(pose.Place(-Vector3.up * (radius * SkirtDrop)), Vector3.zero, Vector4.zero,
            palette.receptacle, StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
            Shading.Surface(pose.Aim(-Vector3.up)));

        for (int side = 0; side < Sides; side++)
        {
            mesh.AddTriangle(floor, skirt + (side + 1) % Sides, skirt + side);
        }
    }
}
