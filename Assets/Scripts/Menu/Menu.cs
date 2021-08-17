using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void LoadNARSPong()
    {
        SceneManager.LoadScene("NARS_Pong");
    }

    public void LoadONAPong()
    {
        SceneManager.LoadScene("ONA_Pong");
    }

    public void LoadPythonPong()
    {
        SceneManager.LoadScene("Python_Pong");
    }

    public void LoadPlayerVsNARS()
    {
        SceneManager.LoadScene("Player_Vs_NARS");
    }

    public void LoadPlayerVsONA()
    {
        SceneManager.LoadScene("Player_Vs_ONA");
    }


    public void LoadPlayerVsPython()
    {
        SceneManager.LoadScene("Player_Vs_Python");
    }


    public void LoadNARSVsONA()
    {
        SceneManager.LoadScene("NARS_Vs_ONA");
    }

    public void LoadNARSVsPython()
    {
        SceneManager.LoadScene("NARS_Vs_Python");
    }

    public void LoadONAVsPython()
    {
        SceneManager.LoadScene("ONA_Vs_Python");
    }
}
