using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{

    public GameObject effect;
    public AudioClip BulletSound;
    public GameObject BulletHole;

    public void Shot(RaycastHit hit,bool hole)
    {
        Instantiate(effect, GameObject.FindGameObjectWithTag("PistolNose").transform);
        GameObject a = new GameObject();
        a.AddComponent<AudioSource>().clip = BulletSound;
        a.GetComponent<AudioSource>().Play();
        a.AddComponent<AutoRemove>();
        if (hole)
            Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal));

       

        


    }
}
