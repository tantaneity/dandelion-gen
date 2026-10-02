using UnityEngine;

public static class Breeze
{
    private const float FullTurn = Mathf.PI * 2.0f;

    public static Gust[] Gusts(DandelionSettings settings)
    {
        Gust[] gusts = new Gust[Mathf.Max(settings.gustCount, 1)];
        for (int i = 0; i < gusts.Length; i++)
        {
            float step = Mathf.Pow(settings.gustSpread, i);
            gusts[i] = new Gust
            {
                frequency = settings.gustFrequency * step,
                strength = settings.windStrength * Mathf.Pow(settings.gustFalloff, i),
                wavelength = settings.gustWavelength / step,
                phase = Seed.Hash(i, settings.seed + 41) * FullTurn
            };
        }

        return gusts;
    }
}
