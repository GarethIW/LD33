using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.UI;

public class HighScoreTest : MonoBehaviour
{

    public InputField name;
    public InputField score;

    private string playerId;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SubmitScore()
    {
        
        playerId = Guid.NewGuid().ToString().Replace("-","");
        Hashtable data = new Hashtable();
        data.Add("playerId", playerId);
        data.Add("name", name.text);
        data.Add("score", Convert.ToInt32(score.text));

        string key = "063f5e89611ff77b" + data["playerId"] + data["score"];
        var bytes = Encoding.UTF8.GetBytes("068d900156e6603d7a804b29570550052e14f42f0ce01dc92b803ec95b886a4c");
        using(var hmacsha512 = new HMACSHA512(bytes))
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
                    //Hashtable getResult = getScores.response.Object;

                });
            }

        });
    }

    string ByteToString(byte[] buff)
    {
        string sbinary = "";
        for (int i = 0; i < buff.Length; i++)
            sbinary += buff[i].ToString("X2"); /* hex format */
        return sbinary;
    }

}
