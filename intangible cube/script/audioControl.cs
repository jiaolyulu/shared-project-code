using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioControl : MonoBehaviour
{
    public AudioSource source;
    private bool drop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void Drop()
    {
        source.Play();
    }
}
