using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerUnit : NetworkBehaviour
{
    Vector3 velocity;
    Vector3 bestGuessPosition;

    float ourLatency;
    float latencySmoothingFactor = 10;

    // Start is called before the first frame update
    void Start()
    {
        
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.Translate(0,1,0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Destroy(gameObject);
        }

        if ( /* some input */ true)
        {
            velocity = new Vector3(1, 0, 0);

            CmdUpdateVelocity(velocity, transform.position);
        }
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

        // I am a non-authoratative client, so I definitely need to listen to the server.

        // If we know what our current latency is, we could do something like this:
        //  transform.position = p + (v * (ourLatency))

        //transform.position = p;

        velocity = v;
        bestGuessPosition = p + (velocity * (ourLatency));


        // Now position of player one is as close as possible on all player's screens

        // IN FACT, we don't want to directly update transform.position, because then 
        // players will keep teleporting/blinking as the updates come in. It looks dumb.
    }


}
