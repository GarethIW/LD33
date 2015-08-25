using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{

    public Kaiju Kaiju;
    public Camera UiCamera;
    public AudioMixerGroup Mixer;

    public float DeadTime = 8f;

    public int score;
    public int DamageCost;
    public int Civilians;
    public int Military;
    public static GameManager Instance;

    private float fadeInTime;
    private float musicTime;

    Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

    // Use this for initialization
    void Start () {
        Instance = this;

	    fadeInTime = 4f;
        musicTime = 9f;

        foreach (AudioSource source in GetComponents<AudioSource>())
        {
            audioSources.Add(source.clip.name, source);
        }

    }
	
	// Update is called once per frame
	void Update () {

	    if (!Kaiju.Dead)
	    {
	        if (fadeInTime > 0)
	        {
	            fadeInTime -= Time.deltaTime;
	        }
	        else
	        {
	            UiCamera.GetComponent<OLDTVScreen>().staticMagnetude =
	                Mathf.Lerp(UiCamera.GetComponent<OLDTVScreen>().staticMagnetude, 0.004f, Time.deltaTime);
	            UiCamera.GetComponent<OLDTVScreen>().noiseMagnetude =
	                Mathf.Lerp(UiCamera.GetComponent<OLDTVScreen>().noiseMagnetude, 0.05f, Time.deltaTime);

	            Mixer.audioMixer.TransitionToSnapshots(new[] {Mixer.audioMixer.FindSnapshot("Game")}, new[] {1f}, 1f);
	        }

	        if (musicTime > 0f)
	        {
	            musicTime -= Time.deltaTime;
	            if (musicTime <= 0f)
	            {
	                audioSources["MainTheme"].Play();
	                audioSources["DyingTheme"].Play();
	            }
	        }
	    }

	    if (Kaiju.Health < 200f)
	    {
            Mixer.audioMixer.TransitionToSnapshots(new[] { Mixer.audioMixer.FindSnapshot("Dying") }, new[] { 1f }, 0.5f);
        }

	    if (Kaiju.Dead)
	    {
	        DeadTime -= Time.deltaTime;
	        if (DeadTime <= 0)
	        {
	            UiCamera.GetComponent<OLDTVScreen>().staticMagnetude =
	                Mathf.Lerp(UiCamera.GetComponent<OLDTVScreen>().staticMagnetude, 1f, Time.deltaTime);
                UiCamera.GetComponent<OLDTVScreen>().noiseMagnetude =
                    Mathf.Lerp(UiCamera.GetComponent<OLDTVScreen>().noiseMagnetude, 1f, Time.deltaTime);

                Mixer.audioMixer.TransitionToSnapshots(new[] { Mixer.audioMixer.FindSnapshot("Intro") }, new[] { 1f }, 3f);
            }
	    }
          
    }

   
}
