using UnityEngine;
using System.Collections;
using Spine;

public class CityManager : MonoBehaviour {

    public  int CityWidth=10;
    public  int CityDepth = 10;
    private int ColumnOffset=2;
    public GameObject RoadPrefab;
    public int Seed=4;
    public GameObject[] Features;
    private bool justPlacedRoad=false;
    private GameObject roadsObject;
    private int currentRow;
    private bool justPlacedScraper = false;

	// Use this for initialization
	void Start ()
    {

        roadsObject = new GameObject("Roads");

        roadsObject.transform.SetParent(transform);
	    roadsObject.transform.localPosition = Vector3.zero;

        createBlock();

        for(int x=-10;x<CityWidth+10;x++)
            for (int y = -5; y < (CityDepth * 2) + 5; y++)
            {
                if (x < 0 || x > CityWidth || y < 0 || y > CityDepth * 2f)
                {
                    InsertInstance(0f, (y%2==0 && y>=0 && y<= (CityDepth * 2)+2) ? RoadPrefab:Features[0], gameObject, new Vector3(x * 2f, 0f, y * 2f));
                }
            }

        // Put some civvies down
	    for (int i = 0; i < 300; i++)
	    {
	        Vector3 pos = new Vector3(Random.Range(1f,getCityBoundryWidth()-1f), 0f, Random.Range(1f,CityDepth*4f));
            var c = EnemyManager.Instance.GetOne("Citizen");
            if (c != null)
            {
                c.transform.position = pos;
                c.GetComponent<Citizen>().Init();
                c.SetActive(true);
            }
        }
    }

    public int getWidth()
    {
        return CityWidth;
    }

    public int getDepth()
    {
        return CityDepth;
    }

    public float getCityBoundryWidth()
    {

      float  w= transform.position.x + CityWidth * 2;



        return w;


       
    }

    public void createBlock()
    {
        for (int r = 0; r <= CityDepth; r++)
        {
            CreateRow(ColumnOffset, RoadPrefab, roadsObject, currentRow, CityWidth, false);
            currentRow += 2;
            CreateRow(ColumnOffset, RoadPrefab, roadsObject, currentRow, CityWidth, true);
            currentRow += 2;
        }
    }

    public void CreateRow(float offset,GameObject prefab,GameObject parent,float row,float tileMax,bool randomise)
    {
        Vector3 insertionPosition = new Vector3(transform.position.x, 0f, row);
        GameObject instance;
        for (int i = 0; i <= tileMax; i++)
        {

           
            if (randomise)
            {
                int r = Random.Range(0, 10);

                if (r >= Seed||justPlacedRoad)
                {

                    int f = Random.Range(0, Features.Length);
                    //if (f == 1 && justPlacedScraper)
                    //{
                    //    f = 0;
                    //    justPlacedScraper = false;
                    //}
                    //if (f == 1) justPlacedScraper = true;
                    instance = InsertInstance(offset, Features[f], parent, insertionPosition);
                    justPlacedRoad = false;
                }
                else
                {

                    instance = InsertInstance(offset, prefab, parent, insertionPosition);
                    justPlacedRoad = true;

                }
            }
            else
            {
                instance = InsertInstance(offset, prefab, parent, insertionPosition);
               
            }

            insertionPosition.x += offset;
        }

        // put some cars down
        if (!randomise)
        {
            float y = (row) + 0.25f;
            for (int i = 0; i < 5; i++)
            {
                float x = Random.Range(2f, getCityBoundryWidth() - 2f);
                var c = EnemyManager.Instance.GetOne(Random.Range(0, 2) == 0 ? "Car" : "SmallCar");
                if (c != null)
                {
                    c.transform.position = new Vector3(x,0f,y);
                    c.SetActive(true);
                }
            }

            y= (row) + 1.75f;
            for (int i = 0; i < 5; i++)
            {
                float x = Random.Range(2f, getCityBoundryWidth() - 2f);
                var c = EnemyManager.Instance.GetOne(Random.Range(0, 2) == 0 ? "Car" : "SmallCar");
                if (c != null)
                {
                    c.transform.position = new Vector3(x, 0f, y);
                    c.SetActive(true);
                }
            }
        }
    }

    private static GameObject InsertInstance(float offset, GameObject prefab, GameObject parent, Vector3 insertionPosition)
    {
        GameObject instance = (GameObject)Instantiate(prefab, insertionPosition, Quaternion.Euler(0, 0, 0));
       
        instance.transform.SetParent(parent.transform);
        instance.transform.localPosition = insertionPosition;
        return instance;
    }

    
}
