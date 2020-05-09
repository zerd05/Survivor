using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float eat;
    public int bullets;
    public int wood;
    public int rock;
    public bool havePistol;
    public bool haveKnife;
    public float[] position;
    public PlayerData(PlayerMove playerMove)
    {
        playerMove.UpdateItemSave();
        health = playerMove.hp;
        eat = playerMove.eat;
        bullets = playerMove.bulletCount;
        wood = playerMove.woodCount;
        rock = playerMove.rockCount;
        havePistol = playerMove.havePistol;
        haveKnife = playerMove.haveKnife;
        position = new float[3];
        position[0] = playerMove.transform.position.x;
        position[1] = playerMove.transform.position.y;
        position[2] = playerMove.transform.position.z;
    }


}
