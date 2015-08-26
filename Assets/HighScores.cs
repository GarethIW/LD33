using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class HighScores : MonoBehaviour
{
    public string LeaderboardId;
    public string LeaderboardSecret;

    public InputField Name;

    public HighScorePlayerRow[] AllScores;
    public HighScorePlayerRow[] PlayerAllScores;
    public HighScorePlayerRow[] TodayScores;
    public HighScorePlayerRow[] PlayerTodayScores;

    public Text Civ;
    public Text Mil;
    public Text Prop;
    public Text Score;

    public GameObject AllScorePanels;
    public GameObject SubmitPanel;

    private string playerId;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null) return;

        if (Civ == null) return;

        Civ.text = GameManager.Instance.Civilians.ToString();
        Mil.text = GameManager.Instance.Military.ToString();
        Prop.text = GameManager.Instance.DamageCost.ToString();
        Score.text = GameManager.Instance.score.ToString();
    }



    public void SubmitScore()
    {
        SubmitPanel.SetActive(false);
        AllScorePanels.SetActive(true);

        playerId = Guid.NewGuid().ToString().Replace("-", "");
        Hashtable data = new Hashtable();
        data.Add("playerId", playerId);
        data.Add("name", Name.text.Substring(0, Name.text.Length > 15 ? 15 : Name.text.Length));
        data.Add("score", GameManager.Instance.score);

        string key = LeaderboardId + data["playerId"] + data["score"];
        var bytes = Encoding.UTF8.GetBytes(LeaderboardSecret);
        using (var hmacsha512 = new HMACSHA512(bytes))
        {
            hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(key));
            data.Add("signature", ByteToString(hmacsha512.Hash).ToLower());
        }

#if UNITY_WEBPLAYER
        Application.ExternalCall("submitScore", LeaderboardId, data["playerId"], data["name"], data["score"],
            data["signature"], "ScoreBoardCanvas", "SubmitCallback");
#else
        HTTP.Request theRequest = new HTTP.Request("put", "https://api.leaderboards.io/leaderboard/" + LeaderboardId + "/score", data);
        theRequest.Send((request) =>
        {
            Hashtable result = request.response.Object;
            if (result != null)
            {
                SubmitCallback();
            }
        });
#endif
    }

    public void SubmitCallback()
    {
#if UNITY_WEBPLAYER
        Application.ExternalCall("getScores", LeaderboardId, "today", "ScoreBoardCanvas", "TodayCallback");
        Application.ExternalCall("getScores", LeaderboardId, "all", "ScoreBoardCanvas", "AllCallback");
#else
        GetScores("today", TodayCallback);
        GetScores("all", AllCallback);
#endif
    }

    public void TodayCallback(string response)
    {
        var scores = JSON.JsonDecode(response);
        if (scores.GetType().FullName.Equals("System.Collections.ArrayList"))
        {
            ArrayList results = (ArrayList)scores;

            for (int i = 0; i < 10; i++)
            {
                if (i >= results.Count) break;

                Hashtable playerResult = (Hashtable)results[i];
                string player = (string)playerResult["name"];
                string playerIdentity = (string)playerResult["playerId"];
                Int32 playerScore = (Int32)playerResult["score"];
                TodayScores[i].SetScores(player, playerScore.ToString(), (i + 1).ToString(), playerIdentity == playerId);
            }

            int foundPos = -1;
            for (int i = 0; i < results.Count; i++)
            {
                Hashtable playerResult = (Hashtable)results[i];
                if ((string)playerResult["playerId"] == playerId)
                {
                    foundPos = i;
                    break;
                }
            }

            if (foundPos == -1) foundPos = 0;

            int count = 0;
            int lastPos = 0;
            for (int i = foundPos - 4; i < foundPos + 4; i++)
            {
                if (i >= 0 && i<results.Count && count<10)
                {
                    Hashtable playerResult = (Hashtable)results[i];
                    string player = (string)playerResult["name"];
                    string playerIdentity = (string)playerResult["playerId"];
                    Int32 playerScore = (Int32)playerResult["score"];
                    PlayerTodayScores[count].SetScores(player, playerScore.ToString(), (i + 1).ToString(), playerIdentity == playerId);
                    count++;
                    lastPos = i+1;
                }
            }

            if (count < 10)
            {
                for (int i = lastPos; i < results.Count; i++)
                {
                    if (i >= 0 && i < results.Count && count < 10)
                    {
                        Hashtable playerResult = (Hashtable)results[i];
                        string player = (string)playerResult["name"];
                        string playerIdentity = (string)playerResult["playerId"];
                        Int32 playerScore = (Int32)playerResult["score"];
                        PlayerTodayScores[count].SetScores(player, playerScore.ToString(), (i + 1).ToString(), playerIdentity == playerId);
                        count++;
                    }
                }
            }
        }
    }

    public void AllCallback(string response)
    {
        var scores = JSON.JsonDecode(response);
        if (scores.GetType().FullName.Equals("System.Collections.ArrayList"))
        {
            ArrayList results = (ArrayList)scores;

            for (int i = 0; i < 10; i++)
            {
                if (i >= results.Count) break;

                Hashtable playerResult = (Hashtable)results[i];
                string player = (string)playerResult["name"];
                string playerIdentity = (string)playerResult["playerId"];
                Int32 playerScore = (Int32)playerResult["score"];
                string date = (string)playerResult["time"];

                AllScores[i].SetScores(player,playerScore.ToString(), (i+1).ToString(), playerIdentity==playerId);
            }

            int foundPos = -1;
            for (int i = 0; i < results.Count; i++)
            {
                Hashtable playerResult = (Hashtable)results[i];
                if ((string)playerResult["playerId"] == playerId)
                {
                    foundPos = i;
                    break;
                }
            }

            if (foundPos == -1) foundPos = 0;

            int count = 0;
            int lastPos = 0;
            for (int i = foundPos - 4; i < foundPos + 4; i++)
            {
                if (i >= 0 && i < results.Count && count < 10)
                {
                    Hashtable playerResult = (Hashtable)results[i];
                    string player = (string)playerResult["name"];
                    string playerIdentity = (string)playerResult["playerId"];
                    Int32 playerScore = (Int32)playerResult["score"];
                    PlayerAllScores[count].SetScores(player, playerScore.ToString(), (i + 1).ToString(), playerIdentity == playerId);
                    count++;
                    lastPos = i + 1;
                }
            }

            if (count < 10)
            {
                for (int i = lastPos; i < results.Count; i++)
                {
                    if (i >= 0 && i < results.Count && count < 10)
                    {
                        Hashtable playerResult = (Hashtable)results[i];
                        string player = (string)playerResult["name"];
                        string playerIdentity = (string)playerResult["playerId"];
                        Int32 playerScore = (Int32)playerResult["score"];
                        PlayerAllScores[count].SetScores(player, playerScore.ToString(), (i + 1).ToString(), playerIdentity == playerId);
                        count++;
                    }
                }
            }
        }
    }

    private void GetScores(string timeFrame, Action<string> callback)
    {
        HTTP.Request someRequest = new HTTP.Request("get", "https://api.leaderboards.io/leaderboard/" + LeaderboardId + "/" + timeFrame);
        someRequest.Send((getScores) =>
        {
            callback(getScores.response.Text);
        });
    }

    // we provide Object and Array convenience methods that attempt to parse the response as JSON
    // if the response cannot be parsed, we will return null
    // note that if you want to send json that isn't either an object ({...}) or an array ([...])
    // that you should use JSON.JsonDecode directly on the response.Text, Object and Array are
    // only provided for convenience
    //    Debug.LogError(request.exception);
    //    Hashtable result = request.response.Object;
    //    if (result != null)
    //    {
    //        HTTP.Request someRequest = new HTTP.Request("get", "https://api.leaderboards.io/leaderboard/063f5e89611ff77b/all");
    //        someRequest.Send((getScores) =>
    //        {
    //            // parse some JSON, for example:
    //            var scores = JSON.JsonDecode(getScores.response.Text);


    //            if (scores.GetType().FullName.Equals("System.Collections.ArrayList"))
    //            {
    //                ArrayList results = (ArrayList)scores;
    //                int p = 1;

    //                for (int i = 0; i <= results.Count - 1; i++)
    //                {
    //                    Hashtable playerResult = (Hashtable)results[i];

    //                    string player = (string)playerResult["name"];
    //                    string playerIdentity = (string)playerResult["playerId"];
    //                    Int32 playerScore = (Int32)playerResult["score"];
    //                    string date = (string)playerResult["time"];

    //                    //PlayerScoreResult player = new PlayerScoreResult(player, playerScore,);


    //                    Debug.Log(i + " " + player + " " + playerIdentity + " " + playerScore);

    //                    // all scores
    //                    if (i <= 9)
    //                    {
    //                        AllHighScores[i].SetScores(player, playerScore.ToString(), p.ToString());
    //                        p++;
    //                    }


    //                    //player ranking scores
    //                    if (playerIdentity.Equals(playerId))
    //                    {
    //                        int cc = 0;
    //                        int index = getIndexNumber(i, results.Count);

    //                        if (index < 0) index = 0;
    //                        for (int x = index; x <= ((index + 9 < results.Count) ? index + 9 : results.Count - 1); x++)
    //                        {

    //                            if (i + x <= results.Count && i + x >= 0)
    //                            {


    //                                Hashtable playerRankResult = (Hashtable)results[i + x];
    //                                string rankPlayer = (string)playerRankResult["name"];
    //                                string rankPlayerIdentity = (string)playerRankResult["playerId"];
    //                                Int32 rankPlayerScore = (Int32)playerRankResult["score"];
    //                                string rankDate = (string)playerRankResult["time"];

    //                                PlayerHighScores[cc].SetScores(rankPlayer, rankPlayerScore.ToString(), (i +cc).ToString());
    //                                cc++;
    //                            }
    //                        }


    //                    }


    //                }




    //            }
    //            //Hashtable getResult = getScores.response.Object;

    //        });

    //        getTodaysScores();
    //    }

    //});


    //Debug.Log("unz");

    //SubmitPanel.SetActive(false);

    //AllScorePanels.SetActive(true);





    int getIndexNumber(int i, int max)
    {
        int result = 0;

        if (i == 0)
        {
            result = 0;
        }
        else if (i == 1)
        {
            result = -4;
        }
        else if (i == 2)
        {
            result = -3;
        }
        else if (i == 3)
        {
            result = -4;
        }
        else if (i == max)
        {
            result = max - 10;
        }
        else if (i + 1 == max)
        {
            result = max - 9;
        }
        else if (i + 2 == max)
        {
            result = max - 8;
        }
        else if (i + 3 == max)
        {
            result = max - 8;
        }
        else if (i + 4 == max)
        {
            result = max - 7;
        }
        else if (i + 5 == max)
        {
            result = max - 6;
        }
        else
        {
            result = -5;
        }



        Debug.Log("getIndexNumber() returning " + result + "From i:" + i + " Max " + max);
        return result;
    }



    string ByteToString(byte[] buff)
    {
        string sbinary = "";
        for (int i = 0; i < buff.Length; i++)
            sbinary += buff[i].ToString("X2"); /* hex format */
        return sbinary;
    }

    public void PlayAgain()
    {
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

   

    // parse some JSON, for example:
        //    var scores = JSON.JsonDecode(getScores.response.Text);


        //    if (scores.GetType().FullName.Equals("System.Collections.ArrayList"))
        //    {
        //        ArrayList results = (ArrayList)scores;
        //        int p = 1;

        //        for (int i = 0; i <= results.Count - 1; i++)
        //        {
        //            Hashtable playerResult = (Hashtable)results[i];

        //            string player = (string)playerResult["name"];
        //            string playerIdentity = (string)playerResult["playerId"];
        //            Int32 playerScore = (Int32)playerResult["score"];
        //            string date = (string)playerResult["time"];

        //            //PlayerScoreResult player = new PlayerScoreResult(player, playerScore,);


        //            Debug.Log(i + " " + player + " " + playerIdentity + " " + playerScore);

        //            // all scores
        //            if (i <= 9)
        //            {
        //                AllHighScoresToday[i].SetScores(player, playerScore.ToString(), p.ToString());
        //                p++;
        //            }


        //            //player ranking scores
        //            if (playerIdentity.Equals(playerId))
        //            {
        //                int cc = 0;
        //                int index = getIndexNumber(i, results.Count);

        //                for (int x = index; x <= ((index + 9<results.Count)?index+9:results.Count-1); x++)
        //                {

        //                    if (i + x <= results.Count && i+x>=0)
        //                    {


        //                        Hashtable playerRankResult = (Hashtable)results[i + x];
        //                        string rankPlayer = (string)playerRankResult["name"];
        //                        string rankPlayerIdentity = (string)playerRankResult["playerId"];
        //                        Int32 rankPlayerScore = (Int32)playerRankResult["score"];
        //                        string rankDate = (string)playerRankResult["time"];

        //                        PlayerHighScoresToday[cc].SetScores(rankPlayer, rankPlayerScore.ToString(), (i + cc).ToString());
        //                        cc++;
        //                    }
        //                }


        //            }


        //        }

        //      //  AllScorePanels.SetActive(true);


        //    }
        //    //Hashtable getResult = getScores.response.Object;

        //});
    //}
    

}
