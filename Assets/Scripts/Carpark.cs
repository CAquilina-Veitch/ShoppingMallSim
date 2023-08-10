using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Carpark : MonoBehaviour
{
    [SerializeField] Wallet w;
    public UnityEvent unlock;
    public int cost = 15;
    // Start is called before the first frame update
    public void TryCarpark()
    {
        if (w.trySpend(false, cost) == true)
                {
                    unlock.Invoke();
                }
                else
                {

                }
    }

}
