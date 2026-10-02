using UnityEngine;

public struct Flight
{
    public Vector3 offset;
    public Vector3 heading;
    public float away;
    public bool isGone;
}

public static class Dispersal
{
    private const float LeaveSpan = 0.35f;
    private const float Drift = 0.55f;
    private const float Rise = 0.22f;
    private const float Tumble = 48.0f;

    private static float Leaving(DandelionSettings settings, int index, float blow)
    {
        if (blow <= 0.0f)
        {
            return 0.0f;
        }

        float draw = Seed.Hash(index, settings.seed + 7);
        float due = draw * settings.blowSpread;
        return Mathf.Clamp01((blow * (1.0f + settings.blowSpread) - due) / LeaveSpan);
    }

    public static Flight Follow(DandelionSettings settings, int index, float blow, float seconds)
    {
        float gone = Leaving(settings, index, blow);
        if (gone <= 0.0f)
        {
            return new Flight { offset = Vector3.zero, heading = Vector3.up, away = 0.0f, isGone = false };
        }

        Vector2 course = settings.blowHeading.normalized;
        Vector3 along = new Vector3(course.x, 0.0f, course.y);
        float reach = gone * gone * settings.blowSpeed;
        float sway = Mathf.Sin((seconds + index * 0.37f) * settings.blowSway) * Drift * gone;

        Vector3 drifted = along * reach
                          + Vector3.up * (Rise * reach)
                          + Vector3.Cross(Vector3.up, along) * sway;

        Vector3 facing = Quaternion.AngleAxis(Tumble * gone * (Seed.Hash(index, settings.seed + 3) - 0.5f),
            Vector3.Cross(Vector3.up, along)) * Vector3.up;

        return new Flight
        {
            offset = drifted,
            heading = facing,
            away = gone,
            isGone = reach > settings.blowRange
        };
    }
}
