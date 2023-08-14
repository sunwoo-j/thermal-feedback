using System.Collections;
using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    public string portName = "COM3";
    public int baudRate = 9600;
    public HeatReceptor heatReceptor;
    private SerialPort serialPort;

    private void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    private void Update()
    {
        // Get temperature from the HeatReceptor script
        int temperature = heatReceptor.GetCurrentTemperature();

        if (temperature > 45) 
        {
            temperature = 45;
        }

        // Send temperature to Arduino
        serialPort.WriteLine(temperature);
    }

    private void OnDestroy()
    {
        serialPort.Close();
    }
}
