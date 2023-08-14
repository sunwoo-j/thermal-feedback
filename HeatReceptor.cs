using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Hand
{
    Left,
    Right
}

public class HeatReceptor : MonoBehaviour
{   
    public Hand whichHand;
    [Range(25, 35)]
    public int initialTemperature = 33; // The temperature of the heat receptor.
    [Range(0f, 10f)]
    public float maxDistance = 5f; // The maximum distance within which heat transfer occurs.
    public TMP_Text temperatureText; // Reference to the TextMeshPro component.

    private int currentTemperature;
    private float distance;

    private void Update()
    {
        // Find all HeatSource objects in the scene.
        HeatSource[] heatSources = FindObjectsOfType<HeatSource>();

        int sourceCount = 0;
        int cumTemperature = 0;
        int conductTemperature = initialTemperature;
        int averageTemperature = 0;

        // Calculate the temperature based on radiative heat transfer from each HeatSource.
        foreach (HeatSource heatSource in heatSources)
        {
            distance = Vector3.Distance(heatSource.transform.position, transform.position);

            if (heatSource.heatTransferType.Equals(HeatTransferType.Conduction) && distance < 0.13f)
            {
                conductTemperature = heatSource.temperature;
                break;
            }
            else if (distance <= maxDistance && heatSource.heatTransferType.Equals(HeatTransferType.Radiation))
            {
                sourceCount++;
                int radiativeHeat = (int)CalculateRadiativeHeat(heatSource.temperature, distance, heatSource.emissivity);
                cumTemperature += radiativeHeat;
            }
        }

        if (sourceCount > 0)
        {
            averageTemperature = (int)cumTemperature/sourceCount;
        }

        if (conductTemperature > averageTemperature || conductTemperature < initialTemperature)
        {
            currentTemperature = conductTemperature;
        }
        else if (averageTemperature < initialTemperature)
        {
            currentTemperature = initialTemperature;
        }
        else
        {
            currentTemperature = averageTemperature;
        }

        // Update the temperature display on the TextMeshPro object.
        temperatureText.text = currentTemperature.ToString() + "°C";
    }

    public float GetCurrentTemperature()
    {
        return currentTemperature;
    }

    // Calculate radiative heat transfer between the heat source and receptor using the Stefan-Boltzmann law.
    private float CalculateRadiativeHeat(int heatSourceTemperature, float distance, float emissivity)
    {
        float radiativeHeat = heatSourceTemperature * Mathf.Pow(emissivity, 0.25f) * ((maxDistance - Mathf.Pow(distance + 0.5f, 4f)) / maxDistance);
        if (radiativeHeat < initialTemperature)
        {
            return initialTemperature;
        }
        else
        {
            return radiativeHeat;
        }
    }
}
