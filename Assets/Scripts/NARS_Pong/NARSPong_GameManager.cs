using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class NARSPong_GameManager: MonoBehaviour
{
    private static NARSPong_GameManager _instance;
    int misses = 0, blocks = 0;

    NARSPong_GameManager()
    {

    }

    private void Start()
    {
        _instance = this;
    }

    public static NARSPong_GameManager GetInstance()
    {
        return _instance;
    }

    public int GetMisses()
    {
        return misses;
    }

    public void AddMiss()
    {
        misses++;
        GetComponent<NARSPong_ScoreboardGUI>().UpdateScoreboardMisses(misses);
    }

    public int GetBlocks()
    {
        return blocks;
    }

    public void AddBlock()
    {
        blocks++;
        GetComponent<NARSPong_ScoreboardGUI>().UpdateScoreboardBlocks(blocks);
    }
}
