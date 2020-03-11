using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 数据筛选和处理 : MonoBehaviour
{
    public GameObject camera;
    //public GameObject ganRod;

    float dropOutTime;

    float xPos, zPos;
    public int state;
    public receiveData port;
    bool dipIn = false;
    bool preDipIn = false;
    public AudioSource dip;
    public AudioSource environment;

    public AudioSource fish;
    public float fishEatingTime;
    public bool fishInside;//判断吊钩是否在鱼的范围内。
    public float smoothDistance = 0.2f;
    int[] d;
    int[] preD;
    public int xMinIndex, zMinIndex;
    //判断运动是否稳定
    bool smoothMovement,preSmoothMovement;
    float prexPos = -1.0f;float prezPos=-1.0f;
    public float startStableTime=0;

    public float rodStartTime = 0;
    public AudioSource rod;
    bool preRod = false;
    bool nowRod;//记录鱼杆之前是否处于稳定状态
    public 鱼动了 fishrod;

    public AudioSource splash;
    public AudioSource gotFish;
    public AudioSource fishEscape;


    public Text inside;
    public Text smooth;
    public Text stretchLine;
    public Text pitchText;
    public Text gotText;
    public Text noFishText;
    public Slider smoothDistanceSlider;
    float gotFishTime, lostFishTime;
    int findMin(int a, int b, int c)        // this is a function for finding the smallest value of a,b,c and return the sensor's index which has the smallest value.
    {
        int x;
        if (d[a] < d[b])
        {
            x = a;

        }
        else
        {
            x = b;
        }
        if (d[x] > d[c])
        {
            x = c;
        }
        return x;
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        d = port.data;
        preD = new int[6]{ 100,100,100,100,100,100};
        dipIn = false;
        
        if (d.Length == 6)//如果数据正常          // if the length of the data received by arduino is in the length of 6, that means the sensor is probably working fine.
        {
            

            dipInto();
            
            if (dipIn == true) //如果在水中
            {
                xMinIndex = findMin(0, 1, 3);           // it is for finding the smallest value of this 3 sensors. it is recording the index of the sensor that has the smallest value. This is for x axis.
                zMinIndex = findMin(2, 4, 5);           // it is for finding the smallest value of this 3 sensors. it is recording the index of the sensor that has the smallest value. This is for z axis.
                bool xState = true;
                bool zState = true;
                if (d[0]>60& d[1] > 60 & d[3] > 60 )        // if all sensor value is more than 60, that means that the sensor is not reading correctly.
                {
                    xState = false;
                }
                if (d[2] > 60 & d[4] > 60 & d[5] > 60)
                {
                    zState = false;
                }
                if (xState & zState)
                {
                    xPos = GameObject.Find("传感器数据可视化/Sphere (" + (xMinIndex + 1).ToString() + ")").transform.position.x;                //I am adding a seperate script to this gameObject that include a formula of translating the sensor value into the right position.
                    zPos = GameObject.Find("传感器数据可视化/Sphere (" + (zMinIndex + 1).ToString() + ")").transform.position.z;
                    camera.transform.position = new Vector3(xPos, 0, zPos);             //By transforming the position to the game object's positon, we can map the real world to the virtual world.
                    stableState();
                    prexPos = xPos;
                    prezPos = zPos;
                }
               
            }
            else
            {
                
               
                //that means that something is not inside. 
                //就说明没有在钓鱼状态，就停止鱼的声音。只有环境音。
                if (state == 1)
                {
                    state = 0;
                    splash.Play();      //if it is from a state of no fish at the hook ,then they would only hear a slight splash sound effect.
                }
                if (state == 2)
                {
                    //the state equals 2 means that there is fish at the hook's place when the user lift the rod. But you still have to figure out if the are lifting the rod at the right time.(which is during the time when the pitch getting higher)
                    //说明有鱼上钩，但是得判断是否成功。
                    state = 3;      //changing the state to 3, which can stop the state 2 so that other sound such as underwater sound and environment sound can know how to respond to it.
                    if (Time.time - rodStartTime - fishEatingTime > 0& Time.time - rodStartTime - fishEatingTime < 1.5f)        //this is to tell whether they are lifting the rod at the right time, which means that the pitch of the stretching sound would go higher for 1.5 seconds.
                    {
                        gotFish.Play();
                        gotFishTime = Time.time;
                    }
                    else
                    {
                        splash.Play();
                        fishEscape.Play();              //if the fishing rod is not lift at the right time, then play the sound of fish escape.
                        lostFishTime = Time.time;
                    }
                    
                    
                }
                
                startStableTime = Time.time;
            }
            rodVolume();//调整吊杆的音量       adjusting the volume of the stretching line of fishing rod
            fishRodPitchHigh();         //adjusting the pitch of the stretching line of fishing rod
            changingText();             //changing the text content on the screen to shown which step is going through
        }
        
        
    }

    void dipInto()
    {
        for (int i = 0; i < 6; i++)
        {
            if (d[i] < 90)
            {
                dipIn = true;           // if every sensor value is more than 90, that means that now the sensor is not detecting anything inside.
                break;
            }
        }

        if (dipIn == true & preDipIn == false& Time.time-dropOutTime>3)     //because sometimes the sensor is not detecting something even if the fish hook is inside. So I add that the start to detect something should be over the interval of 3 seconds after the detection of finding something moving out of the box.
        {
            dip.Play();                 //if it is the moment when the fish pool start to detect something, then play the sound of the water splash.
            Debug.Log("DipIN");
            
        }
        if (dipIn == false & preDipIn == true)
        {
            if (preD[1] > 90 & preD[5] > 90)
            {
                Debug.Log("DipOUT");
            }
            else
            {
                dipIn = true;
            }
        }
        preD = d;
        if (dipIn == false)             //if there is something inside, then change the environement sound of bird and animals into the underwater sounds.
        {
            environment.volume += 0.05f;
            environment.volume = Mathf.Min(1.0f,environment.volume);
            fish.volume -= 0.05f;
            fish.volume = Mathf.Max(0.01f, fish.volume);
        }
        else
        {
            dropOutTime = Time.time;
            environment.volume -= 0.05f;
            environment.volume = Mathf.Max(0.01f, environment.volume);
            if (state != 2)
            {
                fish.volume += 0.05f;
                fish.volume = Mathf.Min(1.0f, fish.volume);
            }
            
            
        }
        preDipIn = dipIn;
    }

    void stableState()
    {
        //判断是否读数稳定
        if (Mathf.Abs(prexPos - xPos) + Mathf.Abs(prezPos - zPos) > smoothDistance)     // it is to tell whether the current movement is stable by comparing the previous sensor value with the current ones.
        {
            smoothMovement = false;                 
            startStableTime = Time.time;            // it is recording the time that the fishing hook is being stable. By comparing the current time to this value, we can tell whether the hook has been stable for at least 2 seconds.
            state = 1;
        }
        else
        {
            smoothMovement = true;                  // it is recording the time that the fishing hook is being stable. By comparing the current time to this value, we can tell whether the hook has been stable for at least 2 seconds.
        }
        //如果稳定读数3秒以上
        if (preSmoothMovement == false & smoothMovement == true)
        {
            if (state != 2)
            {
                startStableTime = Time.time;        // it is recording the time that the fishing hook is being stable. By comparing the current time to this value, we can tell whether the hook has been stable for at least 2 seconds.
            }
            preSmoothMovement = smoothMovement;
        }

        if (preRod == false&nowRod==true&state!=2)
        {
            state = 2;
            rodStartTime = Time.time;                           //this is recording the start time of the stretching sound. So I can tell when I should raise up the pitch by comparing time with this variable.
            fishEatingTime = Random.Range(4, 15) / 6.0f;        //this would generate a random time for the pitch of the fishing line stretching to become higher, which would make the time of catching fish more random
            preRod = true;
        }
        if (Mathf.Abs(camera.transform.position.x-fish.transform.position.x)+ Mathf.Abs(camera.transform.position.z - fish.transform.position.z)<fishrod.distanceOfInside)
        {
            fishInside = true;
            fishrod.speed = 0.2f;               // if the fish is inside the range of the fishing hook, it would swim slower.
        }
        else
        {
            fishInside = false;
            fishrod.speed = 0.5f;               // if the fish is outside the range of the fishing hook, it would swim faster.
        }

        if (Time.time - startStableTime > 2 & smoothMovement == true&fishInside==true)   //this is to make sure that the fishing hook is kept still and inside the range of the fish for more than 2 seconds.
        {
            nowRod = true;    
           
           
        }
        else
        {
            nowRod = false;
            preRod = false;

        }
        
      
        if (fishrod.quit)
        {
            state = 1;
            fishrod.quit = false;
        }
    }

    void rodVolume()
    {
        if (state == 2)
        { //开始播放咬杆子的声音    playing the sound of lifting the rod and the fishing line is stretching with tension.
            rod.volume += 0.05f;
            rod.volume = Mathf.Min(1.0f, rod.volume);
        }
        else
        {
            rod.volume -= 0.1f;
            rod.volume = Mathf.Max(0.0f, rod.volume);
        }
    }
    void fishRodPitchHigh()
    {
        //播放提示可以提起杆子的声音  still playing the sound of fishing line pitch getting higher
        float startEating = Time.time - rodStartTime - fishEatingTime;
        if (state == 2 & startEating > 0 & startEating < 1.5f)
        {
            rod.pitch = 1.75f;
            Debug.Log("音调升高");
        }
        else
        {
            rod.pitch = 1.0f;
        }
    }
    
    void changingText()//this is changing the text shown on the screen, it is just for me to understand what step the user is going through. Because when I am inviting people to playtest my cube, I am always troubled about how I can be able to udnerstand what they are hearing and what is the reason they failed to catch a fish, so I made this text for me to udnerstand the reason why they cannot catch a fish.
    {
        if (fishInside&dipIn)
        {
            float alpha = inside.color.a;
            alpha += 0.1f;
            alpha = Mathf.Clamp(alpha,0.1f,1.0f);
            inside.color = new Vector4(inside.color.r, inside.color.g, inside.color.b, alpha);
        }
        else
        {
            float alpha = inside.color.a;
            alpha -= 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            inside.color = new Vector4(inside.color.r, inside.color.g, inside.color.b, alpha);
        }
        if (smoothMovement & dipIn)
        {
            float alpha = smooth.color.a;
            alpha += 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            smooth.color = new Vector4(smooth.color.r, smooth.color.g, smooth.color.b, alpha);
        }
        else
        {
            float alpha = smooth.color.a;
            alpha -= 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            smooth.color = new Vector4(smooth.color.r, smooth.color.g, smooth.color.b, alpha);
        }
        if (state == 2)
        {
            float alpha = stretchLine.color.a;
            alpha += 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            stretchLine.color = new Vector4(stretchLine.color.r, stretchLine.color.g, stretchLine.color.b, alpha);
        }
        else
        {
            float alpha = stretchLine.color.a;
            alpha -= 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            stretchLine.color = new Vector4(stretchLine.color.r, stretchLine.color.g, stretchLine.color.b, alpha);
        }
        if (Time.time - rodStartTime - fishEatingTime > 0 & Time.time - rodStartTime - fishEatingTime < 1.5f&state==2)
        {
            float alpha = pitchText.color.a;
            alpha += 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            pitchText.color = new Vector4(pitchText.color.r, pitchText.color.g, pitchText.color.b, alpha);
        }
        else
        {
            float alpha = pitchText.color.a;
            alpha -= 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            pitchText.color = new Vector4(pitchText.color.r, pitchText.color.g, pitchText.color.b, alpha);
        }
        if (Time.time - lostFishTime < 2.0f)
        {
            float alpha = noFishText.color.a;
            alpha += 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            noFishText .color = new Vector4(noFishText .color.r, noFishText .color.g, noFishText.color.b, alpha);
        }
        else
        {
            float alpha = noFishText.color.a;
            alpha -= 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            noFishText.color = new Vector4(noFishText.color.r, noFishText.color.g, noFishText.color.b, alpha);
        }
        if (Time.time - gotFishTime < 4.0f)
        {
            float alpha = gotText.color.a;
            alpha += 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            gotText.color = new Vector4(gotText.color.r, gotText.color.g, gotText.color.b, alpha);
        }
        else
        {
            float alpha = gotText.color.a;
            alpha -= 0.1f;
            alpha = Mathf.Clamp(alpha, 0.1f, 1.0f);
            gotText.color = new Vector4(gotText.color.r, gotText.color.g, gotText.color.b, alpha);
        }
    }
    public void ChangeSmoothValue()     //changing the value based on the slider, this is for adjusting the easiness of smooth movement of fishing hook.
    {
        smoothDistance = smoothDistanceSlider.value;
    }
}
