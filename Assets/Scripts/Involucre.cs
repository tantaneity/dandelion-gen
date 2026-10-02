using UnityEngine;

public static class Involucre
{
    private const float FullTurn = 360.0f;
    private const float InnerLift = 1.35f;

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette, Pose pose, float open)
    {
        for (int ring = 0; ring < 2; ring++)
        {
            bool isInner = ring == 1;
            int count = isInner ? settings.bractCount : settings.bractCount - 3;
            float bend = isInner
                ? Mathf.Lerp(settings.bractClosed, settings.bractInnerBend, open)
                : Mathf.Lerp(settings.bractClosed, settings.bractOuterBend, open);
            float length = settings.bractLength * (isInner ? InnerLift : 1.0f);

            for (int index = 0; index < count; index++)
            {
                float angle = FullTurn * index / count + (isInner ? 0.0f : FullTurn * 0.5f / count);
                Vector3 outward = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.right;
                Vector3 heading = Quaternion.AngleAxis(bend, Vector3.Cross(Vector3.up, outward)) * Vector3.up;
                float draw = Seed.Hash(index + ring * 97, settings.seed + 31);

                Floret.Build(mesh, settings, new FloretPlan
                {
                    root = pose.Place(outward * (settings.receptacleRadius * 0.92f)
                                      - Vector3.up * settings.bractSeat),
                    heading = pose.Aim(heading.normalized),
                    flank = pose.Aim(Vector3.Cross(heading, outward).normalized),
                    length = length * (1.0f + (draw - 0.5f) * 0.18f),
                    width = settings.bractWidth,
                    curl = isInner ? 0.0f : settings.bractCurl * open,
                    fill = isInner ? palette.bractInner : palette.bract
                });
            }
        }
    }
}
