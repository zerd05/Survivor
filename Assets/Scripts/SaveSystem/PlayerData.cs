using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float eat;
    public float water;
    public int bullets;
    public int wood;
    public int rock;
    public bool havePistol;
    public bool haveKnife;
    public float[] position;
    public float[] rotation;
    public PlayerData(PlayerMove playerMove)
    {
        playerMove.UpdateItemSave();
        health = playerMove.hp;
        eat = playerMove.eat;
        water = playerMove.water;
        bullets = playerMove.bulletCount;
        wood = playerMove.woodCount;
        rock = playerMove.rockCount;
        havePistol = playerMove.havePistol;
        haveKnife = playerMove.haveKnife;
        position = new float[3];
        position[0] = playerMove.transform.position.x;
        position[1] = playerMove.transform.position.y;
        position[2] = playerMove.transform.position.z;
        rotation = new float[4];
        rotation[0] = playerMove.transform.rotation.x;
        rotation[1] = playerMove.transform.rotation.y;
        rotation[2] = playerMove.transform.rotation.z;
        rotation[3] = playerMove.transform.rotation.w;
    }


}
