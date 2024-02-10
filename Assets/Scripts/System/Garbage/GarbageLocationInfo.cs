using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GarbageType
{
    TOXIC, NORMAL
}

public class GarbageLocationInfo : MonoBehaviour
{

    [Header("Garbage Settings")]
    public GarbageType garbageType = GarbageType.NORMAL;
    [Range(1, 10000)]
    public int garbagePerLocation = 100;
    [Range(1, 100)]
    public int garbageAmountRandomness = 10;
}
