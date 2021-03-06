﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScorePlayerRow : MonoBehaviour {


    private string score;
    private string position;
    private string playerName;

    public Text ScoreText;
    public Text PositionText;
    public Text UserNameText;
   


    public void SetScores(string playerName, string score, string position, bool isPlayer)
    {
        ScoreText.text = score;
        PositionText.text = position;
        UserNameText.text = playerName;

        ScoreText.color = isPlayer ? Color.red:Color.white;
        PositionText.color = isPlayer ? Color.red : Color.white;
        UserNameText.color = isPlayer ? Color.red : Color.white;
    }

}
