
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{

    public static void SavePlayer(PlayerMove playerMove)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path,FileMode.Create);

        PlayerData data = new PlayerData(playerMove);

        formatter.Serialize(stream,data);
        stream.Close();

        List<AIData> AiList = new List<AIData>();

        foreach (var enemyGameObject in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            AIData current = new AIData();
            current.position = new float[3];
            current.position[0] = enemyGameObject.transform.position.x;
            current.position[1] = enemyGameObject.transform.position.y;
            current.position[2] = enemyGameObject.transform.position.z;
            current.type = "Enemy";
            AiList.Add(current);
        }
        foreach (var enemyGameObject in GameObject.FindGameObjectsWithTag("Army"))
        {
            AIData current = new AIData();
            current.position = new float[3];
            current.position[0] = enemyGameObject.transform.position.x;
            current.position[1] = enemyGameObject.transform.position.y;
            current.position[2] = enemyGameObject.transform.position.z;
            current.type = "Army";
            AiList.Add(current);
        }
        foreach (var enemyGameObject in GameObject.FindGameObjectsWithTag("Chicken"))
        {
            AIData current = new AIData();
            current.position = new float[3];
            current.position[0] = enemyGameObject.transform.position.x;
            current.position[1] = enemyGameObject.transform.position.y;
            current.position[2] = enemyGameObject.transform.position.z;
            current.type = "Chicken";
            AiList.Add(current);
        }
        foreach (var enemyGameObject in GameObject.FindGameObjectsWithTag("Cow"))
        {
            AIData current = new AIData();
            current.position = new float[3];
            current.position[0] = enemyGameObject.transform.position.x;
            current.position[1] = enemyGameObject.transform.position.y;
            current.position[2] = enemyGameObject.transform.position.z;
            current.type = "Cow";
            AiList.Add(current);
        }
        foreach (var enemyGameObject in GameObject.FindGameObjectsWithTag("Pig"))
        {
            AIData current = new AIData();
            current.position = new float[3];
            current.position[0] = enemyGameObject.transform.position.x;
            current.position[1] = enemyGameObject.transform.position.y;
            current.position[2] = enemyGameObject.transform.position.z;
            current.type = "Pig";
            AiList.Add(current);
        }

        formatter = new BinaryFormatter();

        path = Application.persistentDataPath + "/Ai.save";
        stream = new FileStream(path, FileMode.Create);


        formatter.Serialize(stream, AiList);
        stream.Close();

    }


    public static List<AIData> LoadAi()
    {
        string path = Application.persistentDataPath + "/Ai.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<AIData> data = formatter.Deserialize(stream) as List<AIData>;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Нет файла Ai сохранения");
            return null;
        }
    }


    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Нет файла");
            return null;
        }
    }

}
