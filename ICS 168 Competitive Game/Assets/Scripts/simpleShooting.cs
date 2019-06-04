using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int speed;
    //public Rigidbody projectile;
    //public Transform Spawnpoint;
    // Start is called before the first frame update
    void Start()
    {
        //bullet = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
            
           //Rigidbody clone;
            //clone = (Rigidbody)Instantiate(projectile, Spawnpoint.position, projectile.rotation);

            //clone.velocity = Spawnpoint.TransformDirection(Vector3.forward * 20);
        //}






        if(Input.GetKeyDown(KeyCode.Q))
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
            
            //Destroy(bullet.gameObject, deathTime);




          // bullet = Instantiate(bulletType);
           // bullet.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
           // deathTime -= Time.deltaTime;
           //if(deathTime < 0)
           //{
           //   Destroy(bullet);
            //    deathTime
            //}
        }

    }

   // void Fire()
    //{
     //   GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
      //  bullet.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
       // deathTime = Time.time + speed;

    //}

    //void OnTriggerEnter(Collider other)
    //{
     //   Debug.Log("Hit!");
    //}
}


