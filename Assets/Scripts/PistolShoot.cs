﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{

    public GameObject effect;
    public AudioClip BulletSound;
    public AudioClip clickSound;
    public AudioClip reloadSound;
    public GameObject BulletHole;


    public int maxBullets = 7;

    public int bullets = 7;



    public bool Shot(RaycastHit hit,bool hole)
    {
        if (bullets > 0)
        {
            bullets -= 1;
            Instantiate(effect, GameObject.FindGameObjectWithTag("PistolNose").transform);
            GameObject a = new GameObject();
            a.AddComponent<AudioSource>().clip = BulletSound;
            a.GetComponent<AudioSource>().Play();
            a.AddComponent<AutoRemove>();
            if (hole)
                Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal));

            return true;

        }

        return false;




    }

    public void Reload()
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            GetComponent<Animator>().SetTrigger("Reload");
            bullets = maxBullets;
            SoundSysyem c = new SoundSysyem();
            c.PlaySound2D(reloadSound, transform.position);
        }
            

    }

    public bool CanShot()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Reload"))
            return false;
            if (bullets > 0)
            return true;
        SoundSysyem c = new SoundSysyem();
        c.PlaySound2D(clickSound, transform.position);
        return false;
    }
}
