using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationOutputGUI : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SetOutputText("NONE");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetOutputText(string text)
    {
        GetComponent<Text>().text = text;
    }
}
