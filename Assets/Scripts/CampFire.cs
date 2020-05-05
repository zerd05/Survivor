using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = PlayerManager.instance.player.transform;
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
