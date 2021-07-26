using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
public class receiveData : MonoBehaviour
{
    public int dist=0;
    SerialPort stream = new SerialPort("COM11", 115200); //Set the port (com4) and the baud rate (9600, is standard on most devices)
    public int[] data;

    void Start()
    {
        stream.Open(); //Open the Serial Stream.
    }

    // Update is called once per frame
    void Update()
    {
        string value = stream.ReadLine(); //Read the information
        
        string[] info = value.Split(','); 
        if (info.Length == 12)
        {
            data = new int[6];
            for (int index = 0; index < 6; index++)
            {
                int nowindex = int.Parse(info[index * 2]);
                data[nowindex - 2] = int.Parse(info[index * 2 + 1]);

            }
          
        }
      
        
    }
    void OnApplicationQuit()
    {
        stream.Close();
        Debug.Log("Port closed!");
    }
}
