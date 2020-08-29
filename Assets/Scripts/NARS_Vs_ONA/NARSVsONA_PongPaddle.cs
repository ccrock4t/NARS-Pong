using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSVsONA_PongPaddle : NARSPong_PongPaddle
{
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == PongBall.TAG)
        {
            switch (GetSensorimotor().GetNARSHost().type)
            {
                case NARSHost.NARSType.NARS:
                    NARSVsONA_GameManager.GetInstance().AddNARSBlock();
                    break;
                case NARSHost.NARSType.ONA:
                    NARSVsONA_GameManager.GetInstance().AddONABlock();
                    break;
                default:
                    break;
            }
           
            if (GetSensorimotor() != null)
            {
                GetSensorimotor().Praise();
            }

        }
    }
}
