using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 变化球的位置 : MonoBehaviour
{
    public string dir="";
    public float rangeA;
    public float rangeB;
    public receiveData port;
    public int index=-1;
    float mapping(float x, float a, float b, float c, float d)
    {
        return (x - a) / (b - a) * (d - c) + c;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((index != -1)&(dir!="")&(port.data.Length==6))
        {
            int dataNumber = port.data[index];
            float num = mapping(dataNumber,10,90,rangeA,rangeB);
            if (dataNumber > 90)
            {
                gameObject.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<Renderer>().enabled = true;
                if (dir == "x")
                {
                    this.transform.position = new Vector3(num, this.transform.position.y, this.transform.position.z);
                }
                if (dir == "z")
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, num);
                }
            }
            
        }
        
    }
}
