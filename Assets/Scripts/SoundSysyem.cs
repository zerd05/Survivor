using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSysyem : MonoBehaviour
{


    public void PlaySound(AudioClip sound,Vector3 position)
    {
        GameObject soundObject = new GameObject("SoundObject");
        soundObject.transform.position = position;
        soundObject.AddComponent<AudioSource>().clip = sound;
        soundObject.GetComponent<AudioSource>().playOnAwake = false;
        soundObject.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
        soundObject.GetComponent<AudioSource>().minDistance = 1;
        soundObject.GetComponent<AudioSource>().maxDistance = 40;
        soundObject.GetComponent<AudioSource>().spatialBlend = 1f;
        soundObject.GetComponent<AudioSource>().Play();
        soundObject.AddComponent<AutoRemove>();



    }

    public void PlaySound2D(AudioClip sound, Vector3 position)
    {
        GameObject soundObject = new GameObject("SoundObject");
        soundObject.transform.position = position;
        soundObject.AddComponent<AudioSource>().clip = sound;
        soundObject.GetComponent<AudioSource>().Play();
        soundObject.AddComponent<AutoRemove>();
    }
}
