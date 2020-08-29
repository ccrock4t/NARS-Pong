using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSSensorimotor : MonoBehaviour
{
    private NARSHost _host;
    public PongBall ball;
    public NARSPong_PongPaddle paddle;
    public float width;

    MeshRenderer leftSensor, centerSensor, rightSensor;
    public MeshRenderer radialLeftSensor, radialCenterLeftSensor, radialCenterSensor, radialCenterRightSensor, radialRightSensor;
    public Color offColor = Color.black, oncolor = Color.yellow;

    float TIMER_DURATION = 0.2f;
    float timer = 0, inferenceTimer = 0, goalTimer = 0;
    string lastHorizontalRelativePositionInput = "", lastRadialInput = "", lastDistanceInput = "", lastHorizontalVelocityInput = "", lastVerticalVelocityInput = "", lastWallInput = "", lastDistanceVelocity = "", lastVerticalPositionInput = "";
    bool isHittingLeftWall, isHittingRightWall;

    // Start is called before the first frame update
    void Start()
    {
        width = paddle.GetComponent<MeshRenderer>().bounds.size.x;
        paddle.SetSensorimotor(this);

        leftSensor = paddle.transform.Find("HorizontalSensor/Left").GetComponent<MeshRenderer>();
        centerSensor = paddle.transform.Find("HorizontalSensor/Center").GetComponent<MeshRenderer>();
        rightSensor = paddle.transform.Find("HorizontalSensor/Right").GetComponent<MeshRenderer>();

        timer = TIMER_DURATION;
        inferenceTimer = TIMER_DURATION / 2f;
    }

    public void SetNARSHost(NARSHost host)
    {
        _host = host;
    }

    public NARSHost GetNARSHost()
    {
        return _host;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        inferenceTimer -= Time.deltaTime;
        goalTimer -= Time.deltaTime;

        if (timer <= 0f)
        {
            //Various test sensors
            //CalculateAndInputDistanceSensorData();
            //CalculateAndInputRadialSensorData();
            //CalculateAndInputHorizontalVelocitySensorData();
            //CalculateAndInputVerticalVelocitySensorData();
            //CalculateAndInputVerticalPositionSensorData();
            //DetectWalls();

            CalculateAndInputHorizontalRelativePositionSensorData();
            RemindGoal();
            timer = TIMER_DURATION;
        }

        if (inferenceTimer <= 0f)
        {
            if (GetNARSHost().type == NARSHost.NARSType.ONA)
            {
                GetNARSHost().AddInferenceCycles(2);
            }

            if (GetNARSHost().type == NARSHost.NARSType.NARS)
            {
                GetNARSHost().AddInferenceCycles(10);
            }
            inferenceTimer = TIMER_DURATION;
        }


    }

#region test sensors
    void CalculateAndInputRadialSensorData()
    {
        string input = "";
        //Need to move
        Vector3 targetDir = ball.transform.position - paddle.transform.position;
        float angle = Vector3.SignedAngle(targetDir, paddle.transform.forward, Vector3.up);
       
        angle *= -1;

        if (angle <= -45f)
        {
            input = "<{ball_angle_left} --> [on]>. :|:";
            SetRadialSensor("left");
        }
        else if (angle > -45f && angle < -15f)
        {
            input = "<{ball_angle_center_left} --> [on]>. :|:";
            SetRadialSensor("center_left");
        }
        if (angle >= -15f && angle <= 15f)
        {
            input = "<{ball_angle_center} --> [on]>. :|:";
            SetRadialSensor("center");
        }
        else if (angle > 15f && angle < 45f)
        {
            input = "<{ball_angle_center_right} --> [on]>. :|:";
            SetRadialSensor("center_right");
        }
        else if (angle >= 45f)
        {
            input = "<{ball_angle_right} --> [on]>. :|:";
            SetRadialSensor("right");
        }

        if (input != "")// && input != lastRadialInput)
        {
            GetNARSHost().AddInput(input);
            lastRadialInput = input;
        }
    }

    void SetRadialSensor(string position)
    {
        if (position == "left")
        {
            radialLeftSensor.material.color = oncolor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = offColor;
        }
        else if (position == "center")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = oncolor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = offColor;
        }
        else if (position == "right")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = oncolor;
        }
        else if (position == "center_left")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = oncolor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = offColor;
        }
        else if (position == "center_right")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = oncolor;
            radialRightSensor.material.color = offColor;
        }
        else if (position == "off")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = offColor;
        }
    }

    void CalculateAndInputHorizontalVelocitySensorData()
    {
        string input = "";
        //Need to move
        bool ballMovingRight = ball.transform.GetComponent<Rigidbody>().velocity.x > 0;
        bool ballMovingLeft = ball.transform.GetComponent<Rigidbody>().velocity.x < 0;

        if (ballMovingLeft)
        {
            input = "<{ball_moving_left} --> [on]>. :|:";
        }
        else if (ballMovingRight)
        {
            input = "<{ball_moving_right} --> [on]>. :|:";
        }

        if (input != "")// && input != lastHorizontalVelocityInput)
        {
            GetNARSHost().AddInput(input);
            lastHorizontalVelocityInput = input;
        }
    }

    void CalculateAndInputVerticalVelocitySensorData()
    {
        string input = "";
        //Need to move
        bool ballMovingUp = ball.transform.GetComponent<Rigidbody>().velocity.z > 0;
        bool ballMovingDown = ball.transform.GetComponent<Rigidbody>().velocity.z < 0;

        if (ballMovingUp)
        {
            input = "<{ball_moving_up} --> [on]>. :|:";
        }
        else if (ballMovingDown)
        {
            input = "<{ball_moving_down} --> [on]>. :|:";
        }

        if (input != "") //&& input != lastVerticalVelocityInput)
        {
            GetNARSHost().AddInput(input);
            lastVerticalVelocityInput = input;
        }
    }

    void CalculateAndInputDistanceSensorData()
    {
        string input = "";
        if (Vector3.Distance(ball.transform.position, paddle.transform.position) <= 4f)
        {
            input = "<{near} --> [on]>. :|:";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) < 8f && Vector3.Distance(ball.transform.position, paddle.transform.position) > 4f)
        {
            input = "<{near_middle} --> [on]>. :|:";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) <= 12f && Vector3.Distance(ball.transform.position, paddle.transform.position) >= 8f)
        {
            input = "<{middle} --> [on]>. :|:";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) < 16f && Vector3.Distance(ball.transform.position, paddle.transform.position) > 12f)
        {
            input = "<{middle_far} --> [on]>. :|:";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) >= 16f)
        {
            input = "<{far} --> [on]>. :|:";
        }

        if (input != "")// && input != lastDistanceInput)
        {
            GetNARSHost().AddInput(input);
            lastDistanceInput = input;
        }
    }

    void CalculateAndInputHorizontalPositionSensorData()
    {
        string input = "";
        //Need to move
        bool ballIsRight = ball.transform.position.x > 15f/3f;
        bool ballIsLeft = ball.transform.position.x < 15f/-3f;
        bool ballIsCenter = !ballIsLeft && !ballIsRight;

        if (ballIsLeft)
        {
            input = "<{ball_left} --> [on]>. :|:";
            // SetSensor("left");
        }
        else if (ballIsCenter)
        {
            input = "<{ball_center} --> [on]>. :|:";
            //SetSensor("right");
        }
        else if (ballIsRight)
        {
            input = "<{ball_right} --> [on]>. :|:";
            //SetSensor("center");
        }
        else
        {
            //SetSensor("off");
        }

        if (input != "")
        {
            GetNARSHost().AddInput(input);
        }
    }

    void CalculateAndInputVerticalPositionSensorData()
    {
        string input = "";
        //Need to move
        bool ballIsHigh = ball.transform.position.z > 2.5f;
        bool ballIsLow = ball.transform.position.z < -2.5f;
        bool ballIsEven = !ballIsHigh && !ballIsLow;

        if (ballIsHigh)
        {
            input = "<{ball_high} --> [on]>. :|:";
           // SetSensor("left");
        }
        else if (ballIsLow)
        {
            input = "<{ball_low} --> [on]>. :|:";
            //SetSensor("right");
        }
        else if (ballIsEven)
        {
            input = "<{ball_even} --> [on]>. :|:";
            //SetSensor("center");
        }
        else
        {
            //SetSensor("off");
        }

        if (input != "")//&& input != lastVerticalPositionInput)
        {
            GetNARSHost().AddInput(input);
            lastVerticalPositionInput = input;
        }
    }

    void DetectWalls()
    {
        string input = "";

        if (isHittingLeftWall)
        {
            input = "<{left_wall} --> [on]>. :|:";
        }
        else if (isHittingRightWall)
        {
            input = "<{right_wall} --> [on]>. :|:";
        }
        else
        {
            input = "(--,<{left_wall} --> [on]>). :|:";
            input += "(\n--,<{right_wall} --> [on]>). :|:";
        }

        if (input != "")//&& input != lastWallInput)
        {
            GetNARSHost().AddInput(input);
            lastWallInput = input;
        }
    }

    #endregion
    void CalculateAndInputHorizontalRelativePositionSensorData()
    {
        string input = "";
        //Need to move
        Vector3 ballDirection = ball.transform.position - paddle.transform.position;
        bool ballIsLeft = Vector3.Dot(paddle.transform.right, ballDirection) < 0 && Mathf.Abs(ball.transform.position.x - paddle.transform.position.x) > width / 2f;
        bool ballIsRight = Vector3.Dot(paddle.transform.right, ballDirection) > 0 && Mathf.Abs(ball.transform.position.x - paddle.transform.position.x) > width / 2f;
        bool ballIsCenter = !ballIsLeft && !ballIsRight;

        if (ballIsLeft)
        {
            input = "<{ball_to_left} --> [on]>. :|:";
            SetSensor("left");
        }
        else if (ballIsRight)
        {
            input = "<{ball_to_right} --> [on]>. :|:";
            SetSensor("right");
        }
        else if (ballIsCenter)
        {
            input = "<{ball_to_center} --> [on]>. :|:";
            SetSensor("center");
        }
        else
        {
            SetSensor("off");
        }

        if (input != "")// && input != lastHorizontalRelativePositionInput)
        {
            GetNARSHost().AddInput(input);
            lastHorizontalRelativePositionInput = input;
        }
    }

    void SetSensor(string position)
    {
        if(position == "left")
        {
            leftSensor.material.color = oncolor;
            centerSensor.material.color = offColor;
            rightSensor.material.color = offColor;
        }else if(position == "center")
        {
            leftSensor.material.color = offColor;
            centerSensor.material.color = oncolor;
            rightSensor.material.color = offColor;
        }
        else if(position == "right")
        {
            leftSensor.material.color = offColor;
            centerSensor.material.color = offColor;
            rightSensor.material.color = oncolor;
        }
        else if (position == "off")
        {
            leftSensor.material.color = offColor;
            centerSensor.material.color = offColor;
            rightSensor.material.color = offColor;
        }
    }

    public void MoveLeft()
    {
        paddle.goLeft = true;
        paddle.goRight = false;
    }

    public void MoveRight()
    {
        paddle.goRight = true;
        paddle.goLeft = false;
    }

    public void DontMove()
    {
       paddle.goRight = false;
       paddle.goLeft = false;
    }

    public void RemindGoal()
    {
        //NARSHost.GetInstance().AddInput("(--,<{SELF} --> [bad]>)! :|:");
        GetNARSHost().AddInput("<{SELF} --> [good]>! :|:");
    }

    public void Punish()
    {
        Debug.Log("BAD NARS");
        //NARSHost.GetInstance().AddInput("(--,<{SELF} --> [good]>). :|:");
        //NARSHost.GetInstance().AddInput("(--,<{SELF} --> [good]>). :|:");
        //NARSHost.GetInstance().AddInput("<{SELF} --> [bad]>. :|:");
    }

    public void Praise()
    {
        Debug.Log("GOOD " + GetNARSHost().type.ToString());
        GetNARSHost().AddInput("<{SELF} --> [good]>. :|:");
    }
}
