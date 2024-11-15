using UnityEngine;
using System.IO;
using System.IO.Ports;
using System;

/**
 * ReadFromArduino
 * 
 * Reads data from an Arduino from the serial port,
 * then uses the data to rotate the object.
 */
public class ReadFromArduino : MonoBehaviour
{

    /**
     * The name of the port the Arduino is connected to
     * On Windows, this will be something like "COM3"
     * On Mac, this will be something like "/dev/cu.usbmodem101"
     * You can supply this value in the Unity Editor as well
     */
    public string portName = "/dev/cu.usbmodem101";
    /** This is the stream that will read from the serial port
     * We used a similar class to read from a CSV file in another assignment
     * The StreamReader class is used to read from a stream of data,
     * which can be a file, a network connection, or in this case, a serial port.
     */
    private StreamReader stream;
    private string line;


    // Define and flush the stream.
    void Start()
    {
        stream = GetStream();
        stream.BaseStream.Flush();
    }

    void Update()
    {

        // Bail on this frame if the stream is null


        if (stream == null)
            stream = GetStream();

        line = stream.ReadLine();

        // Bail on this frame if the line is null
        // Not 100% necessary, but it's good to be safe
        if (line == null)
        {
            return;
        }

        // Split the line into two values
        // This script assumes that the Arduino 
        // is sending two values separated by a comma: "x,y"
        string[] vals = line.Split(',');

        // Bail on this frame if there are not two values
        // This indicates that the Arduino is not sending the data we expect
        if (vals.Length != 2)
        {
            return;
        }

        // Bail on this frame if the values
        // cannot be parsed as floats
        if (!float.TryParse(vals[0], out float x) || !float.TryParse(vals[1], out float y))
        {
            return;
        }

        // Assign the values to x and y
        x = float.Parse(vals[0]);
        y = float.Parse(vals[1]);

        // Rotate the object
        transform.rotation = Quaternion.Euler(transform.rotation.x + x, 0, transform.rotation.y + y);

    }

    StreamReader GetStream()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
        {
            if (portName == "/dev/cu.usbmodem101")
            {
                portName = "COM3"; // Default to COM3 if no port is specified on Windows
            }
            SerialPort port = new SerialPort(portName, 9600);
            port.Open();
            stream = new StreamReader(port.BaseStream);
        }
        else if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX)
        {
            stream = new(portName);
        }

        // Unclog any data that may be in the buffer
        // We won't be able to read it anyway
        stream.BaseStream.Flush();
        return stream;

    }

}
