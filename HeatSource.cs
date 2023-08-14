using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeatTransferType
{
    Radiation,
    Conduction
}

public class HeatSource : MonoBehaviour
{
    public HeatTransferType heatTransferType;
    [Range(-15, 100)]
    public int temperature;
    [Range(0f, 1f)]
    public float emissivity = 0.5f;
}
