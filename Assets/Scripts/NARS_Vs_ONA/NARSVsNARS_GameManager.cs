using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSVsNARS_GameManager : MonoBehaviour
{
    private static NARSVsNARS_GameManager _instance;
    int NARS1Blocks = 0, NARS2Blocks = 0;

    public GameObject NARS1GO, NARS2GO;
    public OperationOutputGUI UIOutputNARS1, UIOutputNARS2;
    public NARSHost NARS1, NARS2;

    NARSVsNARS_GameManager()
    {

    }




    private void Start()
    {
        _instance = this;
        NARS1 = NARS1GO.AddComponent<NARSHost>();
        NARS1.type = NARSHost.NARS1;
        NARS1.UIOutput = UIOutputNARS1;


        NARS2 = NARS2GO.AddComponent<NARSHost>();
        NARS2.type = NARSHost.NARS2;
        NARS2.UIOutput = UIOutputNARS2;
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
