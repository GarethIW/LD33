using UnityEngine;
using System.Collections;

public class CityManager : MonoBehaviour {

    public int CityWidth=10;
    public int CityDepth = 10;
    private int ColumnOffset=2;
    public GameObject RoadPrefab;
    public int Seed=4;
    public GameObject[] Features;
    private bool justPlacedRoad=false;
    private GameObject roadsObject;
    private int currentRow;

	// Use this for initialization
	void Start ()
    {

        roadsObject = new GameObject("Roads");

        roadsObject.transform.SetParent(transform);

        createBlock();

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
        Vector3 insertionPosition = new Vector3(0f, 0f, row);
        GameObject instance;
        for (int i = 0; i <= tileMax; i++)
        {

           
            if (randomise)
            {
                int r = Random.Range(0, 10);

                if (r >= Seed||justPlacedRoad)
                {

                    int f = Random.Range(0, Features.Length);
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
    }

    private static GameObject InsertInstance(float offset, GameObject prefab, GameObject parent, Vector3 insertionPosition)
    {
        GameObject instance = (GameObject)Instantiate(prefab, insertionPosition, Quaternion.Euler(90, 0, 0));
       
        instance.transform.SetParent(parent.transform);
        return instance;
    }

    
}
