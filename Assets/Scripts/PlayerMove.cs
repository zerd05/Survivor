
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class PlayerMove : MonoBehaviour
{
    [HideInInspector]
    public Vector3 loadPosition;

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

    [Header("Слои")]
    public LayerMask woodMask;               //Слой дерева
    public LayerMask enemyMask;              //Слой врага
    public LayerMask armyMask;              //Слой врага
    public LayerMask playerMask;             //Слой персонажа
    public LayerMask lootMask;               //Слой предментов
    public LayerMask rockMask;               //Слой камней
    public LayerMask animalMask;             //Слой животных
    public LayerMask foodMask;               //Слой еды
    public LayerMask armyLootMask;   //Слой мертвого солдата
    public LayerMask waterLayer;
    [Space]             
    public int hitDamage = 50;               //Урон от удара

    public float fallWithoutDamage = 10f;
    public bool enableFallDamage = true;     //Применять ли урон от падений
    public float fallDamageMultiplayer = 2.5f; // Множитель урона от падений 


    [Header("Сложность")]
    public int eatLootPerHit = 15;          //Количество еды выпадающее за 1 удар


    [Header("Ресурсы")]
    public int woodCount = 0;               //Количество дерева
    public int bulletCount = 0;
    public int rockCount = 0;


    [Header("Свойства персонажа")]
    public float hp = 100;                     //Количество здоровья
    public float eat = 100;
    public float water = 100;
    public Image hpBar;
    public Image eatBar;
    public Image waterBar;

    [Header("Выпадающие премдеты")]
    public GameObject woodLoot;
    public GameObject rockLoot;
    public GameObject waterLoot;
    public GameObject chickenLoot;
    public GameObject pigLoot;
    public GameObject cowLoot;
    public GameObject pistolLoot;
    public GameObject ammoLoot;
    public GameObject knifeLoot;

    [Space]


    public Text woodText;                    //Информация о ресурсах
    public Text ammoText;

    [Header("Sounds")]
    public AudioClip woodHit;                //Звук удара топором по дереву
    public AudioClip playerHit;
    public AudioClip notHit;
    public AudioClip[] steps;
    public AudioClip jump;
    public AudioClip takePistol;
    public AudioClip takeItem;
    public AudioClip eatFood;
    public AudioClip drinkSound;






    private float startFall;
    private float endFall;
    private float fallHeight;


    public WeaponSwitch weaponSwitch;

    public GameObject bulletEffect;

    public TraumaInducer TraumaInducer;


    public Vector3 punch = new Vector3(0, 0);
    [Space]
    [Space]
    [Header("Debug")]
    public GameObject enemy;

    private bool isCampFireActive;
    private bool drawGUI;
    private Vector3 prevStep;

    public bool havePistol;

    public bool haveKnife;

    private int fr = 0;
    public void CampFireActive(bool status)
    {
        isCampFireActive = status;
    }
    void Start()
    {
        prevStep = transform.position;



    }

    //private bool endLoad = false;
    //void LateUpdate()
    //{
    //    LoadInfo.isLoadGame = true;
    //    if (LoadInfo.isLoadGame && endLoad)
    //    {
    //        transform.position = loadPosition;
            
    //        endLoad = false;
    //    }
    //}

    void FixedUpdate()
    {
       
        //Система еды
        if (eat < 0)
            eat = 0;
        eat -= 0.001f;
        if (water < 0)
                water = 0;
        water -= 0.01f;
        if (Input.GetAxis("Sprint") > 0 && isGrounded)
        {
            eat -= 0.001f;
            water -= 0.01f;
        }
        
        if (hp < 100 && water > 80 && eat > 90)
        {
            hp += 0.01f;
           
        }

        if (hp < 100 && water > 30 && eat > 20)
        {
            if (isCampFireActive)
                hp += 0.1f;
        }

        if (water <= 0 || eat <= 0)
        {
            hp -= 0.01f;
        }

        if (hp > 100)
            hp = 100;


    }


    void Update()
    {
        if(InGameMenu.IsPaused)
            return;


        UpdateBars();

        RaycastHit ray;


        Physics.Raycast(transformCamera.position, transformCamera.forward, out ray, 3f,lootMask); //Подбор пердметов
        if (ray.transform != null)
        {
            drawGUI = true;
            if (ray.transform.tag == "Magazine")
            {

                textGui = "Нажмите E чтобы взять";
            }
            if (ray.transform.tag == "Rock")
            {
                textGui = "Нажмите E чтобы взять";
            }
            if (ray.transform.tag == "Wood")
            {
                textGui = "Нажмите E чтобы взять";

            }
            if (ray.transform.tag == "Food")
            {
                textGui = "Нажмите E чтобы съесть";
            }
            if (ray.transform.tag == "Water")
            {
                textGui = "Нажмите E чтобы выпить";

            }
            if (ray.transform.tag == "WaterBig")
            {

                textGui = "Нажмите E чтобы выпить";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {

                if (ray.transform.tag == "Magazine")
                {
                    bulletCount += 14;
                    PlaySound(takePistol);
                    textGui = "Нажмите E чтобы выпить";
                }
                if (ray.transform.tag == "Rock")
                {
                    rockCount += ray.transform.GetComponent<Loot>().count;
                    PlaySound(takeItem);
                   
                }
                if (ray.transform.tag == "Wood")
                {
                    woodCount += ray.transform.GetComponent<Loot>().count;
                    PlaySound(takeItem);
                  
                }
                if (ray.transform.tag == "Food")
                {
                    eat += ray.transform.GetComponent<Loot>().count;
                    if (eat > 100)
                        eat = 100;
                    PlaySound(eatFood);
                  
                }
                if (ray.transform.tag == "Water")
                {
                    water += ray.transform.GetComponent<Loot>().count;
                    if (water > 100)
                        water = 100;
                    PlaySound(drinkSound);
                  
                }
                if (ray.transform.tag == "WaterBig")
                {
                    water += 25;
                    if (water > 100)
                        water = 100;
                    PlaySound(drinkSound);
                    
                }

                if (ray.transform.tag != "WaterBig")
                    Destroy(ray.transform.gameObject);

            }
        }
        else
        {
            drawGUI = false;
        }



        GameObject.FindGameObjectWithTag("woodText").GetComponent<Text>().text = "Дерево: " + woodCount.ToString()+"\nКамень: "+rockCount.ToString();
        if (weaponSwitch.weapon.name == "Pistol")
        {
            GameObject.FindGameObjectWithTag("ammoText").GetComponent<Text>().text = weaponSwitch.weapon.GetComponent<PistolShoot>().bullets+"/"+bulletCount;
        }
        else
        {
            GameObject.FindGameObjectWithTag("ammoText").GetComponent<Text>().text = "";
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
                if (fallHeight > fallWithoutDamage)
                {
                    hp -= Mathf.RoundToInt(Mathf.Abs(fallHeight * fallDamageMultiplayer));
                    PlaySound(playerHit);
                }
                    
                startFall = 0;

            }
        }

        if (isGrounded) //Звуки шагов
        {
            if (Vector3.Distance(prevStep, transform.position) > 4&& Input.GetAxis("Sprint") == 0)
            {
               
                SoundSysyem soundSysyem = new SoundSysyem();

                soundSysyem.PlaySound2D(steps[Random.Range(0, steps.Length)], transform.position,1f);
                prevStep = transform.position;
            }
            else if(Vector3.Distance(prevStep, transform.position) > 5)
            {
                SoundSysyem soundSysyem = new SoundSysyem();

                soundSysyem.PlaySound2D(steps[Random.Range(0, steps.Length)], transform.position,1f);
                prevStep = transform.position;
            }
        }
         

         

         if (hp <= 0) //смерть
         {
             hp = 0;
            // GameObject a = GameObject.FindGameObjectWithTag("arms");
            // a.SetActive(false);
            //woodText.text = "Дерево: " + woodCount.ToString() + "\nЗдоровье: " + hp.ToString();
            //woodText.text = "\n\n\n\n\nПерсонаж погиб";
            //GetComponent<PlayerMove>().enabled = false;
            //GetComponent<CharacterController>().enabled = false;
            //gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(500f,50f,500f));
            //GetComponent<Rigidbody>().mass += 300;
            //GetComponent<MouseLock>().enabled = false;
            LoadInfo.isAlive = false;

         }

        if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }

       
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(Input.GetAxis("Sprint")>0 && isGrounded && Input.GetKey(KeyCode.W))
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
            SoundSysyem soundSysyem = new SoundSysyem();

            soundSysyem.PlaySound2D(jump, transform.position);
            

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
                   
                  

                   animator.Play("Shot");


                   Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 1231f);

                   if (hit.transform != null)
                   {
                       if (hit.transform.gameObject.layer == Mathf.Log(enemyMask, 2))
                       {
                           hit.transform.GetComponent<EnemyController>().MakeDamage(weaponSwitch.weapon.GetComponent<PistolShoot>().damage);
                           weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, false);
                       }
                       else if (hit.transform.gameObject.layer == Mathf.Log(armyMask, 2))
                       {
                           hit.transform.GetComponent<ArmyController>().MakeDamage(weaponSwitch.weapon.GetComponent<PistolShoot>().damage);
                           weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, false);
                       }
                        else if (hit.transform.gameObject.layer == Mathf.Log(animalMask, 2))
                       {
                           hit.transform.GetComponent<AnimalController>().MakeDamage(weaponSwitch.weapon.GetComponent<PistolShoot>().damage);
                           weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, false);

                        }
                        else if (hit.transform.gameObject.layer == Mathf.Log(playerMask, 2))
                       {
                           Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 1231f, groundMask);
                           weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, true);

                       }
                       else if(hit.transform.gameObject.layer == Mathf.Log(lootMask, 2))
                       {
                           Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 1231f, groundMask);
                           weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, true);
                        }
                       else
                       {

                           weaponSwitch.weapon.GetComponent<PistolShoot>().Shot(hit, true);
                       }
                    }
                   else
                   {
                       weaponSwitch.weapon.GetComponent<PistolShoot>().FakeShoot();
                    }
                   
                    


               }


               
            }
           else
           {


               if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Swing"))   //Удары
               {

                   animator.Play("Swing");
                   if (MeleeHit(transformCamera, meleeDistance, woodMask, out hit))
                   {

                       PlaySound(woodHit);
                       print("Wood hit");
                       if (weaponSwitch.weapon.name == "Axe")
                       {
                           hit.transform.GetComponent<wood>().Hp -= hitDamage;
                           CreateLoot(woodLoot, hitDamage);
                           if (hit.transform.GetComponent<wood>().Hp <= 0)
                           {
                               Destroy(hit.transform.parent.gameObject);
                           }
                       }
                       else
                       {
                           hit.transform.GetComponent<wood>().Hp -= hitDamage/10;
                           CreateLoot(woodLoot, hitDamage/10);
                           if (hit.transform.GetComponent<wood>().Hp <= 0)
                           {
                               Destroy(hit.transform.parent.gameObject);
                           }
                        }
                       

                   }
                   if (MeleeHit(transformCamera, meleeDistance, rockMask, out hit))
                   {
                       GetComponent<AudioSource>().clip = woodHit;
                       GetComponent<AudioSource>().Play();
                       print("Rock hit");


                       if (weaponSwitch.weapon.name == "Pickaxe")
                       {
                           hit.transform.GetComponent<Rock>().Hp -= hitDamage;
                           CreateLoot(rockLoot, hitDamage);
                           if (hit.transform.GetComponent<Rock>().Hp <= 0)
                           {
                               Destroy(hit.transform.parent.gameObject);
                           }
                        }
                       else
                       {
                           hit.transform.GetComponent<Rock>().Hp -= hitDamage/10;
                           CreateLoot(rockLoot, hitDamage/10);
                           if (hit.transform.GetComponent<Rock>().Hp <= 0)
                           {
                               Destroy(hit.transform.parent.gameObject);
                           }
                        }
                        

                   }

                   if (MeleeHit(transformCamera, meleeDistance, foodMask, out hit))
                   {

                       PlaySound(woodHit);
                        print("Food hit");
                       hit.transform.GetComponent<AnimalController>().deadHealth -= hitDamage;

                       int lootCount = eatLootPerHit;
                       if (weaponSwitch.weapon.name != "Knife")
                       {
                           lootCount = lootCount/3;
                       }

                        switch (hit.transform.tag)
                       {
                               case "Chicken":
                               {
                                  
                                       CreateLoot(chickenLoot, lootCount);
                                   
                                }
                                break;
                               case "Cow":
                               {
                                   CreateLoot(cowLoot, lootCount);


                               }
                                   break;
                               case "Pig":
                               {
                                   CreateLoot(pigLoot, lootCount);
                               }
                                   break;

                        }
                   }
                   if (MeleeHit(transformCamera, meleeDistance, animalMask, out hit))
                   {
                       PlaySound(woodHit);
                        hit.transform.GetComponent<AnimalController>().MakeDamage(hitDamage / 4);
                   }
                    else if (MeleeHit(transformCamera, meleeDistance, enemyMask, out hit))
                   {
                       PlaySound(woodHit);
                        hit.transform.GetComponent<EnemyController>().MakeDamage(hitDamage/4);
                   }
                    else if (MeleeHit(transformCamera, meleeDistance, armyMask, out hit))
                    {
                        PlaySound(woodHit);
                        hit.transform.GetComponent<ArmyController>().MakeDamage(hitDamage / 4);

                    }
                   else if (MeleeHit(transformCamera, meleeDistance, armyLootMask, out hit))
                   {
                       PlaySound(woodHit);

                        hit.transform.GetComponent<ColiderArmy>().MakeDamage(hitDamage / 4);
                       print(weaponSwitch.maxWeapons);
                       int rand = Random.Range(1, 6);
                       
                       switch (rand)
                       {
                           case 1:
                           {
                               bool exist = false;
                               var a = weaponSwitch.WeaponNames();
                               foreach (var item in a)
                               {
                                   if (item == "Pistol")
                                       exist = true;
                               }
                               if(!exist)
                                    CreateLoot(pistolLoot);
                               break;
                           }
                           case 2:
                           {
                               bool exist = false;
                               var a = weaponSwitch.WeaponNames();
                               foreach (var item in a)
                               {
                                   if (item == "Knife")
                                       exist = true;
                               }
                               if (!exist)
                                        CreateLoot(knifeLoot);
                               break;
                           }
                           case 3:
                           {
                                    CreateLoot(pigLoot,20);
                               break;
                           }
                           case 4:
                           {
                               CreateLoot(waterLoot, 40);
                               break;
                           }
                           case 5:
                           {
                               CreateLoot(ammoLoot, Random.Range(2,13));
                               break;
                           }

                        }

                   }
                    else
                   {
                       PlaySound(notHit);
                        print("not Hit");
                   }
               }

            }

           
          

        }
        if (Input.GetKeyDown(KeyCode.Q))
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
            
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        punch = punch / 2;
        controller.Move(punch*Time.deltaTime);
       

    }


    /// <summary>
    /// Обновление переменных для сохранения игры
    /// </summary>
    public void UpdateItemSave()
    {
        var a = weaponSwitch.WeaponNames();
        foreach (var item in a)
        {
            if (item == "Knife")
                haveKnife = true;
            if (item == "Pistol")
                havePistol = true;
        }
    }


    /// <summary>
    /// Создание лута с колличеством
    /// </summary>
    /// <param name="item">Предмет для создания</param>
    /// <param name="count">Количество лута в предмете</param>
    public void CreateLoot(GameObject item,int count)
    {
        RaycastHit hit;
        Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 255f);
        Instantiate(item, hit.point, Quaternion.identity).GetComponent<Loot>().count = count;
        
    }


    /// <summary>
    /// Создание лута одиночного педмета
    /// </summary>
    /// <param name="item"></param>
    public void CreateLoot(GameObject item)
    {
        RaycastHit hit;
        Physics.Raycast(transformCamera.position, transformCamera.forward, out hit, 255f);
        Instantiate(item, hit.point, Quaternion.identity);

    }

    public string textGui;
    private void OnGUI()
    {
        if (drawGUI)
            GUI.Box(new Rect(Screen.width*0.5f - 250/2, Screen.height*0.5f + 22, 250, 22), textGui);
    }
    public void UpdateBars()
    {
        GameObject.FindGameObjectWithTag("hpBar").GetComponent<SimpleHealthBar>().UpdateBar(hp, 100);
        GameObject.FindGameObjectWithTag("eatBar").GetComponent<SimpleHealthBar>().UpdateBar(eat, 100);
        GameObject.FindGameObjectWithTag("waterBar").GetComponent<SimpleHealthBar>().UpdateBar(water, 100);
        //hpBar.GetComponent<SimpleHealthBar>().UpdateBar(hp,100);
        //eatBar.GetComponent<SimpleHealthBar>().UpdateBar(eat,100);
        //waterBar.GetComponent<SimpleHealthBar>().UpdateBar(water, 100);
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        PlaySound(playerHit);
        StartCoroutine(TraumaInducer.Shake());


    }

    public void PlaySound(AudioClip sound)
    {
        SoundSysyem soundSysyem = new SoundSysyem();
        soundSysyem.PlaySound(sound, transform.position);
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
