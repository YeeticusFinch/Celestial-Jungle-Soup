using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class FancyManager : NetworkManager {

    const short MESSAGE_ID = 1002;

    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);

        NetworkManager.singleton.client.RegisterHandler(MESSAGE_ID, OnServerReadyToBeginMessage);

        Debug.Log("Registered Handler...");

        // client joined the host
        if (connection.connectionId > 0)
        {
            var msg = new EmptyMessage();
            NetworkManager.singleton.client.Send(MESSAGE_ID, msg);
        }
        //        Camera.main.SendMessage (GameConstants.ACTIVATE_NETWORK_WAITING_PANEL, false, SendMessageOptions.DontRequireReceiver);
    }


    void OnServerReadyToBeginMessage(NetworkMessage netMsg)
    {
        Debug.Log("Network Message received...");
    }
}
