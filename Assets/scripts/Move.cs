   using UnityEngine;

public class Move : MonoBehaviour {
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sideForce = 500f;
	void FixedUpdate ()
    {
        rb.AddForce(0, 0, forwardForce * Mathf.Sqrt(Time.deltaTime));
        if (Input.GetKey("d")||Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("a")||Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(-sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
    }
}
 