using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Kaiju Kaiju;

    public Text[] headlineText;
    public float HeadlineSpinTimeTarget;

    private float headlineSpinTime;

    private int nextHeadline = 0;

    private int usingHeadline = 0;

    private bool scrollingHeadline;

    private bool hasDefeated;

    string[] headlines = new string[5];

	// Use this for initialization
	void Start ()
	{
	    
        //headlineText[2].text = "248 MILITARY CASUALTIES";

        headlines[0] = "MONSTER ATTACKS CITY!";
        headlines[1] = "MONSTER SEEMS ANGRY";

        headlineText[0].text = headlines[0];
        headlineText[1].text = headlines[1];
    }
	
	// Update is called once per frame
	void Update ()
	{


	    if (!scrollingHeadline)
	    {
	        headlineSpinTime += Time.deltaTime;
	        if (headlineSpinTime >= HeadlineSpinTimeTarget)
	        {
	            headlineSpinTime = 0f;
	            scrollingHeadline = true;
	        }
	    }

	    if (scrollingHeadline)
	    {
	        headlineText[usingHeadline].rectTransform.localPosition = Vector3.Lerp(headlineText[usingHeadline].rectTransform.localPosition,
	            new Vector3(headlineText[usingHeadline].rectTransform.localPosition.x, 38,
	                headlineText[usingHeadline].rectTransform.localPosition.z), Time.deltaTime * 2f);
            headlineText[1-usingHeadline].rectTransform.localPosition = Vector3.Lerp(headlineText[1-usingHeadline].rectTransform.localPosition,
                new Vector3(headlineText[1-usingHeadline].rectTransform.localPosition.x, -2.7f,
                    headlineText[1-usingHeadline].rectTransform.localPosition.z), Time.deltaTime * 2f);

	        if (headlineText[1 - usingHeadline].rectTransform.localPosition.y >= -2.8f &&
	            headlineText[1 - usingHeadline].rectTransform.localPosition.y <= -2.6f)
	        {
	            scrollingHeadline = false;
                headlineText[usingHeadline].rectTransform.localPosition = new Vector3(headlineText[usingHeadline].rectTransform.localPosition.x, -38f, headlineText[usingHeadline].rectTransform.localPosition.z);


	            float kaijuHealthPercent = (100f/ Kaiju.BaseHealth) *Kaiju.Health;

	            headlines[1] = "MONSTER SEEMS ANGRY";
	            if (kaijuHealthPercent <= 80f) headlines[1] = "MILITARY INEFFECTUAL";
	            if (kaijuHealthPercent <= 60f) headlines[1] = "MONSTER HIT BUT UNSHAKEN";
	            if (kaijuHealthPercent <= 40f) headlines[1] = "MONSTER IS WOUNDED";
	            if (kaijuHealthPercent <= 20f) headlines[1] = "MILITARY FEELS CONFIDENT";
	            if (kaijuHealthPercent <= 10f) headlines[1] = "MONSTER LOOKS VISIBLY WEAK";
	            if (kaijuHealthPercent <= 0f)
	            {
	                headlines[1] = "MONSTER IS DEFEATED!";
	            }

                headlines[2] = string.Format("{0} CIVILIANS DEAD", GameManager.Instance.Civilians.ToString(CultureInfo.CurrentCulture.NumberFormat));
	            headlines[3] = string.Format("${0} OF DAMAGE", GameManager.Instance.DamageCost.ToString(CultureInfo.CurrentCulture.NumberFormat));
	            headlines[4] = string.Format("{0} MILITARY CASUALTIES", GameManager.Instance.Military.ToString(CultureInfo.CurrentCulture.NumberFormat));

	            if (nextHeadline == 0)
	            {
                    
                    if (GameManager.Instance.Civilians > 0) nextHeadline = 2;
                    else if (GameManager.Instance.DamageCost > 0) nextHeadline = 3;
                    else if (GameManager.Instance.Military > 0) nextHeadline = 4;
                    else nextHeadline = 1;
                }
                else if (nextHeadline == 1)
                {
                    if (GameManager.Instance.Civilians > 0) nextHeadline = 2;
                    else if (GameManager.Instance.DamageCost > 0) nextHeadline = 3;
                    else if (GameManager.Instance.Military > 0) nextHeadline = 4;
                    else nextHeadline = 0;
                }
                else if (nextHeadline >= 2)
                {
                    nextHeadline++;
                    if (nextHeadline == 5) nextHeadline = 1;

                    if (nextHeadline==2 && GameManager.Instance.Civilians == 0) nextHeadline = 3;
                    if (nextHeadline == 3 &&  GameManager.Instance.DamageCost == 0) nextHeadline = 4;
                    if (nextHeadline == 4 && GameManager.Instance.Military == 0) nextHeadline = 1;
                }

	            if (kaijuHealthPercent <= 0f)
	            {
	                if (!hasDefeated)
	                {
	                    hasDefeated = true;
	                    nextHeadline = 1;
	                }
	            }

	            headlineText[usingHeadline].text = headlines[nextHeadline];

                usingHeadline = 1 - usingHeadline;

            }
        }
	}
}
