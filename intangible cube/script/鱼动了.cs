using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class 鱼动了 : MonoBehaviour
{
    public float speed;
    public float speedX,speedZ;
    public 数据筛选和处理 data;
    public GameObject cameras;
    public GameObject fish;
    public float distanceOfInside=0.8f;
    public float lastChangingTime;
    public float timeRange;
    public bool quit = false;
    float quitTime;
    bool inside = false;
    bool preInside = false;
    public Slider distanceOfInsideSlider;
    // Start is called before the first frame update
    void Start()
    {
        resetAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - data.rodStartTime > 2)
        {
            float xPos = this.transform.position.x;
            float zPos = this.transform.position.z;
            xPos += speedX * Time.deltaTime;
            zPos += speedZ * Time.deltaTime;
            xPos = Mathf.Clamp(xPos, -0.7f, 0.7f);
            zPos = Mathf.Clamp(zPos, -0.7f, 0.7f);

            this.transform.position = new Vector3(xPos, 0, zPos);
            fish.transform.position = this.transform.position;

        }
        else
        {
            this.transform.position = cameras.transform.position;
        }
        if (Time.time - lastChangingTime > timeRange)
        {
            resetAll();
        }
        inSides();
    }
    void inSides()
    {
        if (Mathf.Abs(cameras.transform.position.x-this.transform.position.x)+ Mathf.Abs(cameras.transform.position.z - this.transform.position.z) > distanceOfInside)
        {
            inside = false;
            if (preInside == true)
            {
                quitTime = Time.time;
            }
            preInside = false;
            if (Time.time - quitTime > 0.5)
            {
                quit = true;
            }
        }
        else
        {
            quitTime = Time.time;
            inside = true;
            preInside = true;
        }
       
    }
    void resetAll()
    {
        speedX = Random.Range(-5,5)/10.0f*speed;
        speedZ = Random.Range(-5, 5) / 10.0f*speed;
        lastChangingTime = Time.time;
        timeRange = Random.Range(6, 10) / 3.0f;
    }
    public void changeValue()
    {
        distanceOfInside = distanceOfInsideSlider.value;
    }
}
