using UnityEngine;

public struct Gust
{
    public float frequency;
    public float strength;
    public float wavelength;
    public float phase;
}

public static class Wind
{
    private const float MinimumNatural = 0.05f;
    private const float MinimumHeight = 1e-3f;
    private const float CrossShare = 0.22f;
    private const float CrossFrequencyRatio = 1.37f;
    private const float Tau = Mathf.PI * 2.0f;

    public static float NaturalFrequency(float height, float stiffness)
    {
        float reach = Mathf.Max(height, MinimumHeight);
        return Mathf.Max(stiffness / (reach * reach), MinimumNatural);
    }

    public static float Susceptibility(float height, float stiffness)
    {
        float reach = Mathf.Max(height, MinimumHeight);
        return reach * reach * reach / Mathf.Max(stiffness, MinimumNatural);
    }

    public static float ResponseGain(float driveFrequency, float naturalFrequency, float damping)
    {
        float ratio = driveFrequency / naturalFrequency;
        float offset = 1.0f - ratio * ratio;
        float loss = 2.0f * damping * ratio;
        return 1.0f / Mathf.Sqrt(offset * offset + loss * loss);
    }

    public static float ResponseLag(float driveFrequency, float naturalFrequency, float damping)
    {
        float ratio = driveFrequency / naturalFrequency;
        return Mathf.Atan2(2.0f * damping * ratio, 1.0f - ratio * ratio);
    }

    public static Vector2 Bend(Gust[] gusts, float height, float stiffness, float damping,
        float travel, float seconds)
    {
        float natural = NaturalFrequency(height, stiffness);
        float give = Susceptibility(height, stiffness);
        float along = 0.0f;
        float across = 0.0f;

        foreach (Gust gust in gusts)
        {
            float gain = ResponseGain(gust.frequency, natural, damping);
            float lag = ResponseLag(gust.frequency, natural, damping);
            float beat = Tau * (gust.frequency * seconds - travel / Mathf.Max(gust.wavelength, MinimumHeight))
                         + gust.phase - lag;

            along += gust.strength * gain * Mathf.Sin(beat);

            float crossNatural = natural * CrossFrequencyRatio;
            float crossGain = ResponseGain(gust.frequency, crossNatural, damping);
            float crossLag = ResponseLag(gust.frequency, crossNatural, damping);
            across += gust.strength * crossGain * CrossShare
                      * Mathf.Sin(Tau * gust.frequency * seconds * CrossFrequencyRatio
                                  + gust.phase * CrossFrequencyRatio - crossLag);
        }

        return new Vector2(along, across) * give;
    }
}
