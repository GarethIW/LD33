using UnityEngine;
using System.Collections;
using System;

public class EnemyManager : ObjectPool
{
    public static EnemyManager Instance;
    private CityManager theCity;
    public float SpawnOffset = 10f;
    private float cityWidth;
    private Kaiju kaiju;
    GameObject player;
    private Vector3 cityBoundary;
    // Use this for initialization
    public override void Start()
    {

        Instance = this;

        theCity = GameObject.FindGameObjectWithTag("City").GetComponent<CityManager>();

        player = GameObject.FindGameObjectWithTag("Player");

        kaiju = player.GetComponent<Kaiju>();

        cityWidth = theCity.getWidth();

        float x = theCity.gameObject.transform.position.x;

        if (x < 1)
        {
            // just incase this is 0 and messes with the math.
            x = 1;
        }

        float z = theCity.gameObject.transform.position.z;

        if (z < 1)
        {
            // just incase this is 0 and messes with the math.
            z = 1;
        }


        //X of City * width in tiles * width of tiles.

        cityBoundary.x = x + theCity.getWidth() * 2;

        cityBoundary.z = z + theCity.getDepth() * 2;

        StartCoroutine("Spawn");

        base.Start();
    }


    internal override void InitInstance(GameObject instance)
    {

        instance.SetActive(false);

        Vector3 spawnPoint =getSpawnPosition();

        if (isValidSpawnPoint(spawnPoint,player))
        {


            instance.transform.position = spawnPoint;

        }
        base.InitInstance(instance);
    }



    private Vector3 getSpawnPosition()
    {

        float z = player.transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f);

        if (z < 0)
        {
            z = 1;
        }

        float x = 0f;

        float seed = UnityEngine.Random.Range(0f, 1f);
      
        if (seed > 0.5)
        {
            x = player.transform.position.x + (SpawnOffset+UnityEngine.Random.Range(0,5));
        }
        else
        {
            x = player.transform.position.x - (SpawnOffset+UnityEngine.Random.Range(0, 5));
        }

        if (x < 0) x = 0;

        Vector3 spawnPosition = new Vector3(x, 0f, z);


        return spawnPosition;
    }


    private bool isValidSpawnPoint(Vector3 spawnPoint,GameObject player)
    {
        bool result = false;

        float playerX = player.transform.position.x;

        //if ( ((playerX+SpawnOffset)<=spawnPoint.x)&& spawnPoint.x < cityBoundary.x && (((playerX - SpawnOffset) > spawnPoint.x) && spawnPoint.x > 0f))
        if (spawnPoint.x < cityBoundary.x && spawnPoint.x > 0f)
        {
            result = true;

        }
       
              
        

        return result;
    }



    
    IEnumerator Spawn()
    {
      
        Vector3 spawnPoint = getSpawnPosition();
        if (isValidSpawnPoint(spawnPoint, player))
        {

            GameObject instance;
            int i = UnityEngine.Random.Range(0, 10);

            if (i == 0)
            {
                instance = GetOne("Tank");
                if (CheckAroundFor(spawnPoint, 3f, "Tank"))
                {
                    instance = null;
                }
            }
            else if(i<4)
            {
                instance = GetOne("Man");
            }
            else
            {
                instance = GetOne("Citizen");
            }


           
            if (instance != null)
        {

                instance.transform.position = spawnPoint;
                instance.GetComponent<Enemy>().Init();
                instance.SetActive(true);
                //Debug.Log("Spawn() Spawning at x:" + instance.transform.position.x);
               // Debug.Log("Spawn() Spawning at z:" + instance.transform.position.z);
                //instance.SetActive(true);
            }

         
        }

        yield return new WaitForSeconds(0.1f);
        if (kaiju.Health>0) {

            //Debug.Log(" Spawn() Player Health" + kaiju.hp);
            StartCoroutine("Spawn");
        }
    }

    bool CheckAroundFor(Vector3 pos, float radius, string name)
    {
        bool found = false;
        var instants = transform.FindChild(name);
        if (instants == null) return false;

        for (int i = 0; i < instants.childCount; i++)
        {
            if (instants.GetChild(i).gameObject.activeSelf)
            {
                if (Vector3.Distance(pos, instants.GetChild(i).transform.position) <= radius)
                    found = true;
            }
        }

        return found;
    }

}
