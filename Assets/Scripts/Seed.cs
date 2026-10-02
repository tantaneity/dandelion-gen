using UnityEngine;

public struct SeedPlan
{
    public Vector3 root;
    public Vector3 heading;
    public float scale;
    public float roll;
    public float open;
}

public static class Seed
{
    private const float FullTurn = 360.0f;
    private const float RibJitter = 0.12f;

    private static Vector3 Sideways(Vector3 heading)
    {
        Vector3 guess = Mathf.Abs(heading.y) > 0.9f ? Vector3.right : Vector3.up;
        return Vector3.Cross(heading, guess).normalized;
    }

    private static float Spindle(float along, float taper)
    {
        return Mathf.Pow(Mathf.Max(Mathf.Sin(Mathf.Clamp01(along) * Mathf.PI), 0.0f), taper);
    }

    private static void Achene(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette,
        SeedPlan plan, Vector3 flank, Vector3 binormal)
    {
        int rings = Mathf.Max(settings.acheneRings, 2);
        int sides = Mathf.Max(settings.acheneSides, 3);
        int first = mesh.VertexCount;

        for (int ring = 0; ring < rings; ring++)
        {
            float along = ring / (float)(rings - 1);
            Vector3 centre = plan.root + plan.heading * (settings.acheneLength * plan.scale * along);
            float girth = settings.acheneRadius * plan.scale * Spindle(along, settings.acheneTaper);

            for (int side = 0; side < sides; side++)
            {
                float angle = FullTurn * side / sides * Mathf.Deg2Rad;
                Vector3 outward = flank * Mathf.Cos(angle) + binormal * Mathf.Sin(angle);
                float rib = 1.0f + RibJitter * Mathf.Cos(angle * sides);
                mesh.AddVertex(centre + outward * girth * rib, Vector3.zero, Vector4.zero,
                    palette.achene, StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias,
                    Shading.Surface(outward));
            }
        }

        for (int ring = 0; ring < rings - 1; ring++)
        {
            for (int side = 0; side < sides; side++)
            {
                int next = (side + 1) % sides;
                int lower = first + ring * sides;
                int upper = lower + sides;
                mesh.AddQuad(lower + side, lower + next, upper + next, upper + side);
            }
        }
    }

    private static void Strand(MeshBuffer mesh, DandelionSettings settings, Color fill, float width,
        float ink, Vector3 start, Vector3 heading, Vector3 bend, float length, int nodes)
    {
        int first = mesh.VertexCount;
        Vector3 cursor = start;
        Vector3 facing = heading;
        float step = length / nodes;

        for (int node = 0; node <= nodes; node++)
        {
            float along = node / (float)nodes;
            Vector3 tangent = (facing + bend * along).normalized;
            Vector4 along4 = new Vector4(tangent.x, tangent.y, tangent.z, 1.0f);
            Vector4 back4 = new Vector4(tangent.x, tangent.y, tangent.z, -1.0f);

            mesh.AddVertex(cursor, Vector3.zero, back4, fill, StrokeKind.Stem,
                width, ink, settings.depthBias, Shading.Flat);
            mesh.AddVertex(cursor, Vector3.zero, along4, fill, StrokeKind.Stem,
                width, ink, settings.depthBias, Shading.Flat);

            if (node > 0)
            {
                int lower = first + (node - 1) * 2;
                mesh.AddQuad(lower, lower + 1, lower + 3, lower + 2);
            }

            cursor += tangent * step;
        }
    }

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette, SeedPlan plan)
    {
        Vector3 flank = Sideways(plan.heading);
        Vector3 binormal = Vector3.Cross(plan.heading, flank).normalized;

        Achene(mesh, settings, palette, plan, flank, binormal);

        Vector3 beakStart = plan.root + plan.heading * (settings.acheneLength * plan.scale);
        Strand(mesh, settings, palette.beak, settings.beakWidth, settings.beakInk, beakStart, plan.heading,
            Vector3.zero, settings.beakLength * plan.scale, settings.beakNodes);

        Vector3 crown = beakStart + plan.heading * (settings.beakLength * plan.scale);
        float spread = Mathf.Lerp(settings.bristleSpread * 0.25f, settings.bristleSpread, plan.open);

        for (int bristle = 0; bristle < settings.bristleCount; bristle++)
        {
            float roll = plan.roll + FullTurn * bristle / settings.bristleCount;
            float wobble = (Hash(bristle, settings.seed) - 0.5f) * settings.bristleJitter;
            Vector3 radial = Quaternion.AngleAxis(roll, plan.heading) * flank;
            Vector3 outward = Quaternion.AngleAxis(spread + wobble, Vector3.Cross(plan.heading, radial))
                              * plan.heading;
            Vector3 lift = Quaternion.AngleAxis(-settings.bristleLift, Vector3.Cross(plan.heading, radial))
                               * outward - outward;

            Strand(mesh, settings, palette.pappus, settings.bristleWidth, settings.bristleInk, crown, outward.normalized,
                lift, settings.bristleLength * plan.scale, settings.bristleNodes);
        }
    }

    private static float Hash(int index, int seed)
    {
        uint hashed = (uint)(index * 73856093) ^ (uint)(seed * 19349663);
        hashed ^= hashed >> 13;
        hashed *= 0x85EBCA6B;
        hashed ^= hashed >> 16;
        return (hashed & 0xFFFFFF) / (float)0xFFFFFF;
    }
}
