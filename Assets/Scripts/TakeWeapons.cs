using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeWeapons : MonoBehaviour
{
    private bool drawGUI = false;
    public LayerMask WeaponMask;
    private PlayerMove playerMove;
    public GameObject hand;
    public GameObject pistolPrefab;
    public GameObject knifePrefab;

    [Header("Sounds")]
    public AudioClip takePistol;

    public AudioClip takeKnife;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
       // hand = GameObject.FindGameObjectWithTag("Hand");

    }
    void Update()
    {
        
        RaycastHit ray;
        Physics.Raycast(playerMove.transformCamera.position, playerMove.transformCamera.forward, out ray, 3f, WeaponMask);
        if (ray.transform != null)
        {
            
            drawGUI = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Destroy(ray.transform.gameObject);
                bool canCreate = true;
                    if (ray.transform.tag == "PistolLoot")
                    {
                        for (int i = 0; i < hand.transform.childCount; i++)
                        {
                            if (hand.transform.GetChild(i).name == "Pistol")
                                canCreate = false;
                        }

                        if (canCreate)
                        {
                            Instantiate(pistolPrefab, hand.transform).name = "Pistol";
                            playerMove.PlaySound(takePistol);
                        }
                            
                    }
                    if (ray.transform.tag == "KnifeLoot")
                    {
                        for (int i = 0; i < hand.transform.childCount; i++)
                        {
                            if (hand.transform.GetChild(i).name == "Knife")
                                canCreate = false;
                        }

                        if (canCreate)
                        {
                            Instantiate(knifePrefab, hand.transform).name = "Knife";
                            playerMove.PlaySound(takeKnife);
                        }
                           
                    }
                
            }

        }
        else
        {
            drawGUI = false;
        }

    }

    public void TakeKnife()
    {
        print(knifePrefab);
        print(hand);
        Instantiate(knifePrefab, hand.transform).name = "Knife";
    }

    public void TakePistol()
    {
        var a = Instantiate(pistolPrefab, hand.transform).name = "Pistol";
        
    }
    private void OnGUI()
    {
        if (drawGUI)
            GUI.Box(new Rect(Screen.width * 0.5f - 51, Screen.height * 0.5f + 22, 102, 22), "Нажмите Е чтобы подобрать");
    }
}
