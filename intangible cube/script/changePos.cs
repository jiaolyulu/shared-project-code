using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changePos : MonoBehaviour
{
    private bool fish;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Time.time);
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(Time.time);
        if (fish)
        {
            this.transform.position = new Vector3(Random.Range(-1.0f,1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
            fish = false;
        }
        
    }
    public void ChangePos()
    {
        fish = true;

    }
}
