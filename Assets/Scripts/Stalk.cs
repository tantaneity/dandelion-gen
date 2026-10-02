using UnityEngine;

public struct Pose
{
    public Vector3 seat;
    public Quaternion lean;

    public static readonly Pose Upright = new Pose { seat = Vector3.zero, lean = Quaternion.identity };

    public Vector3 Place(Vector3 local)
    {
        return seat + lean * local;
    }

    public Vector3 Aim(Vector3 local)
    {
        return lean * local;
    }
}

public static class Stalk
{
    private const int Sides = 10;
    private const float FullTurn = 360.0f;
    private const float FootFlare = 1.8f;

    public static Pose Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette,
        Gust[] gusts, float seconds)
    {
        int rings = Mathf.Max(settings.stalkRings, 2);
        Vector3 cursor = Vector3.zero;
        Vector3 tangent = Vector3.up;
        Vector3 heading = new Vector3(settings.blowHeading.x, 0.0f, settings.blowHeading.y).normalized;
        Vector3 side = Vector3.Cross(Vector3.up, heading).normalized;
        float step = settings.stalkHeight / (rings - 1);
        int first = mesh.VertexCount;

        Vector2 sway = Wind.Bend(gusts, settings.stalkHeight, settings.stalkStiffness,
            settings.stalkDamping, 0.0f, seconds);

        for (int ring = 0; ring < rings; ring++)
        {
            float along = ring / (float)(rings - 1);
            float girth = settings.stalkRadius * Mathf.Lerp(FootFlare, 1.0f, Mathf.Sqrt(along));
            Vector3 outward = Vector3.Cross(tangent, side).normalized;

            for (int face = 0; face < Sides; face++)
            {
                float angle = FullTurn * face / Sides * Mathf.Deg2Rad;
                Vector3 rim = side * Mathf.Cos(angle) + outward * Mathf.Sin(angle);
                mesh.AddVertex(cursor + rim * girth, Vector3.zero, Vector4.zero, palette.stalk,
                    StrokeKind.Card, 0.0f, settings.outlineWeight, settings.depthBias, Shading.Surface(rim));
            }

            if (ring < rings - 1)
            {
                float slice = 1.0f / (rings - 1);
                float bend = (settings.stalkArch + sway.x) * slice * (1.0f - along * 0.35f);
                float twist = sway.y * slice;
                tangent = (Quaternion.AngleAxis(bend * Mathf.Rad2Deg, side)
                           * Quaternion.AngleAxis(twist * Mathf.Rad2Deg, Vector3.up) * tangent).normalized;
                side = Vector3.Cross(tangent, Vector3.Cross(side, tangent)).normalized;
                cursor += tangent * step;
            }
        }

        for (int ring = 0; ring < rings - 1; ring++)
        {
            int lower = first + ring * Sides;
            int upper = lower + Sides;
            for (int face = 0; face < Sides; face++)
            {
                int next = (face + 1) % Sides;
                mesh.AddQuad(lower + face, lower + next, upper + next, upper + face);
            }
        }

        return new Pose { seat = cursor, lean = Quaternion.FromToRotation(Vector3.up, tangent) };
    }
}
