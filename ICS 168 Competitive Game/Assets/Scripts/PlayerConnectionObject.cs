using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnectionObject : NetworkBehaviour
{
    public GameObject PlayerUnitPrefab;

    [SyncVar(hook = "OnPlayerNameChanged")]
    public string PlayerName = "Anonymous";

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

        if (Input.GetKeyDown(KeyCode.O))
        {
            CmdSpawnMyUnit();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            string n = "Player" + Random.Range(1, 100);

            Debug.Log("Sending the server a request to change our name to: " + n);
            CmdChangePlayerName(n);
        }
    }

    void OnPlayerNameChanged(string newName)
    {
        Debug.Log("OnPlayerNameChanged: OldName: " + PlayerName + "   NewName: " + newName);

        PlayerName = newName;

        gameObject.name = "PlayerConnectionObject [" + newName + "]";
    }

    [Command]
    void CmdSpawnMyUnit()
    {
        GameObject go = Instantiate(PlayerUnitPrefab);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }

    [Command]
    void CmdChangePlayerName(string n)
    {
        Debug.Log("CmdChangePlayerName: " + n);
        PlayerName = n;

        RpcChangePlayerName(n);
    }

    [ClientRpc]
    void RpcChangePlayerName(string n)
    {
        Debug.Log("RpcChangePlayerName: We were asked to change the player name on a particular PlayerConnectionObject: " + n);
        PlayerName = n;
    }

}
