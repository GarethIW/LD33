using UnityEngine;
using System.Collections;
using System;

public class PlayerScoreResult {

    public string PlayerName;
    public Int32 PlayerScore;
    public string PlayDate;
    public string PlayerId;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="playerScore"></param>
    /// <param name="playerDate"></param>
    PlayerScoreResult(string playerName,Int32 playerScore,string playerDate,string playerId)
    {
        this.PlayerName = playerName;
        this.PlayerScore = playerScore;
        this.PlayDate = playerDate;
        this.PlayerId = playerId;
    }



}
