using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class Playercollision : MonoBehaviour
{
    public Material red, blue;
    public Move movement;
    public Text scoreCounter;
    public Text highscoreShow;
    public GameObject[] cubeObs; //0 cube, 1 longcube, 2 moving cube, 3 exploding cube, 4 shield pickup, 5 score pickup
    public GameObject panel;
    Image img;
    Button btnRestart;
    Text btnRestartText;
    bool firsttime = true;
    bool firsttimeback = true;
    bool backToMenu = false;
    bool shield = false;
    bool flying = false;
    int lasttodestroy, todestroy, tospawn, cubesHitWithShield;
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
    }
    void FixedUpdate()
    {
        if (gameObject.transform.position.y <= -5) //run off of ground - end game
        {
            movement.enabled = false;
        }

        if (shield) //shield off
        {
            if (shieldpickuptime + 5.0f <= Time.time)
            {
                shield = false;
                score += cubesHitWithShield; //todo send to function handling showing score updates
                gameObject.GetComponent<MeshRenderer>().material = red;
                //todo add to dataholder achievment counter how many cubes hit
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
                if (DataHolder.Instance.Highscore <= (score + ((gameObject.transform.position.z + 4980) / 20)))
                {
                    DataHolder.Instance.Highscore = (int)(score + ((gameObject.transform.position.z + 4980) / 20));
                    if (PlayerPrefs.HasKey("HighScore"))
                    {
                        PlayerPrefs.SetInt("HighScore", (int)(score + ((gameObject.transform.position.z + 4980) / 20)));
                        PlayerPrefs.Save();
                    }
                    Debug.Log(PlayerPrefs.GetInt("HighScore"));
                }
                firsttimeback = false;
                startTime = Time.time;
            }
            float t1 = (Time.time - startTime) / ((1-(panel.GetComponent<Image>().color.a / 0.765f)) * 2.0f); 
            float t2 = (Time.time - startTime) / 2.0f;//calculate time remaining
            Debug.Log(panel.GetComponent<Image>().color.a);
            panel.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(panel.GetComponent<Image>().color.a, 0.765f, t2) * 255)); //change color of panel
            scoreCounter.GetComponent<Text>().color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(1, 0, t2) * 255)); //change color of scorecounter
            highscoreShow.GetComponent<Text>().color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(1, 0, t2) * 255)); //change color of highscore show
            btnRestartText.color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(btnRestartText.color.a, 0, t2) * 255)); //restart text color change
            lasttodestroy = todestroy;//store last destroyed index
            todestroy = (int)Mathf.Lerp(0, allobjects.Length, t2); //new destroy index
            btnRestart.interactable = false;
            if (todestroy >= allobjects.Length && t2 >= 1) //end conditions - all of them destroyed and time passed
            {
                loadMainMenu();
            }
            for (int i = lasttodestroy; i < todestroy; i++)
            {
                Destroy(allobjects[i]);
            }
        }

        if (movement.enabled == false) //end game
        {
            if (!backToMenu)
            {
                if (firsttime) //seting time for transitions, and saving score
                {
                    if (PlayerPrefs.HasKey("HighScore"))
                    {
                        PlayerPrefs.SetInt("HighScore", (int)(score + ((gameObject.transform.position.z + 4980) / 20)));
                        PlayerPrefs.Save();
                    }
                    firsttime = false;
                    startTime = Time.time;
                    btnRestart.interactable = true;
                }
                float t = (Time.time - startTime) / 2.0f; //calculating time
                img.color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(0, 0.765f, t) * 255)); //panel change color
                btnRestartText.color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(0, 1, t) * 255)); //restart text color change
                if (Input.GetKeyDown("space")) //if space pressed restarts scene ( same as clicking on button on screen)
                {
                    RestartScene();
                }
            }
        }

        if (movement.enabled == true) //score calculating 
        {
            if (gameObject.transform.position.y >= 1.5f && flying == false) //flying 
            {
                flying = true;
                flytime = 0;
            }
            if (flying) //flying
            {
                flytime += Time.deltaTime;
                if (gameObject.transform.position.y <= 1.5f)
                {
                    flying = false;
                    score += flytime * 6; //adding score for flying 
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
            shield = true;
            gameObject.GetComponent<MeshRenderer>().material = blue;
            shieldpickuptime = Time.time;
        }
        if (collInfo.collider.tag == "Score") //when coliding with score 
        {
            score += 20;
        }
        if (collInfo.collider.tag == "Obstacle") //when coliding with obstacle
        {
            if (!shield)
            {
                movement.enabled = false;
            }
            else
            {
                cubesHitWithShield++;
            }
        }
    }
    public void RestartScene()
    {
        if (DataHolder.Instance.Highscore <= (score + ((gameObject.transform.position.z + 4980) / 20)))
        {
            DataHolder.Instance.Highscore = (int)(score + ((gameObject.transform.position.z + 4980) / 20));
            if (PlayerPrefs.HasKey("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", (int)(score + ((gameObject.transform.position.z + 4980) / 20)));
                PlayerPrefs.Save();
            }
            Debug.Log(PlayerPrefs.GetInt("HighScore"));
        }
        SceneManager.LoadScene("Endless");
    }
    private void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
}
