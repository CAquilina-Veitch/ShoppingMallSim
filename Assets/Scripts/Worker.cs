using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public WorkerInfo info;

    public void Collected()
    {
        Destroy(gameObject);
    }
}
