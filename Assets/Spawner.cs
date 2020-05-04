using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pref;


    public float КакЧастоСпавнить;
    private float КогдаВСледующийРазЗаспавнить;

    void Update()
    {

        if (Time.time >= КогдаВСледующийРазЗаспавнить)
        {
            КогдаВСледующийРазЗаспавнить = Time.time+1f/КакЧастоСпавнить;

            Vector3 a = new Vector3();
            a.z += Random.Range(-30, 30);
            a.x += Random.Range(-30, 30);
            Instantiate(pref, transform.position + a, Quaternion.identity);


        }








    }



}
