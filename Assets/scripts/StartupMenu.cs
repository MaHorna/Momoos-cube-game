using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupMenu : MonoBehaviour
{
    
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }
    
}