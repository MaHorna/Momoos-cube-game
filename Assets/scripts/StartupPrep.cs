﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupPrep : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
