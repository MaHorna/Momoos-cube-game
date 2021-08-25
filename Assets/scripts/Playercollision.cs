using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Playercollision : MonoBehaviour
{
    public Move movement;
    public Text scoreCounter;
    public GameObject Cube;
    public GameObject longCube;
    public GameObject cubeMoving;
    public GameObject cubeExploding;
    public GameObject panel;
    int i; string skore;
    int lastGeneratedCubeSector = 0;
    Image img;
    Button btnRestart;
    Text btnRestartText;
    float duration = 1.5f;
    float startTime;
    bool firsttime = true;
    void Start()
    {
        btnRestart = panel.GetComponentInChildren<Button>();
        btnRestart.interactable = false;
        img = panel.GetComponent<Image>();
        btnRestartText = panel.GetComponentInChildren<Text>();
    }
    void FixedUpdate()
    {
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
    void OnCollisionEnter(Collision collInfo)
    {
        if (collInfo.collider.tag == "Prekazka")
        {
            movement.enabled = false;
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("Endless");
    }
}
