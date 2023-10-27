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
    public Color offColor = Color.black, onColor = Color.yellow;

    float TIMER_DURATION = 0.07f;
    float GOAL_TIMER_DURATION = 0.07f;
    float timer = 5, inferenceTimer = 0, goalTimer = 0, teachCount = 0;
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

        //timer = TIMER_DURATION;
        //inferenceTimer = TIMER_DURATION / 2f;
    }

    public void SetNARSHost(NARSHost host)
    {
        _host = host;
    }

    public NARSHost GetNARSHost()
    {
        return _host;
    }

    int goals_sent = 0;

    // Update is called once per frame
    string global_sensor_string, previous_sensor_string;
    void Update()
    {
        timer -= Time.deltaTime;
        inferenceTimer -= Time.deltaTime;
        goalTimer -= Time.deltaTime;

  
        global_sensor_string = "";
        //Various test sensors
        //CalculateAndInputDistanceSensorData();
        //CalculateAndInputRadialSensorData();
        //CalculateHorizontalVelocitySensorData();
        //CalculateAndInputVerticalVelocitySensorData();
        //CalculateAndInputVerticalPositionSensorData();
        //DetectWalls();

        CalculateHorizontalRelativePositionSensorData();

        global_sensor_string = "<{ball} --> [" + global_sensor_string.Substring(1) + "]" + ">. :|:";
        if (timer < 0 )//!global_sensor_string.Equals(previous_sensor_string))
        {
            timer = TIMER_DURATION;
            previous_sensor_string = global_sensor_string;
            GetNARSHost().AddInput(global_sensor_string);
        }

        if (goalTimer <= 0f)
        {
            RemindGoal();
            if (teachCount < 10)
            {
                Teach();
                teachCount++;
            }
            goalTimer = GOAL_TIMER_DURATION;
        }




    }

#region test sensors
    void CalculateAndInputRadialSensorData()
    {
        //Need to move
        Vector3 targetDir = ball.transform.position - paddle.transform.position;
        float angle = Vector3.SignedAngle(targetDir, paddle.transform.forward, Vector3.up);
       
        angle *= -1;

        if (angle <= -45f)
        {
            global_sensor_string += ",farLeft";
            SetRadialSensor("left");
        }
        else if (angle > -45f && angle < -15f)
        {
            global_sensor_string += ",nearLeft";
            SetRadialSensor("center_left");
        }
        if (angle >= -15f && angle <= 15f)
        {
            global_sensor_string += ",center";
            SetRadialSensor("center");
        }
        else if (angle > 15f && angle < 45f)
        {
            global_sensor_string += ",nearRight";
            SetRadialSensor("center_right");
        }
        else if (angle >= 45f)
        {
            global_sensor_string += ",farRight";
            SetRadialSensor("right");
        }

    }

    void SetRadialSensor(string position)
    {
        if (position == "left")
        {
            radialLeftSensor.material.color = onColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = offColor;
        }
        else if (position == "center")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = onColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = offColor;
        }
        else if (position == "right")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = onColor;
        }
        else if (position == "center_left")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = onColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = offColor;
            radialRightSensor.material.color = offColor;
        }
        else if (position == "center_right")
        {
            radialLeftSensor.material.color = offColor;
            radialCenterLeftSensor.material.color = offColor;
            radialCenterSensor.material.color = offColor;
            radialCenterRightSensor.material.color = onColor;
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

    void CalculateHorizontalVelocitySensorData()
    {
        //Need to move
        bool ballMovingRight = ball.transform.GetComponent<Rigidbody>().velocity.x > 0;
        bool ballMovingLeft = ball.transform.GetComponent<Rigidbody>().velocity.x < 0;

        string property = "";
        if (ballMovingLeft)
        {
            property += ",movingLeft";
        }
        else if (ballMovingRight)
        {
            property += ",movingRight";
        }
        else
        {
            property += ",notMoving";
        }

        global_sensor_string += property;
        //string sensor_string = "<{ball} --> [" + property.Substring(1) + "]" + ">. :|:";
        //GetNARSHost().AddInput(sensor_string);

    }

    void CalculateAndInputVerticalVelocitySensorData()
    {
        //Need to move
        bool ballMovingUp = ball.transform.GetComponent<Rigidbody>().velocity.z > 0;
        bool ballMovingDown = ball.transform.GetComponent<Rigidbody>().velocity.z < 0;

        if (ballMovingUp)
        {
            global_sensor_string += ",movingUp";
        }
        else if (ballMovingDown)
        {
            global_sensor_string += ",movingDown";
        }
    }

    void CalculateAndInputDistanceSensorData()
    {
        string property = "";
        if (Vector3.Distance(ball.transform.position, paddle.transform.position) <= 4f)
        {
            property += ",near";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) < 8f && Vector3.Distance(ball.transform.position, paddle.transform.position) > 4f)
        {
            property += ",nearMiddle";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) <= 12f && Vector3.Distance(ball.transform.position, paddle.transform.position) >= 8f)
        {
            property += ",middle";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) < 16f && Vector3.Distance(ball.transform.position, paddle.transform.position) > 12f)
        {
            property += ",middleFar";
        }
        else if (Vector3.Distance(ball.transform.position, paddle.transform.position) >= 16f)
        {
            property += ",far";
        }

        global_sensor_string += property;
        //string sensor_string = "<{ball} --> [" + property.Substring(1) + "]" + ">. :|:";
       // GetNARSHost().AddInput(sensor_string);
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
            global_sensor_string += ",high";
            // SetSensor("left");
        }
        else if (ballIsLow)
        {
            global_sensor_string += ",low";
            //SetSensor("right");
        }
        else if (ballIsEven)
        {
            global_sensor_string += ",even";
            //SetSensor("center");
        }
        else
        {
            //SetSensor("off");
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
    void CalculateHorizontalRelativePositionSensorData()
    {
        //Need to move
        Vector3 ballDirection = ball.transform.position - paddle.transform.position;
        bool ballIsLeft = Vector3.Dot(paddle.transform.right, ballDirection) < 0 && Mathf.Abs(ball.transform.position.x - paddle.transform.position.x) > width / 2f;
        bool ballIsRight = Vector3.Dot(paddle.transform.right, ballDirection) > 0 && Mathf.Abs(ball.transform.position.x - paddle.transform.position.x) > width / 2f;
        bool ballIsCenter = !ballIsLeft && !ballIsRight;
        string property = "";
        if (ballIsLeft)
        {
            property += ",left";
            SetSensor("left");
        }
        else if (ballIsRight)
        {
            property += ",right";
            SetSensor("right");
        }else if (ballIsCenter)
        {
            property += ",center";
            SetSensor("center");
        }
        global_sensor_string += property;
       // string sensor_string = "<{ball} --> [" + property.Substring(1) + "]" + ">. :|:";
       // GetNARSHost().AddInput(sensor_string);
    }

    void SetSensor(string position)
    {
        if(position == "left")
        {
            leftSensor.material.color = onColor;
            centerSensor.material.color = offColor;
            rightSensor.material.color = offColor;
        }else if(position == "center")
        {
            leftSensor.material.color = offColor;
            centerSensor.material.color = onColor;
            rightSensor.material.color = offColor;
        }
        else if(position == "right")
        {
            leftSensor.material.color = offColor;
            centerSensor.material.color = offColor;
            rightSensor.material.color = onColor;
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
        string goal = "<{SELF} --> [good]>! :|:";
        GetNARSHost().AddInput(goal);
        //GetNARSHost().AddInput("(--,<{SELF} --> [bad]>)! :|:");
    }

    public void Teach()
    {
        //return;
        //GetNARSHost().AddInput("((&/,({ball} --> [left]),((*,{SELF}) --> left)) =/> ({ball} --> [center])).");
        //GetNARSHost().AddInput("((&/,({ball} --> [right]),((*,{SELF}) --> right)) =/> ({ball} --> [center])).");
        //GetNARSHost().AddInput("((&/,({ball} --> [left]),((*,{SELF}) --> left)) =/> ({ball} --> [right])).");
        //GetNARSHost().AddInput("((&/,({ball} --> [right]),((*,{SELF}) --> right)) =/> ({ball} --> [left])).");
        GetNARSHost().AddInput("((&/,({ball} --> [center]),((*,{SELF}) --> deactivate)) =/> ({SELF} --> [good])).");
        GetNARSHost().AddInput("((&/,({ball} --> [left]),((*,{SELF}) --> left)) =/> ({SELF} --> [good])).");
        GetNARSHost().AddInput("((&/,({ball} --> [right]),((*,{SELF}) --> right)) =/> ({SELF} --> [good])).");

    }

    public void Punish()
    {
        Debug.Log("BAD NARS");
        //GetNARSHost().AddInput("<{SELF} --> [bad]>. :|:");
    }

    public void Praise()
    {
        Debug.Log("GOOD " + GetNARSHost().type.ToString());
        GetNARSHost().AddInput("<{SELF} --> [good]>. :|:");
        //GetNARSHost().AddInput("<{SELF} --> [good]>. :|:");
        
    }
}
