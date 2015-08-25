using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.UI;
using System.Collections.Generic;

public class HighScoreTest : MonoBehaviour
{

    public InputField name;
    public InputField score;

    private string playerId;
    public HighScorePlayerRow[] AllHighScores;
    public HighScorePlayerRow[] PlayerHighScores;

    public HighScorePlayerRow[] AllHighScoresToday;
    public HighScorePlayerRow[] PlayerHighScoresToday;


    public GameObject AllScorePanels;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubmitScore()
    {

        playerId = Guid.NewGuid().ToString().Replace("-", "");
        Hashtable data = new Hashtable();
        data.Add("playerId", playerId);
        data.Add("name", name.text);
        data.Add("score", Convert.ToInt32(score.text));

        string key = "063f5e89611ff77b" + data["playerId"] + data["score"];
        var bytes = Encoding.UTF8.GetBytes("068d900156e6603d7a804b29570550052e14f42f0ce01dc92b803ec95b886a4c");
        using (var hmacsha512 = new HMACSHA512(bytes))
        {
            hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(key));
            data.Add("signature", ByteToString(hmacsha512.Hash).ToLower());
            //Console.WriteLine("Result: {0}", ByteToString(hmacsha256.Hash));
        }






        // When you pass a Hashtable as the third argument, we assume you want it send as JSON-encoded
        // data.  We'll encode it to JSON for you and set the Content-Type header to application/json
        HTTP.Request theRequest = new HTTP.Request("put", "https://api.leaderboards.io/leaderboard/063f5e89611ff77b/score", data);
        theRequest.Send((request) =>
        {

            // we provide Object and Array convenience methods that attempt to parse the response as JSON
            // if the response cannot be parsed, we will return null
            // note that if you want to send json that isn't either an object ({...}) or an array ([...])
            // that you should use JSON.JsonDecode directly on the response.Text, Object and Array are
            // only provided for convenience
            Hashtable result = request.response.Object;
            if (result != null)
            {
                HTTP.Request someRequest = new HTTP.Request("get", "https://api.leaderboards.io/leaderboard/063f5e89611ff77b/all");
                someRequest.Send((getScores) =>
                {
                    // parse some JSON, for example:
                    var scores = JSON.JsonDecode(getScores.response.Text);


                    if (scores.GetType().FullName.Equals("System.Collections.ArrayList"))
                    {
                        ArrayList results = (ArrayList)scores;
                        int p = 1;

                        for (int i = 0; i <= results.Count - 1; i++)
                        {
                            Hashtable playerResult = (Hashtable)results[i];

                            string player = (string)playerResult["name"];
                            string playerIdentity = (string)playerResult["playerId"];
                            Int32 playerScore = (Int32)playerResult["score"];
                            string date = (string)playerResult["time"];

                            //PlayerScoreResult player = new PlayerScoreResult(player, playerScore,);


                            Debug.Log(i + " " + player + " " + playerIdentity + " " + playerScore);

                            // all scores
                            if (i <= 9)
                            {
                                AllHighScores[i].SetScores(player, playerScore.ToString(), p.ToString());
                                p++;
                            }


                            //player ranking scores
                            if (playerIdentity.Equals(playerId))
                            {
                                int cc = 0;
                                int index = getIndexNumber(i, results.Count);

                                for (int x = index; x <= index + 9; x++)
                                {

                                    if (i + x <= results.Count)
                                    {


                                        Hashtable playerRankResult = (Hashtable)results[i + x];
                                        string rankPlayer = (string)playerRankResult["name"];
                                        string rankPlayerIdentity = (string)playerRankResult["playerId"];
                                        Int32 rankPlayerScore = (Int32)playerRankResult["score"];
                                        string rankDate = (string)playerRankResult["time"];

                                        PlayerHighScores[cc].SetScores(rankPlayer, rankPlayerScore.ToString(), (i - x).ToString());
                                        cc++;
                                    }
                                }


                            }


                        }

                      


                    }
                    //Hashtable getResult = getScores.response.Object;

                });
            }

        });

        getTodaysScores();
        AllScorePanels.SetActive(true);


    }


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

    void getTodaysScores()
    {
       
                HTTP.Request someRequest = new HTTP.Request("get", "https://api.leaderboards.io/leaderboard/063f5e89611ff77b/today");
                someRequest.Send((getScores) =>
                {
                    // parse some JSON, for example:
                    var scores = JSON.JsonDecode(getScores.response.Text);


                    if (scores.GetType().FullName.Equals("System.Collections.ArrayList"))
                    {
                        ArrayList results = (ArrayList)scores;
                        int p = 1;

                        for (int i = 0; i <= results.Count - 1; i++)
                        {
                            Hashtable playerResult = (Hashtable)results[i];

                            string player = (string)playerResult["name"];
                            string playerIdentity = (string)playerResult["playerId"];
                            Int32 playerScore = (Int32)playerResult["score"];
                            string date = (string)playerResult["time"];

                            //PlayerScoreResult player = new PlayerScoreResult(player, playerScore,);


                            Debug.Log(i + " " + player + " " + playerIdentity + " " + playerScore);

                            // all scores
                            if (i <= 9)
                            {
                                AllHighScoresToday[i].SetScores(player, playerScore.ToString(), p.ToString());
                                p++;
                            }


                            //player ranking scores
                            if (playerIdentity.Equals(playerId))
                            {
                                int cc = 0;
                                int index = getIndexNumber(i, results.Count);

                                for (int x = index; x <= index + 9; x++)
                                {

                                    if (i + x <= results.Count)
                                    {


                                        Hashtable playerRankResult = (Hashtable)results[i + x];
                                        string rankPlayer = (string)playerRankResult["name"];
                                        string rankPlayerIdentity = (string)playerRankResult["playerId"];
                                        Int32 rankPlayerScore = (Int32)playerRankResult["score"];
                                        string rankDate = (string)playerRankResult["time"];

                                        PlayerHighScoresToday[cc].SetScores(rankPlayer, rankPlayerScore.ToString(), (i - x).ToString());
                                        cc++;
                                    }
                                }


                            }


                        }

                      //  AllScorePanels.SetActive(true);


                    }
                    //Hashtable getResult = getScores.response.Object;

                });
            }
    

}
