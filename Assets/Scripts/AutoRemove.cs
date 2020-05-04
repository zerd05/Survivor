using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRemove : MonoBehaviour
{

    public float lifeTime = 20f;

    IEnumerator breakObject()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(breakObject());
    }


}
