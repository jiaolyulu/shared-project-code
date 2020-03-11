using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
public class receiveData : MonoBehaviour
{
 
    public string state;
    public int dist=0;
    SerialPort stream = new SerialPort("COM11", 115200); //Set the port (com4) and the baud rate (9600, is standard on most devices)
    float[] lastRot = { 0, 0, 0 }; //Need the last rotation to tell how far to spin the camera
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
           // Debug.Log(info);
        }
        //switch (value)
        //{
        //    case "System error":
        //        state = "System error";
        //        break;
        //    case "ECE failure":
        //        state = "ECE failure";
        //        break;
        //    case "No convergence":
        //        state = "No convergence";
        //        break;
        //    case "Ignoring range":
        //        state = "Ignoring range";
        //        break;
        //    case "Signal/Noise error":
        //        state = "Signal/Noise error";
        //        break;
        //    case "Raw reading underflow":
        //        state = "Raw reading underflow";
        //        break;
        //    case "Raw reading overflow":
        //        state = "Raw reading overflow";
        //        break;
        //    case "Range reading underflow":
        //        state = "Range reading underflow";
        //        break;
        //    case "Range reading overflow":
        //        state = "Range reading overflow";
        //        break;
        //    default:
        //        state = "Number";
        //        break;
        //}
        
    }

    void OnGUI()
    {
        string newString = "Connected: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z;
        GUI.Label(new Rect(10, 10, 300, 100), newString); //Display new values
                                                          // Though, it seems that it outputs the value in percentage O-o I don't know why.
    }
    void OnApplicationQuit()
    {
        stream.Close();
        Debug.Log("Port closed!");
    }
}
