using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance;
    public int Highscore;
    public bool HSloaded = false;
    public Image[] AchDoneImg;

    public int NumberOfRuns;
    public int scoreCollected;
    public int shieldCollected;
    public int[] AchievementControlValues;
    public int[] AchievementCountables;
    string tmpAchDone;
    public bool[] AchievementsDone; //0 base cube ?, 1 score cube pickup,2 shield cube pickup,3distance traveled,4 number of runs,5 shield small cubes,6 flytime

    //menu options
    public int ScEf;
    public string Name;
    public int CubeIndex;
    public int Volume;
    public int fps;
    public float TransitionLength;
    public bool optionsLoaded = false;
    //menu options
    void Start()
    {
        AchievementsDone = new bool[7];
        AchievementCountables = new int[7];
        if (PlayerPrefs.HasKey("HighScore"))
        {
            Highscore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", 0);
            Highscore = 0;
        }
        HSloaded = true;
        if (PlayerPrefs.HasKey("CubeIndex"))
        {
            CubeIndex = PlayerPrefs.GetInt("CubeIndex");
        }
        else
        {
            PlayerPrefs.SetInt("CubeIndex", 0);
            CubeIndex = 0;
        }

        if (PlayerPrefs.HasKey("Volume"))
        {
            Volume = PlayerPrefs.GetInt("Volume");
        }
        else
        {
            PlayerPrefs.SetInt("Volume", 50);
            Volume = 50;
        }

        if (PlayerPrefs.HasKey("ScEf"))
        {
            ScEf = PlayerPrefs.GetInt("ScEf");
        }
        else
        {
            PlayerPrefs.SetInt("ScEf", 1);
            ScEf = 1;
        }

        if (PlayerPrefs.HasKey("Fps"))
        {
            fps = PlayerPrefs.GetInt("Fps");
        }
        else
        {
            PlayerPrefs.SetInt("Fps", 1);
            fps = 1;
        }

        if (PlayerPrefs.HasKey("TransitionLength"))
        {
            TransitionLength = PlayerPrefs.GetFloat("TransitionLength");
        }
        else
        {
            PlayerPrefs.SetFloat("TransitionLength", 2.0f);
            TransitionLength = 2.0f;
        }

        if (PlayerPrefs.HasKey("Name"))
        {
            Name = PlayerPrefs.GetString("Name");
        }
        else
        {
            PlayerPrefs.SetString("Name", "Guest");
            Name = "Guest";
        }
        optionsLoaded = true;
        if (PlayerPrefs.HasKey("NumberOfRuns"))
        {
            NumberOfRuns = PlayerPrefs.GetInt("NumberOfRuns");
        }
        else
        {
            PlayerPrefs.SetInt("NumberOfRuns", 0);
            NumberOfRuns = 0;
        }

        if (PlayerPrefs.HasKey("scoreCollected"))
        {
            scoreCollected = PlayerPrefs.GetInt("scoreCollected");
        }
        else
        {
            PlayerPrefs.SetInt("scoreCollected", 0);
            scoreCollected = 0;
        }

        if (PlayerPrefs.HasKey("shieldCollected"))
        {
            shieldCollected = PlayerPrefs.GetInt("shieldCollected");
        }
        else
        {
            PlayerPrefs.SetInt("shieldCollected", 0);
            shieldCollected = 0;
        }

        if (PlayerPrefs.HasKey("AchievementsDone"))
        {
            tmpAchDone = PlayerPrefs.GetString("AchievementsDone");
        }
        else
        {
            PlayerPrefs.SetString("AchievementsDone", "ynnnnnn");
            tmpAchDone = "ynnnnnn";
        }

        for (int i = 0; i < tmpAchDone.Length; i++)
        {
            if (tmpAchDone[i] == 'y')
            {
                AchievementsDone[i] = true;
            }
            else
            {
                AchievementsDone[i] = false;
            }
        }
        AchievementCountables[4] = NumberOfRuns;
        Options();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            //First run, set the instance
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            //Instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void UpdateHS(int newScore)
    {
        if (newScore > Highscore)
        {
            Highscore = newScore;
            AchievementCountables[3] = Highscore;
            PlayerPrefs.SetInt("HighScore", newScore);
            PlayerPrefs.Save();
        }
    }
    void AchCheck()
    {
        for (int i = 0; i < AchievementControlValues.Length; i++)
        {
            if (AchievementCountables[i] >= AchievementControlValues[i])
            {
                AchievementsDone[i] = true;
            }
        }
    }
    public void Options()
    {
        for (int i = 0; i < AchievementsDone.Length; i++)
        {
            GameObject tmp = GameObject.Find(i.ToString());
            Image tmpimg = tmp.GetComponent<Image>();
            if (AchievementsDone[i] == false)
            {
                tmpimg.color = new Color32(208, 12, 12, 80);
            }
            else
            {
                tmpimg.color = new Color32(0, 200, 0, 80);
            }
        }
        GameObject tmp1 = GameObject.Find("SliderFPS");
        Slider tmpslider = tmp1.GetComponent<Slider>();
        tmpslider.value = fps;

        tmp1 = GameObject.Find("SliderTrLe");
        tmpslider = tmp1.GetComponent<Slider>();
        tmpslider.value = TransitionLength;

        tmp1 = GameObject.Find("SliderVolume");
        tmpslider = tmp1.GetComponent<Slider>();
        tmpslider.value = Volume;

        tmp1 = GameObject.Find("Toggle" + CubeIndex);
        tmp1.GetComponent<Toggle>().isOn = true;

        tmp1 = GameObject.Find("NameInput");
        tmp1.GetComponent<InputField>().text = Name;

        tmp1 = GameObject.Find("ScEfToggle");
        if (ScEf == 1)
        {
            tmp1.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            tmp1.GetComponent<Toggle>().isOn = false;
        }
    }
    void SaveStringAchDone()
    {
        string output = "";
        for (int i = 0; i < AchievementsDone.Length; i++)
        {
            if (AchievementsDone[i] == true)
            {
                output += 'y';
            }
            else
            {

                output += 'n';
            }
        }
        PlayerPrefs.SetString("AchievementsDone", output);
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("NumberOfRuns", NumberOfRuns);
        PlayerPrefs.SetInt("scoreCollected", scoreCollected);
        PlayerPrefs.SetInt("shieldCollected", shieldCollected);
        AchCheck();
        SaveStringAchDone();
        PlayerPrefs.Save();
    }
    public void ChangeName(string name)
    {
        Name = name;
    }
    public void ChangeVolume(System.Single newvolume)
    {
        Volume = (int)newvolume;
    }
    public void ChangeScEf(bool newscef)
    {
        if (newscef)
        {
            ScEf = 1;
        }
        else
        {
            ScEf = 0;
        }
    }
    public void ChangeCubeIndex(int newindex)
    {

        if (AchievementsDone[newindex] == false)
        {
            GameObject tmp1 = GameObject.Find("Toggle0");
            tmp1.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            CubeIndex = newindex;
        }
    }
    public void ChangeFps(System.Single newFps)
    {
        fps = (int)newFps;
    }
    public void ChangeTransLength(System.Single newLength)
    {
        TransitionLength = newLength;
    }
    public void SaveOptions()
    {
        PlayerPrefs.SetString("Name", Name);
        PlayerPrefs.SetInt("CubeIndex", CubeIndex);
        PlayerPrefs.SetInt("Volume", Volume);
        PlayerPrefs.SetInt("ScEf", ScEf);
        PlayerPrefs.SetInt("Fps", fps);
        PlayerPrefs.SetFloat("TransitionLength", TransitionLength);
        PlayerPrefs.Save();
    }

    public void eraseData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
