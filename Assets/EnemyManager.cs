using UnityEngine;
using System.Collections;

public class EnemyManager : ObjectPool {
    public static EnemyManager Instance;
    // Use this for initialization
    public override void Start () {

        Instance = this;

        base.Start();
	}



	
	
}
