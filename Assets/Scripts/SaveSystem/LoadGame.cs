using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    private Transform target;
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        if (LoadInfo.isLoadGame)
        {
            PlayerData playerData =  SaveSystem.LoadPlayer();
            Vector3 position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
            
            PlayerMove playerMove = target.GetComponent<PlayerMove>();
            playerMove.controller.enabled = false;
            playerMove.loadPosition = position;
            playerMove.controller.enabled = true;
            playerMove.hp = playerData.health;
            playerMove.eat = playerData.eat;
            playerMove.woodCount = playerData.wood;
            playerMove.rockCount = playerData.rock;
            playerMove.bulletCount = playerData.bullets;
            if (playerData.havePistol)
            {
                target.GetComponent<TakeWeapons>().TakePistol();
            }
            if (playerData.haveKnife)
            {
                target.GetComponent<TakeWeapons>().TakeKnife();
            }
            print(position);
        }
    }
}
