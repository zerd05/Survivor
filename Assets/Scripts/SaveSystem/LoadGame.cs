using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject pigPrefab;
    public GameObject cowPrefab;
    public GameObject chickenPrefab;
    public GameObject armyPrefab;
    public GameObject enemyPrefab;
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
                player.GetComponentInChildren<PistolShoot>().bullets= playerData.bulletsInPistol;
            }
            if (playerData.haveKnife)
            {
                player.GetComponent<TakeWeapons>().TakeKnife();
            }

            LoadInfo.isAlive = true;

            if (SaveSystem.LoadAi() != null)
            {
                List<AIData> aiData = SaveSystem.LoadAi();
                foreach (var current in aiData)
                {

                    switch (current.type)
                    {
                        case "Enemy":
                        {
                            Instantiate(enemyPrefab, new Vector3(current.position[0],current.position[1],current.position[2]), Quaternion.identity);
                            break;
                        }
                        case "Cow":
                        {
                            Instantiate(cowPrefab, new Vector3(current.position[0], current.position[1], current.position[2]), Quaternion.identity);

                                break;
                        }
                        case "Chicken":
                        {
                            Instantiate(chickenPrefab, new Vector3(current.position[0], current.position[1], current.position[2]), Quaternion.identity);
                                break;
                        }
                        case "Pig":
                        {
                            Instantiate(pigPrefab, new Vector3(current.position[0], current.position[1], current.position[2]), Quaternion.identity);
                                break;
                        }
                        case "Army":
                        {
                            Instantiate(armyPrefab, new Vector3(current.position[0], current.position[1], current.position[2]), Quaternion.identity);
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            GameObject[] respawns = GameObject.FindGameObjectsWithTag("Respawn");

            int selectedRespawn = Random.Range(0, respawns.Length);
            Vector3 position = respawns[selectedRespawn].transform.position;
            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            playerMove.hp = 100;
            playerMove.eat = Random.Range(80,100);
            playerMove.water = Random.Range(80, 100);
            LoadInfo.isAlive = true;
        }
    }
}
