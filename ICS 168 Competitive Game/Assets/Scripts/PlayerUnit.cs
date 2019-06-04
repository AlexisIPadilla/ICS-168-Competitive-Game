using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour
{
    public int maxHealth;
    public int health;
    public bool positionLock;
    public GameObject bullet;

    Vector3 velocity;
    Vector3 bestGuessPosition;

    public float speed;

    public float jumpSpeed;
    public float verticalDragSpeed;
    public int heightLevel;
    public float planeVertDist;

    public Transform cameraFocus;
    bool cameraUnset = true;
    Transform cameraTransform;

    float ourLatency;
    float latencySmoothingFactor = 10;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
       if (hasAuthority == false)
        {
            bestGuessPosition = bestGuessPosition + (velocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, bestGuessPosition, Time.deltaTime * latencySmoothingFactor);

            return;
        }

        transform.Translate(velocity * Time.deltaTime);

        if (positionLock)
            velocity = Vector3.zero;
        else
            velocity = new Vector3(Input.GetAxis("Horizontal") * speed, velocity.y, Input.GetAxis("Vertical") * speed);

        CmdUpdateVelocity(velocity, transform.position);



        //-----ORIGINAL JUMP CODE STARTS HERE-----
        /*float deltaVVel = verticalDragSpeed * Time.deltaTime; //Multiply by deltaTime to keep it consistent for different frame rates
        if (Mathf.Abs(velocity.y) < deltaVVel)                //Snap velocity to 0 if it's less than what we're changing it by
            velocity.y = 0;
        else if (velocity.y > 0)                              //Otherwise, increase/decrease the velocity appropriately
            velocity.y = velocity.y - deltaVVel;
        else
            velocity.y = velocity.y + deltaVVel;

        if (Input.GetButtonDown("Jump"))
        {
            //Only allow further jumps if you are no longer moving vertically.
            //This feels kinda bad, so further refinement may have to be done.
            if (velocity.y == 0) {
                if (Input.GetButton("Alternate Button")) {
                    velocity.y = velocity.y - jumpSpeed;
                }
                else {
                    velocity.y = velocity.y + jumpSpeed;
                }
            }
        }*/
        //-----ORIGINAL JUMP CODE ENDS HERE-----

        //-----ALTERNATE JUMP CODE STARTS HERE-----
        //Move between levels at a constant velocity.
        if (Input.GetButtonDown("Jump"))
        {
            if (Input.GetButton("Alternate Button")) {
                --heightLevel;
                velocity.y = -jumpSpeed;
            }
            else {
                ++heightLevel;
                velocity.y = jumpSpeed;
            }
        }

        //If you're moving up, and you're above the height you should be at, stop.
        //Same idea if you're moving down.
        float targetHeight = heightLevel * planeVertDist;
        if (velocity.y > 0 && transform.position.y > targetHeight) {
            velocity.y = 0;
            transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        }

        if (velocity.y < 0 && transform.position.y < heightLevel * planeVertDist) {
            velocity.y = 0;
            transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
        }
        //-----ALTERNATE JUMP CODE ENDS HERE-----



        if (Input.GetButtonDown("Fire1")) {
            //Debug.Log ("Trying to stop moving.");
            CmdSpawnBullet(cameraTransform.position + cameraTransform.forward * 3, cameraTransform.rotation);
            //CmdStartSkill(true);
        }



        if (Input.GetKeyDown(KeyCode.P))
        {
            Destroy(gameObject);
        }

        //If we haven't already set the focus of the main camera to this, set it to this.
        //We already know this is our own player character, so we won't accidentally follow someone else.
        if (cameraUnset)
        {
            cameraUnset = false;
            Camera.main.GetComponent<cameraScript>().player = cameraFocus;
        }
    }

    [Command]
    void CmdSpawnBullet(Vector3 p, Quaternion r)
    {
        GameObject newBullet = Instantiate(bullet, p, r);

        //NetworkServer.SpawnWithClientAuthority(newBullet, connectionToClient);

        RpcSpawnBullet(p, r);
    }

    [ClientRpc]
    void RpcSpawnBullet(Vector3 p, Quaternion r)
    {
        if (hasAuthority)
            return;
        GameObject newBullet = Instantiate(bullet, p, r);
    }

    [Command]
    void CmdUpdateVelocity(Vector3 v, Vector3 p)
    {
        // I am on a server
        transform.position = p;
        velocity = v;

        //  transform.position = p + (v * (thisPlayersLatencyToServer))

        // Now let the clients know the correct position of this object.
        RpcUpdateVelocity(velocity, transform.position);
    }

    [ClientRpc]
    void RpcUpdateVelocity(Vector3 v, Vector3 p)
    {
        // I am on a client

        if (hasAuthority)
        { 
            return;
        }

        // I am a non-authoritative client, so I definitely need to listen to the server.

        // If we know what our current latency is, we could do something like this:
        //  transform.position = p + (v * (ourLatency))

        //transform.position = p;

        velocity = v;
        bestGuessPosition = p + (velocity * (ourLatency));


        // Now position of player one is as close as possible on all player's screens

        // IN FACT, we don't want to directly update transform.position, because then 
        // players will keep teleporting/blinking as the updates come in. It looks dumb.
    }


    [Command]
    void CmdStartSkill(bool pLock) {
        Debug.Log("Locking Position on Server");
        positionLock = pLock;

        RpcStartSkill(pLock);
    }

    [ClientRpc]
    void RpcStartSkill(bool pLock) {
        if (hasAuthority)
            return;
        Debug.Log("I'm a client that got locked.");
        positionLock = pLock;
    }


}
