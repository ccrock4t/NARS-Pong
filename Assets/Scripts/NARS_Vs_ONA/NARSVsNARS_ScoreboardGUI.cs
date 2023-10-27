using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NARSVsNARS_ScoreboardGUI : MonoBehaviour
{
    public Text NARS1BlocksText, NARS2BlocksText;

    public void UpdateNARS1BlocksText(int score)
    {
        NARS1BlocksText.text = NARSVsNARS_GameManager.GetInstance().NARS1.type.ToString() + "\nBlocks: " + score;
    }

    public void UpdateNARS2BlocksText(int score)
    {
        NARS2BlocksText.text = NARSVsNARS_GameManager.GetInstance().NARS2.type.ToString() + "\nBlocks: " + score;
    }

}
