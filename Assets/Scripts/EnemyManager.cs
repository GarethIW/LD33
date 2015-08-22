using UnityEngine;
using System.Collections;
using System;

public class EnemyManager : ObjectPool
{
    public static EnemyManager Instance;
    private CityManager theCity;
    public float SpawnOffset=20f;
    private float cityWidth;

    private Vector3 cityBoundary;
    // Use this for initialization
    public override void Start()
    {

        Instance = this;

        theCity = GameObject.FindGameObjectWithTag("City").GetComponent<CityManager>();
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

        cityBoundary.x = x * theCity.getWidth() * 2;

        cityBoundary.z = z * theCity.getWidth() * 2;

        StartCoroutine("Spawn");

        base.Start();
    }


    internal override void InitInstance(GameObject instance)
    {

        instance.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Kaiju kaiju = player.GetComponent<Kaiju>();

        Vector3 spawnPoint = Vector3.one;

        if (getSpawnPosition(player, ref spawnPoint)) ;


        instance.transform.position = spawnPoint;

        //instance.SetActive(true);

        base.InitInstance(instance);
    }

    private bool getSpawnPosition(GameObject player, ref Vector3 spawnPosition)
    {

        bool result = false;
        float z = player.transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f);

        if (z < 0)
        {
            z = 1;
        }

        Vector3 spawnPoint = new Vector3(0f, 0f, z);


        if (player.transform.position.x + SpawnOffset < cityBoundary.x)
        {
            spawnPoint.x = player.transform.position.x + SpawnOffset;
            result = true;

        }
        else if (player.transform.position.x - SpawnOffset < cityBoundary.x)
        {
            spawnPoint.x = player.transform.position.x - SpawnOffset;
            result = true;
        }
        else
        {
            spawnPoint = Vector3.one;
            result = false;
        }

        return result;

    }


    IEnumerator Spawn()
    {
        GameObject instance=GetOne("Man");

        if (instance != null)
        {
            instance.SetActive(true);
        }

        yield return new WaitForSeconds(5f);

        StartCoroutine("Spawn");
       
    }


}
