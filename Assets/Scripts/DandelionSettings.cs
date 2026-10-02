using UnityEngine;

public sealed class DandelionSettings
{
    public int seed = 1;

    public float acheneLength = 0.024f;
    public float acheneRadius = 0.0028f;
    public int acheneSides = 9;
    public int acheneRings = 7;
    public float acheneTaper = 0.85f;

    public float beakLength = 0.075f;
    public float beakWidth = 0.0013f;
    public int beakNodes = 4;

    public int bristleCount = 44;
    public int bristleNodes = 5;
    public float bristleLength = 0.047f;
    public float bristleWidth = 0.00062f;
    public float bristleSpread = 52.0f;
    public float bristleLift = 26.0f;
    public float bristleJitter = 7.0f;
    public float bristleInk = 0.05f;
    public float beakInk = Outline.Small;

    public int clockSeeds = 125;
    public float clockFloor = -0.72f;
    public float receptacleRadius = 0.021f;
    public float seedJitter = 0.14f;
    public float clockDome = 1.0f;
    public float bloomDome = 0.42f;

    public float blow = 0.0f;
    public float blowSpread = 1.6f;
    public float blowSpeed = 1.4f;
    public float blowRange = 1.3f;
    public float blowSway = 2.6f;
    public Vector2 blowHeading = new Vector2(1.0f, 0.25f);

    public int floretCount = 265;
    public float floretLength = 0.062f;
    public float floretWidth = 0.0054f;
    public int floretTeeth = 5;
    public float floretRise = 49.0f;
    public float floretSplay = 87.0f;
    public float floretCurl = -9.0f;
    public float floretJitter = 0.16f;
    public float floretShadeJitter = 0.13f;
    public float floretInk = Outline.Small;

    public int bractCount = 16;
    public float bractLength = 0.038f;
    public float bractWidth = 0.0052f;
    public float bractSeat = 0.016f;
    public float bractInnerBend = 19.0f;
    public float bractClosed = 5.0f;
    public float bractOuterBend = 128.0f;
    public float bractCurl = 26.0f;

    public float stalkHeight = 0.42f;
    public float stalkRadius = 0.0046f;
    public int stalkRings = 14;
    public float stalkArch = 0.22f;
    public float stalkStiffness = 0.9f;
    public float stalkDamping = 0.26f;

    public float windStrength = 0.35f;
    public float gustFrequency = 0.55f;
    public float gustWavelength = 2.4f;
    public int gustCount = 3;
    public float gustSpread = 1.7f;
    public float gustFalloff = 0.6f;
    public Vector2 windHeading = new Vector2(1.0f, 0.25f);

    public float open = 1.0f;
    public float grey = 0.0f;

    public float phase = 0.0f;

    public float depthBias = 0.0f;
    public float outlineWeight = Outline.Small;
}
