using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    private Transform player;
    private PlayerMove playerMove;
    public GameObject effect;
    public AudioClip BulletSound;
    public AudioClip clickSound;
    public AudioClip reloadSound;
    public GameObject BulletHole;

    public int damage = 35;

    public int maxBullets = 7;

    public int bullets = 7;

    public GameObject pistolNose;


    void Start()
    {
        player = PlayerManager.instance.player.transform;
        playerMove = player.GetComponent<PlayerMove>();
    }


    public bool Shot(RaycastHit hit,bool hole)
    {
        if (bullets > 0)
        {
            bullets -= 1;
            Instantiate(effect, pistolNose.transform).GetComponent<AutoRemove>().lifeTime = 12f;
            GameObject a = new GameObject();
            a.AddComponent<AudioSource>().clip = BulletSound;
            a.GetComponent<AudioSource>().Play();
            a.AddComponent<AutoRemove>();
            if (hole)
                Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal),hit.transform);

            return true;

        }

        return false;




    }

    public bool FakeShoot()
    {
        if (bullets > 0)
        {
            bullets -= 1;
            Instantiate(effect, GameObject.FindGameObjectWithTag("PistolNose").transform).GetComponent<AutoRemove>().lifeTime = 0.2f;
            GameObject a = new GameObject();
            a.AddComponent<AudioSource>().clip = BulletSound;
            a.GetComponent<AudioSource>().Play();
            a.AddComponent<AutoRemove>();
          
            return true;

        }

        return false;
    }

    public void Reload()
    {
      if(bullets==maxBullets)
          return;
        if (playerMove.bulletCount>0)
            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Reload"))
            {
                GetComponent<Animator>().SetTrigger("Reload");
                if (playerMove.bulletCount > maxBullets)
                {

                    if(bullets == 0)
                    {
                        bullets = maxBullets;
                        playerMove.bulletCount -= maxBullets;
                    }
                    else
                    {
                        int vr = maxBullets - bullets;
                        bullets += vr;
                        playerMove.bulletCount -= vr;
                    }
                }
                else
                {
                    int vr = maxBullets - bullets;
                    if (playerMove.bulletCount > vr)
                    {
                        bullets = maxBullets;
                        playerMove.bulletCount -= vr;

                    }
                    else
                    {
                        bullets += playerMove.bulletCount;
                        playerMove.bulletCount -= playerMove.bulletCount;
                    }
                }
                    
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
