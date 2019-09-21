using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeFrames
{

    public static int ToFrames(float seconds)
    {
        return Mathf.FloorToInt(seconds * 60);
    }

    public static float ToSeconds(int frames)
    {
        return Mathf.Floor(frames / 60f);
    }
}
