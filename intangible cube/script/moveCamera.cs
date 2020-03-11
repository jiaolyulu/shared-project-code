using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    public receiveData port;
    
    ArrayList arraylist;
    int arrayLength = 3;
    public int maxRange=100;
    float zPos=1000.0f;
    // Start is called before the first frame update
    float mapping(float x, float a, float b, float c, float d)
    {
        return (x - a) / (b - a) * (d - c) + c;
    }
    void Start()
    {
        arraylist = new ArrayList();
        for (int i = 0; i < arrayLength; i++)
        {
            arraylist.Add(0);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        int distance = port.dist;
        arraylist.Insert(arrayLength, distance);
        arraylist.RemoveRange(0, 1);
        int sum = 0;
        int count = 0;
        //Debug.Log(distance);
        foreach (int dist in arraylist)
        {
            if (dist < maxRange)
            {
                sum += dist;
                count++;
            }
        }
        
        if (count != 0) {
            zPos = sum/count;
           // Debug.Log(count);
            zPos = mapping(zPos,20,80,1,-1);
        }
       
        if (zPos!=1000.0f)
        {
            this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,zPos);
        }

    }
}
