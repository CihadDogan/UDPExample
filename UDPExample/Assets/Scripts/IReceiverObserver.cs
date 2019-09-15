using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReceiverObserver
{
    void OnDataReceived(double[] val);
}
