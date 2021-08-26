using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DataHolder : MonoBehaviour
{
    public static DataHolder Instance;
    public int Highscore;
    public string Name;
    public int CubeIndex;
    public int[] AchievementCountables;
    public bool[] AchievementsDone;
    void Start() {
         
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
