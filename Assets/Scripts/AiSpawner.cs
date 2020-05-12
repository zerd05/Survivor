using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSpawner : MonoBehaviour
{
    //private Transform target;
    public float minDistance;
    
    public float  maxDistance;
    public GameObject armyPrefab;
    public GameObject zombiePrefab;
    public GameObject pigPrefab;
    public GameObject cowPrefab;
    public GameObject chickenPrefab;
    public LayerMask groundMask;

    public int armyCount;
    public int zombieCount;
    public int pigCount;
    public int cowCount;
    public int chickenCount;
    void Start()
    {


     

        

        InvokeRepeating("SpawnEnemy",1f,0.5f);
        



    }

    public void SpawnEnemy()
    {

        Vector3 to = (Random.insideUnitSphere * maxDistance);
        if (GameObject.FindObjectsOfType<ArmyController>().Length < armyCount)
        {
            to = (Random.insideUnitSphere * maxDistance);
            string name = to.ToString();
            to += transform.position;
            to.y = transform.position.y;
            to = to - (transform.position + new Vector3(0, 1500));
            RaycastHit ray;
            Physics.Raycast(transform.position + new Vector3(0, 1500), to, out ray, 1900, groundMask);
            if (Vector3.Distance(transform.position, ray.point) < minDistance)
                return;

            Instantiate(armyPrefab, ray.point, Quaternion.identity);
           
           
        }
        if (GameObject.FindObjectsOfType<ArmyController>().Length < zombieCount)
        {
            to = (Random.insideUnitSphere * maxDistance);
            string name = to.ToString();
            to += transform.position;
            to.y = transform.position.y;
            to = to - (transform.position + new Vector3(0, 1500));
            RaycastHit ray;
            Physics.Raycast(transform.position + new Vector3(0, 1500), to, out ray, 1900, groundMask);
            if (Vector3.Distance(transform.position, ray.point) < minDistance)
                return;

            Instantiate(zombiePrefab, ray.point, Quaternion.identity);
            
        }
        if (GameObject.FindObjectsOfType<ArmyController>().Length < cowCount)
        {
            to = (Random.insideUnitSphere * maxDistance);
            string name = to.ToString();
            to += transform.position;
            to.y = transform.position.y;
            to = to - (transform.position + new Vector3(0, 1500));
            RaycastHit ray;
            Physics.Raycast(transform.position + new Vector3(0, 1500), to, out ray, 1900, groundMask);
            if (Vector3.Distance(transform.position, ray.point) < minDistance)
                return;

            Instantiate(cowPrefab, ray.point, Quaternion.identity);
           
        }
        if (GameObject.FindObjectsOfType<ArmyController>().Length < pigCount)
        {
            to = (Random.insideUnitSphere * maxDistance);
            string name = to.ToString();
            to += transform.position;
            to.y = transform.position.y;
            to = to - (transform.position + new Vector3(0, 1500));
            RaycastHit ray;
            Physics.Raycast(transform.position + new Vector3(0, 1500), to, out ray, 1900, groundMask);
            if (Vector3.Distance(transform.position, ray.point) < minDistance)
                return;

            Instantiate(pigPrefab, ray.point, Quaternion.identity);
          
        }
        if (GameObject.FindObjectsOfType<ArmyController>().Length < chickenCount)
        {
            to = (Random.insideUnitSphere * maxDistance);
            string name = to.ToString();
            to += transform.position;
            to.y = transform.position.y;
            to = to - (transform.position + new Vector3(0, 1500));
            RaycastHit ray;
            Physics.Raycast(transform.position + new Vector3(0, 1500), to, out ray, 1900, groundMask);
            if (Vector3.Distance(transform.position, ray.point) < minDistance)
                return;

            Instantiate(chickenPrefab, ray.point, Quaternion.identity);
           
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,  minDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,maxDistance );
    }
}
