using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NARSVsONA_ScoreboardGUI : MonoBehaviour
{
    public Text NARSBlocksText, ONABlocksText;

    public void UpdateNARSBlocksText(int score)
    {
        NARSBlocksText.text = "NARS\nBlocks: " + score;
    }

    public void UpdateONABlocksText(int score)
    {
        ONABlocksText.text = "ONA\nBlocks: " + score;
    }

}
