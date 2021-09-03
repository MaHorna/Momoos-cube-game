using System;
using UnityEngine;
using UnityEngine.UI;
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    public AudioClip[] audioClips;
    public float[] audioLength;
    float starttime;
    int nowPlaying = 0;
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
    // Start is called before the first frame update
    void Start()
    {
        System.Random r1 = new System.Random();
        starttime = Time.time;
        nowPlaying = r1.Next(0, audioClips.Length);
        gameObject.GetComponent<AudioSource>().clip = audioClips[nowPlaying];
        gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        float tmp = DataHolder.Instance.Volume;
        gameObject.GetComponent<AudioSource>().volume = tmp/100;        
        if (starttime + audioLength[nowPlaying] <= Time.time)
        {
            System.Random r1 = new System.Random();
            nowPlaying = r1.Next(0, audioClips.Length);
            gameObject.GetComponent<AudioSource>().clip = audioClips[nowPlaying];
            gameObject.GetComponent<AudioSource>().Play();
            starttime = Time.time;
        }
    }
}
