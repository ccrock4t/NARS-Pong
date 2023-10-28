using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public TMP_Dropdown NARS1_Dropdown, NARS2_Dropdown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetNARS()
    {
        string NARS1string = NARS1_Dropdown.options[NARS1_Dropdown.value].text;
        string NARS2string = NARS2_Dropdown.options[NARS2_Dropdown.value].text;

        NARSHost.SetNARS1(NARS1string);
        NARSHost.SetNARS2(NARS2string);
    }

    void LoadNARSScene(string scene)
    {
        SetNARS();
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    public void LoadNARSPong()
    {
        LoadNARSScene("NARS_Pong");
    }


    public void LoadPlayerVsNARS()
    {
        LoadNARSScene("Player_Vs_NARS");
    }


    public void LoadNARSVsNARS()
    {
        LoadNARSScene("NARS_Vs_NARS");
    }

}
