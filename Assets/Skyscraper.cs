using UnityEngine;
using System.Collections;

public class Skyscraper : MonoBehaviour
{
    public GameObject SectionPrefab;

    public int Height;
    public Material BaseMaterial;
    public Material MidMaterial;
    public Material[] TopMaterial;
    public Material BillBoardMaterial;
    public GameObject BillBoardBoard;
    public Material[] BillBoardLogoMaterial;
   
    public bool hasBillBoard = false;

    // Use this for initialization
    void Start ()
	{
	    Height = Random.Range(5, 10);
        float tint = Random.Range(0.7f, 1.2f);


        Color tintColour = new Color(tint, tint, tint);
      

	    float y = 0.193f; // half the section height
	    for (int i = 0; i < Height; i++)
	    {
            //Debug.Log("Start() Building Building");
	        var section = Instantiate(SectionPrefab);
            section.transform.SetParent(transform, false);
            section.transform.localPosition = new Vector3(0f,y,0f);
            Renderer renderer = section.GetComponent<Renderer>();
          
            if (i == 0)
            {
                renderer.sharedMaterial= BaseMaterial;
            }
            
            else if (i==Height-1)
            {


                int seed = Random.Range(0, TopMaterial.Length);

                Material mat = TopMaterial[seed];

                hasBillBoard=mat.name.Equals("ok-top2");
                section.GetComponent<SkyscraperSection>().hasBillBoard=hasBillBoard;

                renderer.sharedMaterial =mat;

                if (hasBillBoard)
                {
                    GameObject billBoard = Instantiate(BillBoardBoard);
                    billBoard.transform.SetParent(section.transform, false);
                    int billSeed = Random.Range(0, BillBoardLogoMaterial.Length);

                  mat = BillBoardLogoMaterial[billSeed];
                     renderer = billBoard.GetComponent<Renderer>();
                    renderer.sharedMaterial = mat;

                }

            }
            else
            {
                renderer.sharedMaterial = MidMaterial;
            }




            renderer.material.SetColor("_Color", tintColour);


	        y += 0.386f; // Scale of section


	        //if (i >= Height - 3)
	        //{
         //       // Get rid of the climb trigger on the top section
	        //    section.transform.GetChild(0).gameObject.SetActive(false);
	        //}
	    }
	}
	
	// Update is called once per frame
	void Update ()
	{
       
	}
}
