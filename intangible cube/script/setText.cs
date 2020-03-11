using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setText : MonoBehaviour
{
    public Slider slide;
    public Text texts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateText()
    {
       texts.text = slide.value.ToString();
    }
}
