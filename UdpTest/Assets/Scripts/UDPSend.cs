using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour
{
    // prefs
    public int ValToSend = 1;
    public string IP;  // define in init
    public int port;  // define in init
    public float lastTime;

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // start from unity3d
    public void Start()
    {
        init();
    }

    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        StartCoroutine(ISend());
    }

    void OnDisable()
    {
        client.Close();
    }

    IEnumerator ISend()
    {
        while (true)
        {
            SendMessage(ValToSend);
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    // sendData
    private void SendMessage(int val)
    {
        try
        {
            // Convert string message to byte array.  
            byte[] serverMessageAsByteArray = BitConverter.GetBytes(val);

            // Den message zum Remote-Client senden.
            client.Send(serverMessageAsByteArray, serverMessageAsByteArray.Length, remoteEndPoint);
            //}
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }
}