using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableWorker : MonoBehaviour
{
    public WorkerInfo info;

    public void Collected()
    {
        Destroy(gameObject);
    }
}
