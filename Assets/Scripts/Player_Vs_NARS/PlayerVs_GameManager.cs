using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerVs_GameManager : MonoBehaviour
{
    private static PlayerVs_GameManager _instance;
    int playerPoints = 0, NARSPoints = 0, NARSBlocks = 0;

    public GameObject NARS;
    public OperationOutputGUI UIOutput;
    PlayerVs_GameManager()
    {

    }

    private void Start()
    {
        _instance = this;
        NARSHost host = NARS.AddComponent<NARSHost>();
        host.type = NARSHost.NARS1;
        host.UIOutput = UIOutput;
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

    public void AddNARSPoint(string NARSTypeName)
    {
        NARSPoints++;
        GetComponent<PlayerVs_ScoreboardGUI>().UpdateNARSPointsText(NARSTypeName + ": " + NARSPoints);
    }

    public int GetNARSBlocks()
    {
        return NARSBlocks;
    }

    public void AddNARSBlock(string NARSTypeName)
    {
        NARSBlocks++;
        GetComponent<PlayerVs_ScoreboardGUI>().UpdateNARSBlocksText(NARSTypeName + "\nBlocks: " + NARSBlocks);
    }
}
