using UnityEngine;
using System.Collections;

public class Skyscraper : MonoBehaviour
{
    public GameObject SectionPrefab;

    public int Height;

	// Use this for initialization
	void Start ()
	{
	    Height = Random.Range(5, 10);

	    float y = 0.25f;
	    for (int i = 0; i < Height; i++)
	    {
	        var section = Instantiate(SectionPrefab);
            section.transform.SetParent(transform, false);
            section.transform.localPosition = new Vector3(0f,y,0f);
	        y += 0.5f;
	        if (i >= Height - 3)
	        {
                // Get rid of the climb trigger on the top section
	            section.transform.GetChild(0).gameObject.SetActive(false);
	        }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
