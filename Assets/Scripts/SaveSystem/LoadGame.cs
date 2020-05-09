using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{

    public GameObject playerPrefab;
    void Start()
    {
        //target = PlayerManager.instance.player.transform;
        if (LoadInfo.isLoadGame)
        {
            PlayerData playerData =  SaveSystem.LoadPlayer();
            
            Vector3 position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
            Quaternion rotation = new Quaternion(playerData.rotation[0], playerData.rotation[1], playerData.rotation[2], playerData.rotation[3]);
            GameObject player =  Instantiate(playerPrefab, position, rotation);
            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            playerMove.controller.enabled = false;
            playerMove.loadPosition = position;
            playerMove.controller.enabled = true;
            playerMove.hp = playerData.health;
            playerMove.eat = playerData.eat;
            playerMove.woodCount = playerData.wood;
            playerMove.rockCount = playerData.rock;
            playerMove.bulletCount = playerData.bullets;
            playerMove.water = playerData.water;
            if (playerData.havePistol)
            {
                player.GetComponent<TakeWeapons>().TakePistol();
            }
            if (playerData.haveKnife)
            {
                player.GetComponent<TakeWeapons>().TakeKnife();
            }
            print(position);
        }
        else
        {
            
        }
    }
}
