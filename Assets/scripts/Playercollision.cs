using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
public class Playercollision : MonoBehaviour
{
    public Material red;
    public Material blue;
    public Move movement;
    public Text scoreCounter;
    public GameObject Cube;
    public GameObject longCube;
    public GameObject cubeMoving;
    public GameObject cubeExploding;
    public GameObject cubePowerup;
    public GameObject panel;
    int i; string skore;
    int lastGeneratedCubeSector = 0;
    Image img;
    Button btnRestart;
    Text btnRestartText;
    float duration = 1.5f;
    float startTime;
    bool firsttime = true;
    bool firsttimeback = true;
    bool backToMenu = false;
    int lasttodestroy, todestroy;
    GameObject[] obstacles;
    GameObject[] allobjects;
    void Start()
    {
        btnRestart = panel.GetComponentInChildren<Button>();
        btnRestart.interactable = false;
        img = panel.GetComponent<Image>();
        btnRestartText = panel.GetComponentInChildren<Text>();
    }
    void FixedUpdate()
    {
        if (gameObject.transform.position.y <= -5)
        {
            movement.enabled = false;
        }
        if (shield)
        {
            if (shieldpickupscore + 10 <= i / 20)
            {
                shield = false;
                gameObject.GetComponent<MeshRenderer>().material = red;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backToMenu = true;
            obstacles = GameObject.FindGameObjectsWithTag("Prekazka");
            allobjects = GameObject.FindGameObjectsWithTag("Shield");
            allobjects = allobjects.Concat(obstacles).ToArray();
        }
        if (backToMenu)
        {
            if (firsttimeback)
            {
                firsttimeback = false;
                startTime = Time.time;
            }
            float t = (Time.time - startTime) / 2.0f;
            panel.GetComponent<Image>().color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(0, 0.765f, t) * 255));
            scoreCounter.GetComponent<Text>().color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(1,0, t) * 255));
            lasttodestroy = todestroy;
            todestroy = (int)Mathf.Lerp(0, allobjects.Length, t);
            if (todestroy >= allobjects.Length)
            {
                startTime = 0;
                firsttimeback = true;
                backToMenu = false;
                loadMainMenu();
            }
            for (int i = lasttodestroy; i < todestroy; i++)
            {
                Destroy(allobjects[i]);
            }

        }
        if (movement.enabled == false)
        {
            if (firsttime)
            {
                firsttime = false;
                startTime = Time.time;
                btnRestart.interactable = true;
            }
            float t = (Time.time - startTime) / duration;
            img.color = new Color32(255, 255, 255, (byte)(Mathf.SmoothStep(0, 0.7f, t) * 255));
            btnRestartText.color = new Color32(208, 12, 12, (byte)(Mathf.SmoothStep(0, 0.7f, t) * 255));
            if (Input.GetKeyDown("space"))
            {
                RestartScene();
            }
        }
        if (movement.enabled == true)
        {
            i++;
        }
        if ((i % 20) == 0)
        {
            skore = Convert.ToString(i / 20);
            scoreCounter.text = skore;
        }
        if ((i % 50) == 0)
        {
            if (gameObject.transform.position.z - lastGeneratedCubeSector < 200)
            {
                int CubeToSpawn = i / 100;

                int LastCubeChance = i % 100;
                System.Random r1 = new System.Random(); //which cube to spawn
                System.Threading.Thread.Sleep(10);
                System.Random r2 = new System.Random(); //left/right pos
                System.Threading.Thread.Sleep(10);
                System.Random r3 = new System.Random(); //up/down pos
                for (int k = 0; k < CubeToSpawn; k++)
                {

                    int cube = r1.Next(0, 2);
                    if (cube == 1)
                    {
                        if (r1.Next(0, 101) <= 1)
                        {
                            Instantiate(cubeExploding, new Vector3(r2.Next(-7, 8), 2, r3.Next(lastGeneratedCubeSector, lastGeneratedCubeSector + 51)), Quaternion.identity);
                        }
                        else if (r1.Next(0, 101) <= 10)
                        {
                            Instantiate(cubePowerup, new Vector3(r2.Next(-7, 8), 1, r3.Next(lastGeneratedCubeSector, lastGeneratedCubeSector + 51)), Quaternion.identity);
                        }
                        else if (r1.Next(0, 101) <= 5)
                        {
                            Instantiate(cubeMoving, new Vector3(r2.Next(-7, 8), 2, r3.Next(lastGeneratedCubeSector, lastGeneratedCubeSector + 51)), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(Cube, new Vector3(r2.Next(-7, 8), 2, r3.Next(lastGeneratedCubeSector, lastGeneratedCubeSector + 51)), Quaternion.identity);
                        }
                    }
                    else
                    {
                        Instantiate(longCube, new Vector3(r2.Next(-7, 8), 2, r3.Next(lastGeneratedCubeSector, lastGeneratedCubeSector + 51)), Quaternion.identity);
                    }

                }
                if (r1.Next(0, 101) <= LastCubeChance)
                {
                    Instantiate(Cube, new Vector3(r2.Next(-7, 8), 2, r3.Next(lastGeneratedCubeSector, lastGeneratedCubeSector + 51)), Quaternion.identity);
                }
                lastGeneratedCubeSector = (int)gameObject.transform.position.z + 200;
            }
        }

    }
    private bool shield = false;
    private int shieldpickupscore;
    void OnCollisionEnter(Collision collInfo)
    {
        if (collInfo.collider.tag == "Shield")
        {
            shield = true;
            gameObject.GetComponent<MeshRenderer>().material = blue;
            shieldpickupscore = i / 20;
        }
        if (collInfo.collider.tag == "Prekazka")
        {
            if (!shield)
            {
                movement.enabled = false;
            }

        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("Endless");
    }
    private void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
