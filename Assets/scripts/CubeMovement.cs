using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    // Start is called before the first frame update
    float startPos = 0;
    float offset = 0;
    bool incresing = true;
    void Start()
    {
        startPos = gameObject.transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (startPos + offset >= 7)
        {
            incresing = false;
        }
        else if (startPos + offset <= -7)
        {
            incresing = true;
        }
        if (incresing)
        {
            offset += 0.1f;
            gameObject.transform.position = transform.position + new Vector3(0.1f, 0,0); 
        }
        if (incresing == false)
        {
            offset -= 0.1f;
            gameObject.transform.position = transform.position + new Vector3(-0.1f,0,0); 
        }
    }
}
