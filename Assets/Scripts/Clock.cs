using UnityEngine;

public static class Clock
{
    private const int DomeRings = 6;
    private const int DomeSides = 20;
    private const float FullTurn = 360.0f;

    private static void Receptacle(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette)
    {
        float radius = settings.receptacleRadius;
        int centre = mesh.VertexCount;
        mesh.AddVertex(Vector3.up * radius, Vector3.zero, Vector4.zero, palette.receptacle,
            StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias, Shading.Surface(Vector3.up));

        for (int ring = 1; ring <= DomeRings; ring++)
        {
            float pitch = Mathf.PI * 0.5f * ring / DomeRings;
            float height = Mathf.Cos(pitch) * radius;
            float girth = Mathf.Sin(pitch) * radius;

            for (int side = 0; side < DomeSides; side++)
            {
                float angle = FullTurn * side / DomeSides * Mathf.Deg2Rad;
                Vector3 place = new Vector3(Mathf.Cos(angle) * girth, height, Mathf.Sin(angle) * girth);
                mesh.AddVertex(place, Vector3.zero, Vector4.zero, palette.receptacle, StrokeKind.Card,
                    0.0f, settings.outlineWeight, settings.depthBias, Shading.Surface(place.normalized));
            }
        }

        for (int side = 0; side < DomeSides; side++)
        {
            mesh.AddTriangle(centre, centre + 1 + side, centre + 1 + (side + 1) % DomeSides);
        }

        for (int ring = 1; ring < DomeRings; ring++)
        {
            int inner = centre + 1 + (ring - 1) * DomeSides;
            int outer = inner + DomeSides;
            for (int side = 0; side < DomeSides; side++)
            {
                int next = (side + 1) % DomeSides;
                mesh.AddQuad(inner + side, inner + next, outer + next, outer + side);
            }
        }
    }

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette, float seconds)
    {
        Receptacle(mesh, settings, palette);

        for (int index = 0; index < settings.clockSeeds; index++)
        {
            Vector3 heading = Phyllotaxis.OnSphere(index, settings.clockSeeds);
            if (heading.y < settings.clockFloor)
            {
                continue;
            }

            Flight flight = Dispersal.Follow(settings, index, seconds);
            if (flight.isGone)
            {
                continue;
            }

            float wobble = Seed.Hash(index, settings.seed + 11) - 0.5f;
            Seed.Build(mesh, settings, palette, new SeedPlan
            {
                root = heading * settings.receptacleRadius + flight.offset,
                heading = Vector3.Slerp(heading, flight.heading, flight.away).normalized,
                scale = 1.0f + wobble * settings.seedJitter,
                roll = index * Phyllotaxis.GoldenAngle,
                open = 1.0f
            });
        }
    }
}
