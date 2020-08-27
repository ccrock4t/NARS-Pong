using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSPong_Goal : MonoBehaviour
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
        if(other.tag == PongBall.TAG)
        {
            NARSPong_GameManager.GetInstance().AddMiss();
            NARSSensorimotor.GetInstance().Punish();
            other.GetComponent<PongBall>().ResetPosition();
        }
    }
}
