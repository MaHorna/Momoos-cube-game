using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpEndless : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 1;
    }
    void FixedUpdate()
    {
        if (DataHolder.Instance.optionsLoaded)
        {
            QualitySettings.vSyncCount = DataHolder.Instance.fps;
        }
    }
}
