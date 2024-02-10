using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [Header("Item Info")]
    [Range(0, 25)]
    public int energy = 5;
    public float price = 500;
}
