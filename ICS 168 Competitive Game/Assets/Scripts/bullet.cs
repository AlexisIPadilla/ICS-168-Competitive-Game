using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float deathTime;
    void Start()
    {
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        //print(Time.time);
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
        //print(Time.time);
    }


}
