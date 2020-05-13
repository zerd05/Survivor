using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ArmyController : MonoBehaviour
{

    public AudioClip shootSound;
    public int shootDamage;
    public GameObject shootEffect;
    public GameObject shootEffectPlace;
    public GameObject pistolTip;
    public LayerMask armyLootLayer;
    public int deadHealth = 300;

    public GameObject pistolLoot;
    public int hitChance = 100;

    public int Health = 23123;


    private float lookRadius = 11f;

    private Transform target;

    public Animator animator;

    public float stoppingDistance = 5;
    public float wanderRadius = 7f;
    public float wanderSpeed = 4f;
    public float findPlayerSpeed = 2f;

    private NavMeshAgent agent;
    private Vector3 wanderPoint;

    private float nextTimeToHit;

    public float standartLookRadius;
    [Header("Sounds")]
    public AudioClip hitAudioClip;




    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        wanderPoint = RandomWanderPoint();
        prevDistance = transform.position;
        switch (PlayerPrefs.GetInt("Difficulty", 2))
        {
            case 1:
            {
                Health = 70;
                shootDamage = 15;
                hitChance = 50;
                break;
            }
            case 2:
            {
                Health = 100;
                shootDamage = 25;
                hitChance = 70;
                break;
            }
            case 3:
            {
                Health = 120;
                shootDamage = 40;
                hitChance = 90;
                break;
            }
        }

        timeToDelete = Time.time;
        if (!agent.SetDestination(transform.position))
            Destroy(gameObject);

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public void PlaySound(AudioClip sound)
    {
        SoundSysyem soundSysyem = new SoundSysyem();
        soundSysyem.PlaySound(sound, transform.position);
    }

    public void MakeDamage(int damage)
    {
        Health -= damage;
        //animator.SetTrigger("Hit");
        lookRadius = standartLookRadius * 100;
        StartCoroutine(StopHuntPlayer());
        PlaySound(hitAudioClip);



        //int soundNumber = Random.Range(0, hitAudioClips.Length);

        SoundSysyem sounds = new SoundSysyem();

        //sounds.PlaySound(hitAudioClips[soundNumber], transform.position);
        animator.SetTrigger("Hit");
    }


    private IEnumerator StopHuntPlayer()
    {
        yield return new WaitForSeconds(50.0f);
        lookRadius = standartLookRadius;


    }

    private bool deadThings = false;
    private float timeToDelete = 0;
    private Vector3 prevDistance;
    void Update()
    {
      if(deadHealth<=0)
          Destroy(gameObject);
      

        float currentTime = Time.time;
      if (timeToDelete < currentTime - 15f)
      {
          if (Vector3.Distance(prevDistance, transform.position) < 1f)
          {
              if (Health > 0)
                  Destroy(gameObject);
             
            }
          else
          {
              timeToDelete = Time.time;
              prevDistance = transform.position;
          }

          



      }

        animator.SetBool("Run",true);
        if (Health <= 0)
        {
            
            if(!deadThings)

            {
                CharacterJoint[] a = GetComponentsInChildren<CharacterJoint>();
                foreach (CharacterJoint c in a)
                {
                    c.enableProjection = true;
                    c.enableCollision = true;
                    c.gameObject.AddComponent<ColiderArmy>().armyController = this;
                    c.gameObject.layer = Convert.ToInt32(Mathf.Log(armyLootLayer, 2));
                }

                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<Animator>().enabled = false;


                Destroy(GetComponent<Rigidbody>());
                GetComponent<CapsuleCollider>().enabled = false;
                Destroy(agent);
                Destroy(gameObject, 200f);
               
                deadThings = true;
                return;
            }
            return;

        }




        float distance = Vector3.Distance(target.position, transform.position);

      
            if (distance <= lookRadius)
            {
                if(agent.enabled)
                {
                    lookRadius = standartLookRadius * 100;
                    StartCoroutine(StopHuntPlayer());
                    agent.speed = findPlayerSpeed;
                    //animator.SetBool("Walk", false);
                    //animator.SetBool("Run", true);
                    agent.SetDestination(target.position);
                }

                RaycastHit ray;
                Physics.Raycast(pistolTip.transform.position, target.position- pistolTip.transform.position, out ray, 400f);
                

                if (distance <= stoppingDistance + 0.4)
                {
                    if(ray.transform!=null)
                        if (ray.transform.tag == "Player")
                        {
                            FaceTarget();
                            animator.SetBool("Shooting", true);
                            if (agent.enabled)
                                agent.SetDestination(transform.position);
                        }
                        else
                        {
                            animator.SetBool("Shooting", false);
                            if (agent.enabled)
                                agent.SetDestination(target.position);
                        }
                    


                }
                else
                {
                    FaceTarget();
                    animator.SetBool("Shooting", false);
                    agent.enabled = true;
                    agent.SetDestination(target.position);
                }
            }
            else
            {
                // agent.SetDestination(gameObject.transform.position);
                //animator.SetBool("Run", false);
                agent.speed = wanderSpeed;
                //animator.SetBool("Walk", true);
                Wander();
                // agent.speed = wanderSpeed;
            }
            float foobar = target.GetComponent<AiSpawner>().maxDistance;
            if (Vector3.Distance(transform.position, target.position) > foobar)
            {
                Destroy(gameObject);

            }
    }



    void Attack()
    {

            if(Random.Range(1, 100)<hitChance)
                target.GetComponent<PlayerMove>().TakeDamage(shootDamage);
            SoundSysyem a = new SoundSysyem();
            a.PlaySound(shootSound,transform.position);
            Instantiate(shootEffect, shootEffectPlace.transform).GetComponent<AutoRemove>().lifeTime = 0.2f;
        //target.GetComponent<PlayerMove>().Kick(transform.forward * 150f + new Vector3(0, 150, 0));


    }




    public void Wander()
    {


        if (Vector3.Distance(transform.position, wanderPoint) < 20f)
        {
            wanderPoint = RandomWanderPoint();
        }
        else
        {
           
                agent.SetDestination(wanderPoint);
                
       
            
        }


    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        //transform.rotation = lookRotation;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, lookRadius = standartLookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }

}
