﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 教学模式 : MonoBehaviour
{
    public Text onoff;
    public bool mode = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeMode()
    {
        mode = !mode;
        if (mode)
        {
            onoff.text = "教学模式中";
            
        }
        else
        {
            onoff.text = "游戏模式中";
        }
    }
}
