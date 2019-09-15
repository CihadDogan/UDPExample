using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationController : MonoBehaviour, IReceiverObserver
{
    UDPReceiver _UdpReceiver;
    UDPTransmitter _Udpransmitter;

    private void Awake()
    {
        _UdpReceiver = GetComponent<UDPReceiver>();
        _UdpReceiver.SetObserver(this);
        _Udpransmitter = GetComponent<UDPTransmitter>();
    }

    /// <summary>
    /// Send data immediately after receiving it.
    /// </summary>
    /// <param name="val"></param>
    void IReceiverObserver.OnDataReceived(double[] val)
    {
        _Udpransmitter.Send(val);
    }
}
