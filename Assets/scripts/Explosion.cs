using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float power;
    public float radius;
    public float uppower;
    private GameObject player;
    private Rigidbody rigidbody;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }
    bool firsttime = true;
    void FixedUpdate()
    {
        if (player != null)
        {
            if (player.transform.position.z + 50 >= gameObject.transform.position.z)
            {
                if (firsttime)
                {
                    firsttime = false;
                    Collider[] objects = UnityEngine.Physics.OverlapSphere(gameObject.transform.position, radius);
                    foreach (Collider h in objects)
                    {
                        Rigidbody r = h.GetComponent<Rigidbody>();
                        if (r != null)
                        {
                            r.AddExplosionForce(power, gameObject.transform.position, radius);
                        }
                    }
                }
            }
        }
    }
}
