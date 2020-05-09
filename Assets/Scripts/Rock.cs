using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public int Hp = 500;

    void Update()
    {
        if (Hp <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

}
