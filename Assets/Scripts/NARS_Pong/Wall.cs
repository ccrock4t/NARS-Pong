using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public static string LeftWallTAG = "LeftWall", TopWallTAG = "TopWall", RightWallTAG = "RightWall";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == PongBall.TAG)
        {
            if (gameObject.tag == Wall.LeftWallTAG)
            {
                NARSHost.GetInstance().AddInput("<{ball_hit_left_wall} --> [on]>. :|:");
            }
            else if (gameObject.tag == Wall.TopWallTAG)
            {
                NARSHost.GetInstance().AddInput("<{ball_hit_top_wall} --> [on]>. :|:");
            }
            else if (gameObject.tag == Wall.RightWallTAG)
            {
                NARSHost.GetInstance().AddInput("<{ball_hit_right_wall} --> [on]>. :|:");
            }
        }

        if (collision.collider.tag == PongPaddle.TAG)
        {
            if (gameObject.tag == Wall.LeftWallTAG)
            {
                NARSHost.GetInstance().AddInput("<{paddle_hit_left_wall} --> [on]>. :|:");
            }
            else if (gameObject.tag == Wall.TopWallTAG)
            {
                NARSHost.GetInstance().AddInput("<{paddle_hit_top_wall} --> [on]>. :|:");
            }
            else if (gameObject.tag == Wall.RightWallTAG)
            {
                NARSHost.GetInstance().AddInput("<{paddle_hit_right_wall} --> [on]>. :|:");
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == PongBall.TAG)
        {
           // NARSHost.GetInstance().AddInput("<{ball_hit_nothing} --> [on]>. :|:");
        }

        if (collision.collider.tag == PongPaddle.TAG)
        {
            NARSHost.GetInstance().AddInput("<{paddle_hit_nothing} --> [on]>. :|:");
        }
    }*/
   
}
