using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVs_ScoreboardGUI : MonoBehaviour
{
    public Text playerPointsText, NARSPointsText, NARSBlocksText;

    public void UpdatePlayerPointsText(int score)
    {
        playerPointsText.text = "Player: " + score;
    }

    public void UpdateNARSPointsText(int score)
    {
        NARSPointsText.text = "NARS: " + score;
    }

    public void UpdateNARSBlocksText(int score)
    {
        NARSBlocksText.text = "NARS\nBlocks: " + score;
    }
}
