using UnityEngine;

public static class Lifecycle
{
    private const float BudUntil = 0.13f;
    private const float OpenUntil = 0.33f;
    private const float BloomUntil = 0.48f;
    private const float ShutUntil = 0.60f;
    private const float GreyAt = 0.64f;
    private const float FluffUntil = 0.80f;

    private static float Ease(float from, float to, float life)
    {
        return Mathf.SmoothStep(0.0f, 1.0f, Mathf.Clamp01((life - from) / (to - from)));
    }

    public static void Apply(DandelionSettings settings, float life)
    {
        settings.grey = life >= GreyAt ? 1.0f : 0.0f;

        if (life < GreyAt)
        {
            settings.open = Ease(BudUntil, OpenUntil, life) * (1.0f - Ease(BloomUntil, ShutUntil, life));
            settings.blow = 0.0f;
            return;
        }

        settings.open = Ease(GreyAt, FluffUntil, life);
        settings.blow = Ease(FluffUntil, 1.0f, life);
    }
}
