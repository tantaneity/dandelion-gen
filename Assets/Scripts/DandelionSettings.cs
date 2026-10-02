using UnityEngine;

public sealed class DandelionSettings
{
    public int seed = 1;

    public float acheneLength = 0.030f;
    public float acheneRadius = 0.0034f;
    public int acheneSides = 9;
    public int acheneRings = 7;
    public float acheneTaper = 0.85f;

    public float beakLength = 0.075f;
    public float beakWidth = 0.0013f;
    public int beakNodes = 4;

    public int bristleCount = 64;
    public int bristleNodes = 5;
    public float bristleLength = 0.055f;
    public float bristleWidth = 0.0009f;
    public float bristleSpread = 52.0f;
    public float bristleLift = 26.0f;
    public float bristleJitter = 7.0f;
    public float bristleInk = Outline.Detail;
    public float beakInk = Outline.Small;

    public float phase = 0.0f;

    public float depthBias = 0.0f;
    public float outlineWeight = Outline.Small;
}
