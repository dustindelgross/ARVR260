#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>
/**
 * The first two includes are from a library for the MPU6050
 * That library is called "MPU6050_light" by refetick
 * 
 * The third include is part of a library for the I2C communication,
 * I2C stands for the Inter-Integrated Circuit protocol.
 * It is a protocol for communication between
 * microcontrollers and peripherals over short distances.
 */

// Declare an object of the class Adafruit_MPU6050
Adafruit_MPU6050 mpu;
// Declare a string to store the gyro readings
String s;

void setup(void)
{
    /**
     * Initialize the serial communication
     * with a baud rate of 9600.
     *
     * If you set the baud rate too high,
     * the port will end up segmenting the frames,
     * resulting in fragments of data
     * that Unity won't be able to read line-by-line.
     */
    Serial.begin(9600);
    while (!Serial)
        delay(10);

    if (!mpu.begin())
    {
        Serial.println("Failed to find MPU6050 chip");
        while (1)
        {
            delay(10);
        }
    }

    // Set the range of the accelerometer and gyroscope
    // You don't need to change these values
    mpu.setAccelerometerRange(MPU6050_RANGE_8_G);
    mpu.setGyroRange(MPU6050_RANGE_2000_DEG);
    mpu.setFilterBandwidth(MPU6050_BAND_21_HZ);

    delay(100);
}

void loop()
{

    // These are the variables that will store the readings
    sensors_event_t a, g, temp;
    mpu.getEvent(&a, &g, &temp); // Get the readings from the MPU6050
    s = g.gyro.x; // Store the x-axis reading
    s.concat(","); // Add a comma to separate the values
    s.concat(g.gyro.z); // Store the z-axis reading
    Serial.println(s); // Print the readings to the serial output

    delay(200);
}