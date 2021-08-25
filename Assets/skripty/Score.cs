using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    public Text nieco;
    int i; string skore;
    void FixedUpdate()
    {
        i++;
        skore = Convert.ToString(i);
        nieco.text = skore;
        
    }
}