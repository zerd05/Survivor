using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    private Transform player;
    public AudioClip startSound;

    void Start()
    {

        SoundSysyem soundSysyem = new SoundSysyem();
        soundSysyem.PlaySound(startSound,transform.position);

        player = PlayerManager.instance.player.transform;


        transform.GetChild(0).rotation = Quaternion.LookRotation(Vector3.right); // Направление огня от костра вверх
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.position) < 3f)
        {
            player.GetComponent<PlayerMove>().CampFireActive(true);
        }
        else
        {
            player.GetComponent<PlayerMove>().CampFireActive(false);
        }

    }
}
