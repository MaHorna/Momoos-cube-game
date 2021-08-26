using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuHandler : MonoBehaviour
{
    float mainmanugroupPosx;
    float mainmenugroupPosY;
    void Start()
    {
        mainmanugroupPosx = MainmenuGroup.transform.position.x;
    }
    public GameObject MainmenuGroup;
    public GameObject panel;

    public GameObject[] cubes;
    private float time1;
    bool endlesstransition = false;
    bool creditstransition = false;
    bool creditstransitionBack = false;
    private GameObject[] obstacles;
    private int i;
    private float destroytime;
    float startTime;
    bool firsttime = true;
    bool firsttimecre = true;
    bool firsttimecreBack = true;
    int lasttodestroy;
    int todestroy;
    void FixedUpdate()
    {
        if (endlesstransition == false)
        {
            time1 += Time.deltaTime;

            if (time1 >= 0.3f)
            {
                time1 = time1 - 0.3f;
                System.Random r1 = new System.Random();
                if (r1.Next(0, 101) <= 10)
                {
                    Instantiate(cubes[3], new Vector3(r1.Next(-7, 8), r1.Next(2, 4), r1.Next(-4970, -4930)), Quaternion.identity);
                }
                else if (r1.Next(0, 101) <= 10)
                {
                    Instantiate(cubes[2], new Vector3(r1.Next(-7, 8), r1.Next(2, 4), r1.Next(-4970, -4930)), Quaternion.identity);
                }
                else
                {
                    Instantiate(cubes[0], new Vector3(r1.Next(-7, 8), r1.Next(2, 4), r1.Next(-4970, -4930)), Quaternion.identity);
                }
            }
        }
        else
        {
            if (firsttime)
            {
                firsttime = false;
                startTime = Time.time;
            }
            float t = (Time.time - startTime) / 2.0f;
            panel.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(0.7f, 0, t) * 255));
            MainmenuGroup.transform.position = new Vector3(Mathf.SmoothStep(mainmanugroupPosx, mainmanugroupPosx - Screen.width, t), MainmenuGroup.transform.position.y, MainmenuGroup.transform.position.z);
            lasttodestroy = todestroy;
            todestroy = (int)Mathf.Lerp(0, obstacles.Length, t);
            if (todestroy >= obstacles.Length)
            {
                loadEndless();
            }
            for (int i = lasttodestroy; i < todestroy; i++)
            {
                Destroy(obstacles[i]);
            }

        }
        if (creditstransition)
        {
            if (firsttimecre)
            {
                firsttimecre = false;
                startTime = Time.time;
            }
            float t = (Time.time - startTime) / 2.0f;
            MainmenuGroup.transform.position = new Vector3(MainmenuGroup.transform.position.x, Mathf.SmoothStep(mainmenugroupPosY, mainmenugroupPosY + Screen.height, t), MainmenuGroup.transform.position.z);
            if (t>=1)
            {
                creditstransition = false;
            }
        }
        if (creditstransitionBack)
        {
            if (firsttimecreBack)
            {
                firsttimecreBack = false;
                startTime = Time.time;
            }
            float t = (Time.time - startTime) / 2.0f;
            MainmenuGroup.transform.position = new Vector3(MainmenuGroup.transform.position.x, Mathf.SmoothStep(mainmenugroupPosY + Screen.height, mainmenugroupPosY, t), MainmenuGroup.transform.position.z);
            if (t>=1)
            {
                creditstransitionBack = false;
            }
        }
    }


    public void loadEndlessMode()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Prekazka");
        destroytime = 2 / obstacles.Length;
        endlesstransition = true;
    }
    private void loadEndless()
    {
        SceneManager.LoadScene("Endless");
    }
    public void exit()
    {
        Application.Quit();
    }
    public void credits()
    {
        firsttimecre =true;
        creditstransition = true;
        creditstransitionBack = false;
    }
    public void creditsback()
    {
        firsttimecreBack = true;
        creditstransitionBack = true;
        creditstransition = false;
    }
}
