using UnityEngine;

public static class Phyllotaxis
{
    public const float GoldenAngle = 137.507764f;

    public static Vector3 OnSphere(int index, int count)
    {
        float span = Mathf.Max(count, 1);
        float height = 1.0f - 2.0f * (index + 0.5f) / span;
        float ring = Mathf.Sqrt(Mathf.Max(1.0f - height * height, 0.0f));
        float angle = index * GoldenAngle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle) * ring, height, Mathf.Sin(angle) * ring);
    }

    public static Vector2 OnDisc(int index, int count)
    {
        float span = Mathf.Max(count, 1);
        float reach = Mathf.Sqrt((index + 0.5f) / span);
        float angle = index * GoldenAngle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * reach;
    }
}
