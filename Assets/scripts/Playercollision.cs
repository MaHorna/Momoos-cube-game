﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class Playercollision : MonoBehaviour
{
    public Material red, blue;
    public Material[] AchievementMat; //0 red, 1 golden,2 light blue ,3 black and red,4 golden with crowns,5 blue with small cubes,6 green,7
    public Move movement;
    public Text scoreCounter;
    public Text highscoreShow;
    public Text shieldtimer;
    public GameObject[] cubeObs; //0 cube, 1 longcube, 2 moving cube, 3 exploding cube, 4 shield pickup, 5 score pickup, 6 waycleanerpickup
    public GameObject panel;
    public GameObject scoreEffect;
    Image img;
    Button btnRestart;
    Text btnRestartText;
    bool firsttime = true;
    bool firsttimeback = true;
    bool backToMenu = false;
    bool shield = false;
    bool flying = false;
    int lasttodestroy, todestroy, tospawn, cubesHitWithShield, smallcubeshitwithShield;
    float genNewCubesAt = -5100.0f;
    float startTime, flytime, score, shieldpickuptime;
    GameObject[] obstacles;
    GameObject[] allobjects;

    void Start()
    {
        btnRestart = panel.GetComponentInChildren<Button>();
        btnRestart.interactable = false;
        img = panel.GetComponent<Image>();
        btnRestartText = panel.GetComponentInChildren<Text>();
        highscoreShow.text = "Highscore: " + DataHolder.Instance.Highscore.ToString();
        gameObject.GetComponent<MeshRenderer>().material = AchievementMat[DataHolder.Instance.CubeIndex];
    }
    void FixedUpdate()
    {

        if (gameObject.transform.position.y <= -3) //run off of ground - end game
        {
            movement.enabled = false;
        }

        if (shield) //shield off
        {
            shieldtimer.text = (5.0f - (Time.time - shieldpickuptime)).ToString("0.00");
            if (shieldpickuptime + 5.0f <= Time.time)
            {
                shieldtimer.gameObject.SetActive(false);
                shield = false;
                gameObject.GetComponent<MeshRenderer>().material = AchievementMat[DataHolder.Instance.CubeIndex];
                if (DataHolder.Instance.CubeIndex == 2)
                {
                    score += (cubesHitWithShield * 3);
                    ScoreEffectUpdate(cubesHitWithShield * 3);
                }
                else
                {
                    score += cubesHitWithShield;
                    ScoreEffectUpdate(cubesHitWithShield);
                }
                if (smallcubeshitwithShield >= DataHolder.Instance.AchievementCountables[5])
                {
                    DataHolder.Instance.AchievementCountables[5] = smallcubeshitwithShield;
                }
                cubesHitWithShield = 0;
                smallcubeshitwithShield = 0;
                DataHolder.Instance.AchievementCountables[2]++;

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //start of back to menu procedures
        {
            backToMenu = true;
            obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            allobjects = GameObject.FindGameObjectsWithTag("Shield");
            allobjects = allobjects.Concat(obstacles).ToArray(); //get all objects to one array of objects to be destroy
            btnRestart.interactable = false;
        }

        if (backToMenu) //back to menu transition
        {
            if (firsttimeback) //set default time and set highscore if high enough
            {
                if (movement.enabled == true)
                {
                    DataHolder.Instance.AchievementCountables[4]++;
                    DataHolder.Instance.UpdateHS((int)(score + ((gameObject.transform.position.z + 4980) / 20)));
                }
                firsttimeback = false;
                startTime = Time.time;
            }
            float t2 = (Time.time - startTime) / DataHolder.Instance.TransitionLength;//calculate time remaining
            panel.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(panel.GetComponent<Image>().color.a, 0.765f, t2) * 255)); //change color of panel
            scoreCounter.GetComponent<Text>().color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(1, 0, t2) * 255)); //change color of scorecounter
            highscoreShow.GetComponent<Text>().color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(1, 0, t2) * 255)); //change color of highscore show
            btnRestartText.color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(btnRestartText.color.a, 0, t2) * 255)); //restart text color change
            lasttodestroy = todestroy;//store last destroyed index
            todestroy = (int)Mathf.Lerp(0, allobjects.Length, t2); //new destroy index
            btnRestart.interactable = false;
            if (todestroy >= allobjects.Length && t2 >= 1) //end conditions - all of them destroyed and time passed
            {
                DataHolder.Instance.SaveData();
                SceneManager.LoadScene("MainMenu");
            }
            for (int i = lasttodestroy; i < todestroy; i++)
            {
                Destroy(allobjects[i]);
            }
        }
        Debug.Log("selected transitio length: " + DataHolder.Instance.TransitionLength);
        if (movement.enabled == false) //end game
        {
            if (!backToMenu)
            {
                if (firsttime) //seting time for transitions, and saving score
                {
                    DataHolder.Instance.AchievementCountables[4]++;
                    DataHolder.Instance.UpdateHS((int)(score + ((gameObject.transform.position.z + 4980) / 20)));
                    firsttime = false;
                    startTime = Time.time;
                    btnRestart.interactable = true;
                }
                float t = (Time.time - startTime) / DataHolder.Instance.TransitionLength; //calculating time
                img.color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(0, 0.765f, t) * 255)); //panel change color
                btnRestartText.color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(0, 1, t) * 255)); //restart text color change
                if (Input.GetKeyDown("space")) //if space pressed restarts scene ( same as clicking on button on screen)
                {
                    DataHolder.Instance.SaveData();
                    SceneManager.LoadScene("Endless");
                }
            }
        }

        if (movement.enabled == true) //score calculating 
        {
            if (gameObject.transform.position.y >= 2.1f && flying == false) //flying 
            {
                flying = true;
                flytime = 0;
            }
            if (flying) //flying
            {
                flytime += Time.deltaTime;
                if (gameObject.transform.position.y <= 2.1f)
                {
                    flying = false;
                    if (DataHolder.Instance.CubeIndex == 6)
                    {
                        ScoreEffectUpdate((int)(flytime * 18));
                        score += flytime * 18; //adding score for flying 
                    }
                    else
                    {
                        ScoreEffectUpdate((int)(flytime * 6));
                        score += flytime * 6; //adding score for flying 
                    }

                    if (DataHolder.Instance.AchievementCountables[6] < flytime)
                    {
                        DataHolder.Instance.AchievementCountables[6] = (int)flytime;
                    }
                }
            }

            scoreCounter.text = ((int)(score + ((gameObject.transform.position.z + 4980) / 20))).ToString();
        }

        if (gameObject.transform.position.z >= genNewCubesAt) //if crossed the threshold for spawning new cubes - every 50 unity units
        {
            spawnCubesAt(genNewCubesAt, tospawn);
            tospawn++;
            genNewCubesAt += 50.0f;
        }
    }

    void OnCollisionEnter(Collision collInfo)
    {
        if (collInfo.collider.tag == "Shield") //when coliding with shield 
        {
            shieldtimer.gameObject.SetActive(true);
            shield = true;
            gameObject.GetComponent<MeshRenderer>().material = blue;
            DataHolder.Instance.AchievementCountables[2]++;
            shieldpickuptime = Time.time;
        }
        if (collInfo.collider.tag == "Score") //when coliding with score 
        {
            DataHolder.Instance.AchievementCountables[1]++;
            if (DataHolder.Instance.CubeIndex == 1)
            {
                ScoreEffectUpdate(40);
                score += 40;
            }
            else
            {
                ScoreEffectUpdate(20);
                score += 20;
            }

        }
        if (collInfo.collider.tag == "Obstacle") //when coliding with obstacle
        {
            if (!shield)
            {
                if (DataHolder.Instance.CubeIndex != 3)
                {
                    movement.enabled = false;
                }
                if (collInfo.gameObject.GetComponent<SelfDestruct>().cubeType == 1 && DataHolder.Instance.CubeIndex == 4)
                {
                    //number of runs achievemenmt  , not dies to moving cube -  type 1 cube
                }
                if (collInfo.gameObject.GetComponent<SelfDestruct>().cubeType == 2 && DataHolder.Instance.CubeIndex == 5)
                {
                    //small cube shit in one shield achievemenmt  , not dies to small cube -  type 2 cube
                }
            }
            else
            {
                cubesHitWithShield++;
                if (collInfo.gameObject.GetComponent<SelfDestruct>().cubeType == 2)
                {
                    smallcubeshitwithShield++;
                }
            }
        }
    }
    void spawnCubesAt(float startOfSector, int cubesToSpawn) //cube spawning handler
    {
        System.Random r1 = new System.Random();
        for (int i = 0; i < cubesToSpawn; i++)
        {
            int random = r1.Next(0, 101); //percentage
            int selectedcube = 0;
            if (random <= 54) //basic cube
            {
                selectedcube = 0;
            }
            else if (random <= 73) //long
            {
                selectedcube = 1;
            }
            else if (random <= 85) //moving
            {
                selectedcube = 2;
            }
            else if (random <= 90) // exploding
            {
                selectedcube = 3;
            }
            else if (random <= 94) //shield
            {
                selectedcube = 4;
            }
            else if (random <= 96) //score
            {
                selectedcube = 5;
            }
            else //way cleaner
            {
                selectedcube = 5; //todo change
            }
            if (selectedcube == 4 || selectedcube == 5)
            {
                Instantiate(cubeObs[selectedcube], new Vector3(r1.Next(-7, 8), 0.75f, r1.Next((int)startOfSector + 200, (int)startOfSector + 251)), Quaternion.identity);
            }
            else
            {
                Instantiate(cubeObs[selectedcube], new Vector3(r1.Next(-7, 8), 2, r1.Next((int)startOfSector + 200, (int)startOfSector + 251)), Quaternion.identity);
            }

        }
    }
    void ScoreEffectUpdate(int scoretoshow)
    {
        System.Random r1 = new System.Random();
        GameObject insObj = Instantiate(scoreEffect, new Vector3(1060, r1.Next(850, 1000), 0), Quaternion.identity);
        insObj.transform.SetParent(GameObject.Find("Canvas").transform);
        insObj.GetComponent<ScoreEffectBehaviour>().ScoreEffectString = scoretoshow.ToString();
    }
    void PlayerCubeInit()
    {

    }
}
