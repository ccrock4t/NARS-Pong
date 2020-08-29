using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NARSVsONA_GameManager : MonoBehaviour
{
    private static NARSVsONA_GameManager _instance;
    int NARSBlocks = 0, ONABlocks = 0;

    NARSVsONA_GameManager()
    {

    }

    private void Start()
    {
        _instance = this;
    }

    public static NARSVsONA_GameManager GetInstance()
    {
        return _instance;
    }

    public int GetNARSBlocks()
    {
        return NARSBlocks;
    }

    public void AddNARSBlock()
    {
        NARSBlocks++;
        GetComponent<NARSVsONA_ScoreboardGUI>().UpdateNARSBlocksText(NARSBlocks);
    }

    public int GetONABlocks()
    {
        return ONABlocks;
    }

    public void AddONABlock()
    {
        ONABlocks++;
        GetComponent<NARSVsONA_ScoreboardGUI>().UpdateONABlocksText(ONABlocks);
    }
}
