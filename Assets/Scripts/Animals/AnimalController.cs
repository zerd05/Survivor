using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AnimalController : MonoBehaviour
{




    public int Health = 80;
    public int deadHealth = 300;
    public LayerMask layerAfterDeath;

    private Transform target;

    public Animator animator;


    public float wanderRadius = 7f;
    public float wanderSpeed = 4f;
    public float runSpeed = 2f;

    


    [Header("Sounds")]
    public AudioClip[] hitAudioClips;

    private NavMeshAgent agent;
    private Vector3 wanderPoint;

    private float timeToChange;
    private bool isRunAway;


    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        wanderPoint = RandomWanderPoint();
        timeToChange = Time.time;

    }


    public void MakeDamage(int damage)
    {
        if (Health <= 0)
        {
            
            return;
        }
            
        Health -= damage;
        animator.SetTrigger("Hit");

        //int soundNumber = Random.Range(0, hitAudioClips.Length);

        //SoundSysyem sounds = new SoundSysyem();

        //sounds.PlaySound(hitAudioClips[soundNumber], transform.position);

        //agent.enabled = false;
        isRunAway = true;
        StartCoroutine(StopRunAway());
        if (Health <= 0)//Смерть живтоного
        {
            gameObject.layer = Convert.ToInt32(Mathf.Log(layerAfterDeath,2));
            //gameObject.layer = layerAfterDeath;
            animator.SetTrigger("Dead");
            if (GetComponent<BoxCollider>() != null)
            {
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<CapsuleCollider>().enabled = false;
            }
            return;
        }
    }

    private IEnumerator StopRunAway()
    {
        yield return new WaitForSeconds(15f);
        isRunAway = false;


    }


    void Update()
    {

        if(deadHealth<=0)
            Destroy(gameObject);





        if(Health>0)

        {
            animator.SetInteger("Health", Health);

            if (Vector3.Distance(target.transform.position, transform.position) < 6.4f || isRunAway)
            {
                animator.SetBool("Run", true);
                agent.speed = runSpeed;
                Vector3 dirToPlayer = transform.position - target.position;
                Vector3 newPos = transform.position + dirToPlayer;
                agent.SetDestination(newPos);
            }
            else
            {
                animator.SetBool("Run", false);
                Wander();
            }
        }

         if (Health <= 0)
         {
             agent.enabled = false;
             
             
            
         }
    }






    public void Wander()
    {
        float currentTime = Time.time;
        agent.speed = wanderSpeed;

        if (Vector3.Distance(transform.position, wanderPoint) < 10f)
        {
            wanderPoint = RandomWanderPoint();
        }
        else if (timeToChange < currentTime - 5f)
        {
            timeToChange = Time.time;
            wanderPoint = RandomWanderPoint();

        }
        else
        {
            agent.SetDestination(wanderPoint);
        }


    }



    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }

}
