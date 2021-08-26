using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupMenu : MonoBehaviour
{
    public GameObject[] cubes;
    void Start()
    {
        QualitySettings.vSyncCount = 1;
    }
    private float time;
    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time >= 0.3f)
        {
            time = time - 0.3f;
            System.Random r1 = new System.Random();
            if (r1.Next(0, 101) <= 10)
            {
                Instantiate(cubes[3], new Vector3(r1.Next(-7, 8), r1.Next(2, 4), r1.Next(-4990, -4970)), Quaternion.identity);
            }
            else if (r1.Next(0, 101) <= 10)
            {
                Instantiate(cubes[2], new Vector3(r1.Next(-7, 8), r1.Next(2, 4), r1.Next(-4990, -4970)), Quaternion.identity);
            }
            else
            {
                Instantiate(cubes[0], new Vector3(r1.Next(-7, 8), r1.Next(2, 4), r1.Next(-4990, -4970)), Quaternion.identity);
            }
        }
    }
}