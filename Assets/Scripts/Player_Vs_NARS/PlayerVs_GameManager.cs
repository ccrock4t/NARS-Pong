using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerVs_GameManager : MonoBehaviour
{
    private static PlayerVs_GameManager _instance;
    int playerPoints = 0, NARSPoints = 0, NARSBlocks = 0;

    PlayerVs_GameManager()
    {

    }

    private void Start()
    {
        _instance = this;
    }

    public static PlayerVs_GameManager GetInstance()
    {
        return _instance;
    }

    public int GetPlayerPoints()
    {
        return playerPoints;
    }

    public void AddPlayerPoint()
    {
        playerPoints++;
        GetComponent<PlayerVs_ScoreboardGUI>().UpdatePlayerPointsText(playerPoints);
    }

    public int GetNARSPoints()
    {
        return NARSPoints;
    }

    public void AddNARSPoint()
    {
        NARSPoints++;
        GetComponent<PlayerVs_ScoreboardGUI>().UpdateNARSPointsText(NARSPoints);
    }

    public int GetNARSBlocks()
    {
        return NARSPoints;
    }

    public void AddNARSBlock()
    {
        NARSBlocks++;
        GetComponent<PlayerVs_ScoreboardGUI>().UpdateNARSBlocksText(NARSBlocks);
    }
}
