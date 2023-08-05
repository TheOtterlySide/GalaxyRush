using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AK.Wwise.Bank MyBank = null;
    public AK.Wwise.Event AK_Start = null;
    public AK.Wwise.Event AK_Resume = null;
    public AK.Wwise.Event AK_Pause = null;
    public AK.Wwise.Event AK_Stop = null;

    public void Awake()
    {
        MyBank.Load();
    }

    public void Start()
    {
        AK_Start.Post(gameObject);
    }

    public void Pause()
    {
        AK_Pause.Post(gameObject);
    }
    
    public void Stop()
    {
        AK_Stop.Post(gameObject);
    }
    
    public void Resume()
    {
        AK_Resume.Post(gameObject);
    }
}