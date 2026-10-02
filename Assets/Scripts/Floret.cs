using UnityEngine;

public struct FloretPlan
{
    public Vector3 root;
    public Vector3 heading;
    public Vector3 flank;
    public float length;
    public float width;
    public float curl;
    public Color fill;
}

public static class Floret
{
    private const int Segments = 5;
    private const float ToothDepth = 0.10f;
    private const float TipShare = 0.86f;

    private static float Teeth(int tooth, int teeth)
    {
        float wave = Mathf.Cos(Mathf.PI * (2.0f * tooth / Mathf.Max(teeth - 1, 1) - 1.0f));
        return 1.0f - ToothDepth * 0.5f * (1.0f - wave);
    }

    public static void Build(MeshBuffer mesh, DandelionSettings settings, FloretPlan plan)
    {
        int teeth = Mathf.Max(settings.floretTeeth, 2);
        int first = mesh.VertexCount;
        Vector3 cursor = plan.root;
        Vector3 tangent = plan.heading;
        float step = plan.length / Segments;
        Vector3 leftEdge = cursor;
        Vector3 rightEdge = cursor;

        for (int ring = 0; ring <= Segments; ring++)
        {
            float along = ring / (float)Segments;
            Vector3 normal = Vector3.Cross(plan.flank, tangent).normalized;
            float half = plan.width * 0.5f * Mathf.Lerp(0.55f, 1.0f, Mathf.Min(along / TipShare, 1.0f));

            leftEdge = cursor - plan.flank * half;
            rightEdge = cursor + plan.flank * half;
            mesh.AddVertex(leftEdge, Vector3.zero, Vector4.zero, plan.fill,
                StrokeKind.Card, 0.0f, settings.floretInk, settings.depthBias, Shading.Surface(normal));
            mesh.AddVertex(rightEdge, Vector3.zero, Vector4.zero, plan.fill,
                StrokeKind.Card, 0.0f, settings.floretInk, settings.depthBias, Shading.Surface(normal));

            if (ring > 0)
            {
                int lower = first + (ring - 1) * 2;
                mesh.AddQuad(lower, lower + 1, lower + 3, lower + 2);
            }

            tangent = Quaternion.AngleAxis(plan.curl / Segments, plan.flank) * tangent;
            cursor += tangent * step;
        }

        int rim = first + Segments * 2;
        Vector3 tip = (leftEdge + rightEdge) * 0.5f;
        Vector3 across = (rightEdge - leftEdge) * 0.5f;
        Vector3 ahead = tangent * (plan.width * ToothDepth);
        Vector3 crest = Vector3.Cross(plan.flank, tangent).normalized;

        int crown = mesh.VertexCount;
        for (int tooth = 0; tooth < teeth; tooth++)
        {
            float side = 2.0f * tooth / (teeth - 1) - 1.0f;
            mesh.AddVertex(tip + across * side + ahead * Teeth(tooth, teeth), Vector3.zero, Vector4.zero,
                plan.fill, StrokeKind.Card, 0.0f, settings.floretInk, settings.depthBias, Shading.Surface(crest));
        }

        for (int tooth = 0; tooth < teeth - 1; tooth++)
        {
            mesh.AddTriangle(rim, crown + tooth, crown + tooth + 1);
        }

        mesh.AddTriangle(rim, crown + teeth - 1, rim + 1);
    }
}
