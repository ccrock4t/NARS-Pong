using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetBallButton : MonoBehaviour
{
    float disabledTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.GetComponent<Button>().enabled)
        {
            if(disabledTimer < 0)
            {
                this.GetComponent<Button>().enabled = true;
                this.GetComponent<Button>().interactable = true;
            }
            else
            {
                disabledTimer -= Time.deltaTime;
            }
        }
    }

    public void DisableFor1Sec()
    {
        disabledTimer = 1.0f;
        this.GetComponent<Button>().enabled = false;
        this.GetComponent<Button>().interactable = false;
    }
}
