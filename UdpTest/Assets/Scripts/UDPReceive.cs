using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

    UDPSend udpSend;

    // public
    public string IP = "127.0.0.1";
    public int port; // define > init

    // start from unity3d
    public void Start()
    {
        udpSend = GetComponent<UDPSend>();
        init();
    }

    // init
    private void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");
        print("Sending to 127.0.0.1 : " + port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");

        client = new UdpClient();

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void OnDisable()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
        receiveThread = null;

        client.Close();
    }

    // receive thread
    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                // string text = Encoding.UTF8.GetString(data);

                Debug.Log("INCOMING DATA : ");
                for (int i = 0; i < data.Length;i++)
                    Debug.Log(data[i]);

                int valToSend = ConvertByteArrayToInt(data);
                udpSend.ValToSend = valToSend;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    int ConvertByteArrayToInt(byte[] array)
    {
        int result = 0;

        string inf = "Array : ";
        for (int i = 0; i < array.Length; i++)
            inf += array[i].ToString() + ",";

        //Debug.Log(inf);

        for (int i = 0; i < array.Length; i++)
        {
            int sp = i * 8;
            result |= array[i] << sp;
        }

        double dresult = (double)result / 10000.0f;
        string sresult = dresult.ToString("0.0000");
        Debug.Log("<color=red>" + sresult + "</color>");

        return result;
    }
}