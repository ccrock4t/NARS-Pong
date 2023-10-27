using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSVsNARS_GameManager : MonoBehaviour
{
    private static NARSVsNARS_GameManager _instance;
    int NARS1Blocks = 0, NARS2Blocks = 0;
    public NARSHost NARS1, NARS2;

    NARSVsNARS_GameManager()
    {

    }

    public void RegisterNARS1(NARSHost host)
    {
        NARS1 = host;
    }

    public void RegisterNARS2(NARSHost host)
    {
        NARS2 = host;
    }



    private void Start()
    {
        _instance = this;
    }

    public static NARSVsNARS_GameManager GetInstance()
    {
        return _instance;
    }

    public int GetNARS1Blocks()
    {
        return NARS1Blocks;
    }

    public void AddNARS1Block()
    {
        NARS1Blocks++;
        GetComponent<NARSVsNARS_ScoreboardGUI>().UpdateNARS1BlocksText(NARS1Blocks);
    }

    public int GetNARS2Blocks()
    {
        return NARS2Blocks;
    }

    public void AddNARS2Block()
    {
        NARS2Blocks++;
        GetComponent<NARSVsNARS_ScoreboardGUI>().UpdateNARS2BlocksText(NARS2Blocks);
    }
}
