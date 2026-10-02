using UnityEngine;

public static class Receptacle
{
    private const int Rings = 10;
    private const int Sides = 32;
    private const float FullTurn = 360.0f;
    private const float SkirtDrop = 0.50f;
    private const float Swell = 1.34f;
    private const int PitSides = 7;
    private const float PitLift = 0.35f;

    public static float Flatten(DandelionSettings settings, float bare)
    {
        return Mathf.Lerp(settings.clockDome, settings.bareDome, bare);
    }

    public static Vector3 Skin(DandelionSettings settings, Vector3 heading, float flatten, float bare)
    {
        float radius = settings.receptacleRadius * Mathf.Lerp(1.0f, Swell, bare);
        return new Vector3(heading.x, heading.y * flatten, heading.z) * radius;
    }

    public static void Pit(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette,
        Pose pose, Vector3 heading, float flatten, float bare)
    {
        if (heading.y <= 0.0f)
        {
            return;
        }

        Vector3 skin = Skin(settings, heading, flatten, bare);
        Vector3 normal = new Vector3(heading.x, heading.y / Mathf.Max(flatten, 0.1f), heading.z).normalized;
        Vector3 flank = Vector3.Cross(normal, Vector3.up);
        flank = (flank.sqrMagnitude < 1e-8f ? Vector3.right : flank.normalized);
        Vector3 binormal = Vector3.Cross(normal, flank);
        Vector3 seat = skin + normal * (settings.pitRadius * PitLift);

        int centre = mesh.VertexCount;
        mesh.AddVertex(pose.Place(seat - normal * settings.pitDepth), Vector3.zero, Vector4.zero,
            palette.pit, StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
            Shading.Surface(pose.Aim(normal)));

        for (int side = 0; side < PitSides; side++)
        {
            float angle = FullTurn * side / PitSides * Mathf.Deg2Rad;
            Vector3 rim = flank * Mathf.Cos(angle) + binormal * Mathf.Sin(angle);
            mesh.AddVertex(pose.Place(seat + rim * settings.pitRadius), Vector3.zero, Vector4.zero,
                palette.pit, StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
                Shading.Surface(pose.Aim(normal)));
        }

        for (int side = 0; side < PitSides; side++)
        {
            mesh.AddTriangle(centre, centre + 1 + side, centre + 1 + (side + 1) % PitSides);
        }
    }

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette,
        Pose pose, float flatten, float bare)
    {
        Color skin = Color.Lerp(palette.receptacle, palette.receptacleBare, bare);
        float radius = settings.receptacleRadius * Mathf.Lerp(1.0f, Swell, bare);
        int centre = mesh.VertexCount;
        mesh.AddVertex(pose.Place(Vector3.up * (radius * flatten)), Vector3.zero, Vector4.zero,
            skin, StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
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
                mesh.AddVertex(pose.Place(place), Vector3.zero, Vector4.zero, skin,
                    StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
                    Shading.Surface(pose.Aim(normal)));
            }
        }

        int skirt = mesh.VertexCount;
        for (int side = 0; side < Sides; side++)
        {
            float angle = FullTurn * side / Sides * Mathf.Deg2Rad;
            Vector3 rim = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle)) * radius * 0.40f;
            Vector3 place = rim - Vector3.up * (radius * SkirtDrop);
            mesh.AddVertex(pose.Place(place), Vector3.zero, Vector4.zero, skin,
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
            skin, StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
            Shading.Surface(pose.Aim(-Vector3.up)));

        for (int side = 0; side < Sides; side++)
        {
            mesh.AddTriangle(floor, skirt + (side + 1) % Sides, skirt + side);
        }
    }
}
