﻿
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{

    public CharacterController controller;

    public float gravity = -9.8f;            //Сила гравитации
    public float speed = 12f;                //Скорость ходьбы
    public float speedSprint = 15f;          //Скорость бега
    public float jumpHeight = 3f;            //Выстора прыжка
                                             
    public Transform groundCheck;            //Проверка нахождения на земле
                                             
    public float groundDistance = 0.4f;      //Расстояние до земли
    public LayerMask groundMask;             //Слой земли
                                             
                                             
    Vector3 velocity;                        //
    bool isGrounded;                         //Находится ли персонаж на земле
                                             
    public Animator animator;                //
    public float meleeDistance = 4f;
                                             
    public Transform transformCamera;        //Камера персонажа
    public LayerMask woodMask;               //Слой дерева
    public LayerMask enemyMask;
    public LayerMask playerMask;
                                             
    public int hitDamage = 50;               //Урон от удара

    public float fallWithoutDamage = 10f;
    public bool enableFallDamage = true;     //Применять ли урон от падений
    public float fallDamageMultiplayer = 2.5f; // Множитель урона от падений 


    private int woodCount = 0;               //Количество дерева
    public int hp = 100;                     //Количество здоровья
                                             
    public Text woodText;                    //Информация о ресурсах
    public Text ammoText;
    public AudioClip woodHit;                //Звук удара топором по дереву


    private float startFall;
    private float endFall;
    private float fallHeight;


    public WeaponSwitch weaponSwitch;

    public GameObject bulletEffect;



    public Vector3 punch = new Vector3(0, 0);
    [Space]
    [Space]
    [Header("Debug")]
    public GameObject enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        woodText.text = "Дерево: " + woodCount.ToString()+"\nЗдоровье: "+hp.ToString();
        if (weaponSwitch.weapon.name == "Pistol")
        {
            ammoText.text = weaponSwitch.weapon.GetComponent<PistolShoot>().bullets+"/"+weaponSwitch.weapon.GetComponent<PistolShoot>()
                .maxBullets;
        }


            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (enableFallDamage)
        {
            if (!isGrounded)
            {
                if (startFall < transform.position.y)
                    startFall = transform.position.y;


            }
            if (isGrounded && startFall != 0f)
            {
                endFall = transform.position.y;

                fallHeight = Mathf.Abs(startFall) - Mathf.Abs(endFall);
                if(fallHeight>fallWithoutDamage)
                    hp -= Mathf.RoundToInt(Mathf.Abs(fallHeight * fallDamageMultiplayer));
                startFall = 0;

            }
        }
         

         

         if (hp <= 0) //смерть
         {
             hp = 0;
             GameObject a = GameObject.FindGameObjectWithTag("arms");
             a.SetActive(false);
            woodText.text = "Дерево: " + woodCount.ToString() + "\nЗдоровье: " + hp.ToString();
            woodText.text = "\n\n\n\n\nПерсонаж погиб";
            GetComponent<PlayerMove>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(500f,50f,500f));
            GetComponent<Rigidbody>().mass += 300;
            GetComponent<MouseLock>().enabled = false;

         }

        if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }

       
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(Input.GetAxis("Sprint")>0 && isGrounded)
        {
            animator.SetBool("Run", true);
            controller.Move(move * speedSprint * Time.deltaTime);
        }else
        {
            animator.SetBool("Run", false);
            controller.Move(move * speed * Time.deltaTime);
        }
        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }


        if(Input.GetButtonDown("Reload"))
        {
            if (weaponSwitch.weapon.name == "Pistol")
            {
                weaponSwitch.weapon.GetComponent<PistolShoot>().Reload();
            }
        }



        if (Input.GetButtonDown("Fire1"))
        {
            //print(weaponSwitch.weapon);
           RaycastHit hit;
           if (weaponSwitch.weapon.name == "Pistol")  //Выстрел с пистолета
           {
               if (weaponSwitch.weapon.GetComponent<PistolShoot>().CanShot())
               {
                   
                   //print("Можно стрелять");

                   animator.Play("Shot");


                   Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 1231f);


                    if (hit.transform.gameObject.layer == Mathf.Log(enemyMask,2))
                    {
                        hit.transform.GetComponent<EnemyController>().MakeDamage(hitDamage);
                        weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, false);
                    }
                   else if (hit.transform.gameObject.layer == Mathf.Log(playerMask,2))
                   {
                        Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 1231f,groundMask);
                        weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, true);

                   }
                   else
                   {

                       weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, true);
                   }



               }
               else
               {
                   //Звук не заряженного пистолета
               }
               


               // if (MeleeHit(transformCamera, 600f, enemyMask, out hit))
               // {
               //    hit.transform.GetComponent<EnemyController>().MakeDamage(hitDamage);
               //    weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, false);
               // }
               //else
               //{
               //    Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 1231f);
               //    weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, true);
               // }

               
            }
           else
           {

               if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Swing"))
               {

                   animator.Play("Swing");
                   if (MeleeHit(transformCamera, meleeDistance, woodMask, out hit))
                   {
                       GetComponent<AudioSource>().clip = woodHit;
                       GetComponent<AudioSource>().Play();
                       print("hit");
                       hit.transform.GetComponent<wood>().Hp -= hitDamage;
                       woodCount += hitDamage;

                   }
                   else if (MeleeHit(transformCamera, meleeDistance, enemyMask, out hit))
                   {
                       hit.transform.GetComponent<EnemyController>().MakeDamage(hitDamage);
                   }
                   else
                   {
                       print("not Hit");
                   }
               }

            }

           
          

        }
        if (Input.GetButton("Fire2"))
        {
            RaycastHit hit;
            Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 1231f);
            Instantiate(enemy, hit.point, Quaternion.identity);

           // animator.Play("Lhit");
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach(var a in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(a);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (var a in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                a.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 50, 0f));
            }
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        punch = punch / 2;
        controller.Move(punch*Time.deltaTime);
       

    }


    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

   

    public void Kick(Vector3 velocity)
    {
        punch = velocity;
        //controller.Move(velocity);
    }

    bool MeleeHit(Transform transformCamera,float distance,LayerMask layerMask, out RaycastHit hit )
    {
        
        if (Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, distance, layerMask))
        {
            return true;
        }

        return false;
    }
}
