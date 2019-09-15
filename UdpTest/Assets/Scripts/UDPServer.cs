using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPServer : MonoBehaviour {

    // prefs
    public int ValToSend = 1;
    public string IP = "127.0.0.1";
    public int ReceivePort;
    public int TransmitPort;

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient TransmitClient;
    UdpClient ReceiveClient;
    // receiving Thread
    Thread receiveThread;

    // start from unity3d
    public void Start()
    {
        init();
    }

    // init
    public void init()
    {
        // Transmit
        print("UDPSend.init()");
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), TransmitPort);
        TransmitClient = new UdpClient();
        StartCoroutine(ISendVal());

        // Receive
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void OnApplicationQuit()
    {
        receiveThread.Abort();
        receiveThread = null;
        TransmitClient.Close();
        ReceiveClient.Close();
    }

    IEnumerator ISendVal()
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
            TransmitClient.Send(serverMessageAsByteArray, serverMessageAsByteArray.Length, remoteEndPoint);
            //}
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    // receive thread
    private void ReceiveData()
    {
        ReceiveClient = new UdpClient(ReceivePort);

        while (true)
        {
            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = ReceiveClient.Receive(ref anyIP);

                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                // string text = Encoding.UTF8.GetString(data);

                Debug.Log("INCOMING DATA : ");
                for (int i = 0; i < data.Length; i++)
                    Debug.Log(data[i]);

                int valToSend = ConvertByteArrayToInt(data);
                ValToSend = valToSend;
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
