using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuHandler : MonoBehaviour
{
    public Text highscoreshow;
    public GameObject MainmenuGroup;
    public GameObject panel;
    public GameObject[] cubes;
    GameObject[] obstacles;
    float mainmenugroupPosx, mainmenugroupPosY, time1, startTime = 0;
    //bool endlesstransition,creditstransition,creditstransitionBack = false;
    //bool firsttime,firsttimecre,firsttimecreBack = true;
    int lasttodestroy, todestroy;
    int leftToRightIndex;
    int transitionIndex; //transition index , 0 none, 1 up - leaderboard, 2 right -  endless, 3 down - credits, 4 left - options
    void Start()
    {
        leftToRightIndex = 1;
    }
    void FixedUpdate()
    {
        if (DataHolder.Instance.HSloaded == true)
        {
            highscoreshow.text = "Highscore: " + DataHolder.Instance.Highscore.ToString();
        }
        if (transitionIndex == 0) //spawn new cubes only
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
        else if (transitionIndex == 1) //up
        {
            float t = (Time.time - startTime) / DataHolder.Instance.TransitionLength;
            MainmenuGroup.transform.position = new Vector3(MainmenuGroup.transform.position.x, Mathf.SmoothStep(mainmenugroupPosY, mainmenugroupPosY - Screen.height, t), MainmenuGroup.transform.position.z);
            if (t >= 1)
            {
                transitionIndex = 0;
            }
        }
        else if (transitionIndex == 2) //right
        {
            float t = (Time.time - startTime) / DataHolder.Instance.TransitionLength;
            MainmenuGroup.transform.position = new Vector3(Mathf.SmoothStep(mainmenugroupPosx, mainmenugroupPosx - Screen.width, t), MainmenuGroup.transform.position.y, MainmenuGroup.transform.position.z);
            if (t >= 1)
            {
                transitionIndex = 0;
            }
        }
        else if (transitionIndex == 3)//down
        {
            float t = (Time.time - startTime) / DataHolder.Instance.TransitionLength;
            MainmenuGroup.transform.position = new Vector3(MainmenuGroup.transform.position.x, Mathf.SmoothStep(mainmenugroupPosY, mainmenugroupPosY + Screen.height, t), MainmenuGroup.transform.position.z);
            if (t >= 1)
            {
                transitionIndex = 0;
            }
        }
        else if (transitionIndex == 4)//left
        {
            DataHolder.Instance.Options();
            float t = (Time.time - startTime) / DataHolder.Instance.TransitionLength;
            MainmenuGroup.transform.position = new Vector3(Mathf.SmoothStep(mainmenugroupPosx, mainmenugroupPosx + Screen.width, t), MainmenuGroup.transform.position.y, MainmenuGroup.transform.position.z);
            if (t >= 1)
            {
                transitionIndex = 0;
            }
        }
        else if (transitionIndex == 5) //right to endless
        {
            float t = (Time.time - startTime) / DataHolder.Instance.TransitionLength;
            panel.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(0.7f, 0, t) * 255));
            MainmenuGroup.transform.position = new Vector3(Mathf.SmoothStep(mainmenugroupPosx, mainmenugroupPosx - Screen.width, t), MainmenuGroup.transform.position.y, MainmenuGroup.transform.position.z);
            lasttodestroy = todestroy;
            todestroy = (int)Mathf.Lerp(0, obstacles.Length, t);
            if (todestroy >= obstacles.Length)
            {
                SceneManager.LoadScene("Endless");
            }
            for (int i = lasttodestroy; i < todestroy; i++)
            {
                Destroy(obstacles[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            left();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            up();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (leftToRightIndex == 1)
            {
                loadEndlessMode();
            }
            else
            {
                right();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            down();
        }
    }

    public void loadEndlessMode()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        if (transitionIndex == 0)
        {
            mainmenugroupPosx = MainmenuGroup.transform.position.x;
            transitionIndex = 5;
            startTime = Time.time;
        }
    }
    public void exit()
    {
        Application.Quit();
    }
    public void up()
    {
        if (transitionIndex == 0)
        {
            mainmenugroupPosY = MainmenuGroup.transform.position.y;
            transitionIndex = 1;
            startTime = Time.time;
        }
    }
    public void right()
    {
        if (transitionIndex == 0)
        {
            DataHolder.Instance.SaveOptions();
            mainmenugroupPosx = MainmenuGroup.transform.position.x;
            transitionIndex = 2;
            startTime = Time.time;
            leftToRightIndex++;
        }
    }
    public void down()
    {
        if (transitionIndex == 0)
        {
            mainmenugroupPosY = MainmenuGroup.transform.position.y;
            transitionIndex = 3;
            startTime = Time.time;
        }
    }
    public void left()
    {
        if (transitionIndex == 0)
        {
            leftToRightIndex--;
            mainmenugroupPosx = MainmenuGroup.transform.position.x;
            transitionIndex = 4;
            startTime = Time.time;
        }
    }
}
