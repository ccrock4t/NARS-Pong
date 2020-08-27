using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour
{
    public static string TAG = "Ball";
    Rigidbody rb;
    Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        origin = gameObject.transform.position;
        ResetPosition();
    }

    // Update is called once per frame


    void NudgeX()
    {
        int randInt = Random.Range(1, 3);
        int sign = randInt == 1 ? -1 : 1;
        rb.AddForce(sign*new Vector3(Random.Range(0.4f,0.6f), 0f, 0f), ForceMode.Impulse);
    }

    void NudgeZ()
    {
        int randInt = Random.Range(1, 3);
        int sign = randInt == 1 ? -1 : 1;
        rb.AddForce(2f*sign* new Vector3(0f, 0f, Random.Range(0.4f, 0.6f)), ForceMode.Impulse);
    }

    public void ResetPosition()
    {
        gameObject.transform.position = origin;
        rb.velocity = new Vector3(0, 0, 0);
        Invoke("NudgeX", 1f);
        Invoke("NudgeZ", 1f);
    }
}
