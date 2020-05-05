using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItems : MonoBehaviour
{

    private bool isPlacing = false;
    public GameObject campFirePreview;
    public GameObject originalCampFire;
    public GameObject prev;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isPlacing)
            {
                isPlacing = false;
                Destroy(prev);
            }
            else
            {
                isPlacing = true;
                if (isPlacing)
                {
                    GameObject player = PlayerManager.instance.player;

                    Camera playerCamera = PlayerManager.instance.player.GetComponentInChildren<Camera>();
                    RaycastHit hit;
                    Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1231f);
                    prev = Instantiate(campFirePreview, hit.point + new Vector3(0, 0.1f), Quaternion.identity);
                }
            }
                
            
        }

        if (isPlacing)
        {
            GameObject player = PlayerManager.instance.player;

            Camera playerCamera = PlayerManager.instance.player.GetComponentInChildren<Camera>();
            RaycastHit hit;
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1231f);
            prev.transform.position = hit.point + new Vector3(0, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPlacing)
            {
                isPlacing = false;
                GameObject player = PlayerManager.instance.player;

                Camera playerCamera = PlayerManager.instance.player.GetComponentInChildren<Camera>();
                RaycastHit hit;
                Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1231f);
                Destroy(prev);
                Instantiate(originalCampFire, hit.point+new Vector3(0, 0.1f), Quaternion.identity);
            }
        }
    }
}
