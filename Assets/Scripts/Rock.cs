using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public int Hp = 500;

    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WoodReset());
    }

    IEnumerator WoodReset()
    {
        if (Hp <= 0)
        {
            meshCollider.enabled = false;
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(120.0f);
            meshRenderer.enabled = true;
            meshCollider.enabled = true;
            Hp = 500;
        }
    }
}
