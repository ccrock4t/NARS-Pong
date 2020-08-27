using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NARSPong_ScoreboardGUI : MonoBehaviour
{
    public Text missesText, blocksText;

    public void UpdateScoreboardMisses(int score)
    {
        missesText.text = "Misses: " + score;
    }

    public void UpdateScoreboardBlocks(int score)
    {
        blocksText.text = "Blocks: " + score;
    }
}
