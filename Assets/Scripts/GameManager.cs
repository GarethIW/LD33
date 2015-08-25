using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public Kaiju Kaiju;
    public Camera UiCamera;

    public float DeadTime = 5f;

    public int score;
    public int DamageCost;
    public int Civilians;
    public int Military;
    public static GameManager Instance;

	// Use this for initialization
	void Start () {
        Instance = this;

    }
	
	// Update is called once per frame
	void Update () {

	    if (Kaiju.Dead)
	    {
	        DeadTime -= Time.deltaTime;
	        if (DeadTime <= 0)
	        {
	            UiCamera.GetComponent<OLDTVScreen>().staticMagnetude =
	                Mathf.Lerp(UiCamera.GetComponent<OLDTVScreen>().staticMagnetude, 1f, Time.deltaTime);
                UiCamera.GetComponent<OLDTVScreen>().noiseMagnetude =
                    Mathf.Lerp(UiCamera.GetComponent<OLDTVScreen>().noiseMagnetude, 1f, Time.deltaTime);
            }
	    }
          
    }

   
}
