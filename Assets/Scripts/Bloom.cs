using UnityEngine;

public static class Bloom
{
    private const float FullTurn = 360.0f;
    private const float OuterShare = 0.55f;

    private static Color Shade(DandelionSettings settings, DandelionPalette palette, int index, float reach)
    {
        float draw = Seed.Hash(index, settings.seed + 23);
        Color petal = Color.Lerp(palette.floretHeart, palette.floret, Mathf.SmoothStep(0.0f, 1.0f, reach));
        float lift = 1.0f + (draw - 0.5f) * settings.floretShadeJitter;
        return new Color(petal.r * lift, petal.g * lift, petal.b * lift, 1.0f);
    }

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette, Pose pose, float open)
    {
        Receptacle.Build(mesh, settings, palette, pose, settings.bloomDome);

        for (int index = 0; index < settings.floretCount; index++)
        {
            Vector2 spot = Phyllotaxis.OnDisc(index, settings.floretCount);
            float reach = spot.magnitude;
            Vector3 outward = new Vector3(spot.x, 0.0f, spot.y).normalized;
            if (outward.sqrMagnitude < 0.5f)
            {
                outward = Vector3.right;
            }

            float dome = Mathf.Sqrt(Mathf.Max(1.0f - reach * reach, 0.0f)) * settings.receptacleRadius;
            Vector3 root = outward * (reach * settings.receptacleRadius) + Vector3.up * dome;

            float splay = Mathf.Lerp(settings.floretRise, settings.floretSplay,
                Mathf.SmoothStep(0.0f, 1.0f, reach)) * open;
            Vector3 heading = Quaternion.AngleAxis(splay, Vector3.Cross(Vector3.up, outward)) * Vector3.up;
            float draw = Seed.Hash(index, settings.seed + 5);

            Floret.Build(mesh, settings, new FloretPlan
            {
                root = pose.Place(root),
                heading = pose.Aim(heading.normalized),
                flank = pose.Aim(Vector3.Cross(heading, outward).normalized),
                length = settings.floretLength * Mathf.Lerp(1.0f, OuterShare + 0.45f, reach)
                         * (1.0f + (draw - 0.5f) * settings.floretJitter),
                width = settings.floretWidth,
                curl = settings.floretCurl * open,
                fill = Shade(settings, palette, index, reach)
            });
        }
    }
}
