using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSVsNARS_PongPaddle : NARSPong_PongPaddle
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == PongBall.TAG)
        {
            NARSHost NARS_host = GetSensorimotor().GetNARSHost();
            if (NARS_host == NARSVsNARS_GameManager.GetInstance().NARS1)
            {
                NARSVsNARS_GameManager.GetInstance().AddNARS1Block();
            }else if (NARS_host == NARSVsNARS_GameManager.GetInstance().NARS2)
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
