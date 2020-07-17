using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RangeFloat
{
    public float minValue;
    public float maxValue;

    public RangeFloat(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public float RandomValue
    {
        get { return Random.Range(minValue, maxValue); }
    }

    public bool IsInRange(float value)
    {
        return value >= minValue && value <= maxValue;
    }
}
