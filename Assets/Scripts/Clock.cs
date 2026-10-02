using UnityEngine;

public static class Clock
{
    private const float FullTurn = 360.0f;

    public static void Build(MeshBuffer mesh, DandelionSettings settings, DandelionPalette palette, Pose pose, float seconds)
    {
        float flatten = Receptacle.Flatten(settings, settings.blow);
        Receptacle.Build(mesh, settings, palette, pose, flatten, settings.blow);

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
                Receptacle.Pit(mesh, settings, palette, pose, heading, flatten, settings.blow);
                continue;
            }

            float wobble = Seed.Hash(index, settings.seed + 11) - 0.5f;
            Seed.Build(mesh, settings, palette, new SeedPlan
            {
                root = pose.Place(Receptacle.Skin(settings, heading, flatten, settings.blow)) + flight.offset,
                heading = pose.Aim(Vector3.Slerp(heading, flight.heading, flight.away).normalized),
                scale = (1.0f + wobble * settings.seedJitter) * Mathf.Lerp(0.42f, 1.0f, settings.open),
                roll = index * Phyllotaxis.GoldenAngle,
                open = settings.open
            });
        }
    }
}
