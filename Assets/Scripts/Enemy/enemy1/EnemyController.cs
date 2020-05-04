using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{


    public int Health = 23123;


    private float lookRadius = 11f;

    private Transform target;

    public Animator animator;


    public float wanderRadius = 7f;
    public float wanderSpeed = 4f;
    public float findPlayerSpeed = 2f;

    private NavMeshAgent agent;
    private Vector3 wanderPoint;
    // Start is called before the first frame update

    private float nextTimeToHit;
    public float hitRate = 0.34f;
    
    public float standartLookRadius;

    [Header("Sounds")]
    public AudioClip[] hitAudioClips;




    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        wanderPoint = RandomWanderPoint();

    }


    public void MakeDamage(int damage)
    {
        Health -= damage;
        animator.SetTrigger("Hit");
        lookRadius = standartLookRadius * 100;
        StartCoroutine(StopHuntPlayer());
        
        


        int soundNumber =  Random.Range(0, hitAudioClips.Length);

        SoundSysyem sounds = new SoundSysyem();

        sounds.PlaySound(hitAudioClips[soundNumber],transform.position);

        agent.enabled = false;
        StartCoroutine(StopWalk());
    }

    private IEnumerator StopWalk()
    {
        yield return new WaitForSeconds(0.2f);
        agent.enabled = true;


    }

    private IEnumerator StopHuntPlayer()
    {
        yield return new WaitForSeconds(50.0f);
        lookRadius = standartLookRadius;
        

    }

    void Update()
    {

        animator.SetInteger("Health",Health);

        if (Health <= 0)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Run", false);

            int r =  Random.Range(1, 3);
            print(r);
            switch (r)
            {
                case 1:
                {
                    animator.SetTrigger("Dead1");
                    break;
                }
                   
                case 2:
                {

                    animator.SetTrigger("Dead2");
                    break;

                }

            }
            
            enabled = false;
            Destroy(GetComponent<Rigidbody>());
            GetComponent<CapsuleCollider>().enabled = false;
            return;

        }




        float distance = Vector3.Distance(target.position, transform.position);

        if(agent.enabled)
            if (distance <= lookRadius)
            {
                lookRadius = standartLookRadius * 100;
                StartCoroutine(StopHuntPlayer());
                agent.speed = findPlayerSpeed;
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance+0.4)
                {
                    FaceTarget();
                    animator.SetBool("Attack", true);

                    //if (Time.time >= nextTimeToHit)
                    //{
                    //    nextTimeToHit = Time.time + 1f / hitRate;
                       
                    //}
                    
                    //
                }
                else
                {
                    animator.SetBool("Attack", false);
                }
            }
            else
            {
               // agent.SetDestination(gameObject.transform.position);
                animator.SetBool("Run",false);
                agent.speed = wanderSpeed;
                animator.SetBool("Walk", true);
                Wander();
               // agent.speed = wanderSpeed;
            }
    }

    void Attack()
    {
        if (Vector3.Distance(target.position, transform.position) < 2f)
        {
            target.GetComponent<PlayerMove>().TakeDamage(20);
            //target.GetComponent<PlayerMove>().Kick(transform.forward * 150f + new Vector3(0, 150, 0));
        }
        
    }


  

    public void Wander()
    {

     
            if (Vector3.Distance(transform.position, wanderPoint) < 10f)
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
    }

    public Vector3 RandomWanderPoint()
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }



}
