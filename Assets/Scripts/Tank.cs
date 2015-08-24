using UnityEngine;
using System.Collections;
using System;

public class Tank : Enemy {

    protected override void Fire()
    {
        Debug.Log("See my awesome firepower mortals !!");
    }

    protected override void SetUpAudio()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {

            if (source.clip.name.Equals("chicken"))
            {
                moveAudio = source;
            }

        }
    }
}
