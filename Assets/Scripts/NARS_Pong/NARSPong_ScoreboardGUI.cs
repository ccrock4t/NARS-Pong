using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NARSPong_ScoreboardGUI : MonoBehaviour
{
    public Text missesText, blocksText;

    private string resultspath = "Assets/results.txt";
    private StreamWriter writer = null;

    public void Start()
    {
        if (File.Exists(resultspath))
            File.Delete(resultspath);
        writer = new StreamWriter(resultspath, true);
    }

    public void UpdateScoreboardMisses(int score)
    {
        missesText.text = "Misses: " + score;
        writer.WriteLine("miss");
    }

    public void UpdateScoreboardBlocks(int score)
    {
        blocksText.text = "Blocks: " + score;
        writer.WriteLine("block");
    }

    public void OnDestroy()
    {
        writer.Close();
    }
}
