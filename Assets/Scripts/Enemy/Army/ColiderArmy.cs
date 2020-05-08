using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderArmy : MonoBehaviour
{
    public ArmyController armyController;
    public void MakeDamage(int damage)
    {
        armyController.deadHealth -= damage;
    }
}
