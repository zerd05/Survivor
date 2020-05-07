using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxWeapons = 2;

    public int currentWeapon = 0;

    public Animator animator;

    public GameObject weapon;
    // Start is called before the first frame update
    void Awake()
    {

    }

    void Start()
    {
        SelectWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (maxWeapons != transform.childCount - 1)
        {
            maxWeapons = transform.childCount-1;
            currentWeapon = transform.childCount - 1;
            SelectWeapon(currentWeapon);
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeapon + 1 <= maxWeapons)
            {
                currentWeapon++;
            }
            else
            {
                currentWeapon = 0;
            }

            SelectWeapon(currentWeapon);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon - 1 >= 0)
            {
                currentWeapon--;
            }
            else
            {
                currentWeapon = maxWeapons;
            }
            SelectWeapon(currentWeapon);
        }

        if (currentWeapon == maxWeapons + 1)
            currentWeapon = 0;
        if (currentWeapon == -1)
            currentWeapon = maxWeapons;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = 0;
            SelectWeapon(currentWeapon);
        }

    }

    void SelectWeapon(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index)
            {
                if (transform.GetChild(i).name == "Fists")
                {
                   
                }
                transform.GetChild(i).gameObject.SetActive(true);
                weapon = transform.GetChild(i).gameObject;
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
