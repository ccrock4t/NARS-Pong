using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSVsNARS_PongPaddle : NARSPong_PongPaddle
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == PongBall.TAG)
        {
            if (GetSensorimotor().GetNARSHost() == NARSVsNARS_GameManager.GetInstance().NARS1)
            {
                NARSVsNARS_GameManager.GetInstance().AddNARS1Block();
            }else if (GetSensorimotor().GetNARSHost() == NARSVsNARS_GameManager.GetInstance().NARS2)
            {
                NARSVsNARS_GameManager.GetInstance().AddNARS2Block();
            }

            if (GetSensorimotor() != null)
            {
                GetSensorimotor().Praise();
            }

        }
    }
}
