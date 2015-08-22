using System;
using UnityEngine;
using System.Collections;
using System.Linq;


public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public class PoolPrefab
    {
        public GameObject Prefab;
        public int Amount;
    }

    public PoolPrefab[] Prefabs;  

	// Use this for initialization
	public virtual void Start () {
	    foreach (PoolPrefab p in Prefabs)
	    {
	        GameObject instants = new GameObject(p.Prefab.name);
            instants.transform.SetParent(transform);

	        for (int i = 0; i < p.Amount; i++)
	        {
	            GameObject newInstant = (GameObject)Instantiate(p.Prefab, transform.position, Quaternion.identity);
	            newInstant.name = p.Prefab.name + " - " + i;
                InitInstance(newInstant);
                newInstant.transform.SetParent(instants.transform);
	        }
	    }
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
	    

	}

    public virtual GameObject GetOne(string prefabName)
    {
        var instants = transform.FindChild(prefabName);
        if (instants == null) return null;

        for (int i = 0; i < instants.childCount; i++)
        {
            if (!instants.GetChild(i).gameObject.activeSelf) return instants.GetChild(i).gameObject;
        }

        return null;
    }

    internal virtual void InitInstance(GameObject instance)
    {
        
    }
}
