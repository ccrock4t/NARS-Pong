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

    public void UpdateNARSPointsText(string text)
    {
        NARSPointsText.text = text;
    }

    public void UpdateNARSBlocksText(string text)
    {
        NARSBlocksText.text = text;
    }
}
