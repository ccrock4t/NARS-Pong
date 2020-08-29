using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSVsONA_Goal : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //scored a goal
        if (other.tag == PongBall.TAG)
        {
            other.GetComponent<PongBall>().ResetPosition();
        }
    }
}
