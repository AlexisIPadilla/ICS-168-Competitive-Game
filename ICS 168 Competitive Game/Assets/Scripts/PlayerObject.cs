using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObject : NetworkBehaviour
{
    public GameObject playerUnitPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        Debug.Log("PlayerObject::Start -- Spawning my own personal unit");
        Instantiate(playerUnitPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
