using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
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
        CmdSpawnMyUnit();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            CmdSpawnMyUnit();
        }
    }

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(playerUnitPrefab);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
