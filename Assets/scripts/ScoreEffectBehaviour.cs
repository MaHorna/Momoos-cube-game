using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffectBehaviour : MonoBehaviour
{
    public string ScoreEffectString = "";
    bool firstTransition, secondTransition, thirdTransition = false;
    bool firstTransitionFT = true;
    bool thirdTransitionFT = true;
    float startTime, waittime;
    Text mytextComp;
    void Start()
    {
        mytextComp = gameObject.GetComponent<Text>();
        mytextComp.text = "";
    }
    void FixedUpdate()
    {
        if (mytextComp.text == "")
        {
            mytextComp.text = ScoreEffectString;
            firstTransition = true;
        }
        if (firstTransition)
        {

            if (firstTransitionFT) //seting time for transitions, and saving score
            {
                firstTransitionFT = false;
                startTime = Time.time;
            }
            float t = (Time.time - startTime) / 1.0f; //calculating time
            mytextComp.color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(0, 1, t) * 255)); //panel change color
            gameObject.transform.position = new Vector3(Mathf.SmoothStep(1060, 960, t), gameObject.transform.position.y, gameObject.transform.position.z);
            if (t >= 1)
            {
                firstTransition = false;
                secondTransition = true;
            }
        }
        if (secondTransition)
        {
            waittime += Time.deltaTime;
            if (waittime >= 2)
            {
                secondTransition = false;
                thirdTransition = true;
            }
        }

        if (thirdTransition)
        {
            if (thirdTransitionFT) //seting time for transitions, and saving score
            {
                thirdTransitionFT = false;
                startTime = Time.time;
            }
            float t = (Time.time - startTime) / 1.0f; //calculating time
            mytextComp.color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(1, 0, t) * 255)); //panel change color
            gameObject.transform.position = new Vector3(Mathf.SmoothStep(960, 860, t), gameObject.transform.position.y, gameObject.transform.position.z);
            if (t >= 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
