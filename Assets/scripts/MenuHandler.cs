using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuHandler : MonoBehaviour
{
    void Start()
    {
    }
    void FixedUpdate()
    {
        
    }
    public void loadEndlessMode()
    {
        SceneManager.LoadScene("Endless");
    }
}
