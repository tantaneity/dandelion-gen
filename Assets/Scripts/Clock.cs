using UnityEngine;

public static class Clock
{
    private const float FullTurn = 360.0f;

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette, Pose pose, float seconds)
    {
        Receptacle.Build(mesh, settings, palette, pose, settings.clockDome);

        for (int index = 0; index < settings.clockSeeds; index++)
        {
            Vector3 heading = Phyllotaxis.OnSphere(index, settings.clockSeeds);
            if (heading.y < settings.clockFloor)
            {
                continue;
            }

            Flight flight = Dispersal.Follow(settings, index, settings.blow, seconds);
            if (flight.isGone)
            {
                continue;
            }

            float wobble = Seed.Hash(index, settings.seed + 11) - 0.5f;
            Seed.Build(mesh, settings, palette, new SeedPlan
            {
                root = pose.Place(heading * settings.receptacleRadius) + flight.offset,
                heading = pose.Aim(Vector3.Slerp(heading, flight.heading, flight.away).normalized),
                scale = (1.0f + wobble * settings.seedJitter) * Mathf.Lerp(0.42f, 1.0f, settings.open),
                roll = index * Phyllotaxis.GoldenAngle,
                open = settings.open
            });
        }
    }
}
