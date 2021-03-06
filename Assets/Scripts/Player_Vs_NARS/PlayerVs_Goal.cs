﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVs_Goal : MonoBehaviour
{
    public bool isPlayer = false, isNARS = false;
    NARSHost host = null;

    // Start is called before the first frame update
    void Start()
    {
        host = GameObject.Find("NARSHost").GetComponent<NARSHost>();   
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
            if (isNARS) //score on NARS
            {
                PlayerVs_GameManager.GetInstance().AddPlayerPoint(); //player gets a point
            }else if (isPlayer) //score on Player
            {
                PlayerVs_GameManager.GetInstance().AddNARSPoint(host.type.ToString()); //NARS gets a point
            }
            other.GetComponent<PongBall>().ResetPosition();
        }
    }
}
