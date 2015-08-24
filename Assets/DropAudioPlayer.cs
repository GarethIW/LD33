using UnityEngine;
using System.Collections;

public class DropAudioPlayer : MonoBehaviour {

    private AudioSource fallingDownAudio;
    private float lifeCounter=0f;

    // Use this for initialization
    void Start () {

        fallingDownAudio = GetComponent<AudioSource>();

       

    }
	
	// Update is called once per frame
	void Update () {

        lifeCounter += Time.deltaTime;

        if (lifeCounter > 2 && !fallingDownAudio.isPlaying)
        {
            lifeCounter = 0f;
            gameObject.SetActive(false);
        }
	}
}
