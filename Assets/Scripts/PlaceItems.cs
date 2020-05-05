using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceItems : MonoBehaviour
{

    private bool isPlacing = false;
    public GameObject campFirePreviewGreen;
    public GameObject campFirePreviewRed;
    public GameObject originalCampFire;
    public GameObject prev;
    private Transform player;
    private PlayerMove playerMove;

    private bool canPlace = true;
    // Update is called once per frame

    void Start()
    {
        player = PlayerManager.instance.player.transform;
        playerMove = player.GetComponent<PlayerMove>();
    }
    void Update()
    {
        Destroy(prev);
        GameObject player = PlayerManager.instance.player;

        Camera playerCamera = PlayerManager.instance.player.GetComponentInChildren<Camera>();
        RaycastHit hit;
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1231f, playerMove.groundMask);
        Quaternion vr = Quaternion.FromToRotation(Vector3.up, hit.normal);
        float x = Mathf.Abs(vr.x);
        float z = Mathf.Abs(vr.z);

        if (x > 0.3 || z > 0.3)
            canPlace = false;
        else
        {
            canPlace = true;
        }

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
               
            }
                
            
        }
        if (isPlacing)
        {
            player = PlayerManager.instance.player;

            playerCamera = PlayerManager.instance.player.GetComponentInChildren<Camera>();

            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1231f,playerMove.groundMask);

            if (playerMove.woodCount > 1100 && playerMove.rockCount > 400 && canPlace)
            {
                prev = Instantiate(campFirePreviewGreen, hit.point + new Vector3(0, 0.1f), Quaternion.identity);
            }
            else
            {
                prev = Instantiate(campFirePreviewRed, hit.point + new Vector3(0, 0.1f), Quaternion.identity);

            }
            if (Vector3.Distance(hit.point, player.transform.position) > 5f)
            {
                isPlacing = false;
                Destroy(prev);
            }
            prev.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if(playerMove.woodCount>1100 && playerMove.rockCount > 400 && canPlace)
                if (isPlacing)
                {
                    isPlacing = false;
                    player = PlayerManager.instance.player;

                    playerCamera = PlayerManager.instance.player.GetComponentInChildren<Camera>();
                    
                    Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 1231f, playerMove.groundMask);
                    Destroy(prev);
                    Instantiate(originalCampFire, hit.point+new Vector3(0, 0.1f), Quaternion.FromToRotation(Vector3.up, hit.normal));
                    playerMove.woodCount -= 1100;
                    playerMove.rockCount -= 400;
                }
        }
    }
}
