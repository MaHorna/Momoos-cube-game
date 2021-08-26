using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.Find("Pl");
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            if (player.transform.position.z > gameObject.transform.position.z + 10)
            {
                Destroy(gameObject);
            }
        }

    }
}
