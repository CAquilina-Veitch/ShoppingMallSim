using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
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


            if (name.ToArray()[name.Length - 1] == 'L')
            {
                GameObject.FindGameObjectWithTag("SaveData").GetComponent<Progress>().carParkUpgrades[1]++;
            }
            else if (name.ToArray()[name.Length - 1] == 'R')
            {
                GameObject.FindGameObjectWithTag("SaveData").GetComponent<Progress>().carParkUpgrades[2]++;
            }
            else
            {
                GameObject.FindGameObjectWithTag("SaveData").GetComponent<Progress>().carParkUpgrades[0] = 1;
            }

        }
        else
        {

        }
    }
    public void forceCarpark()
    {
        unlock.Invoke();




    }


}
