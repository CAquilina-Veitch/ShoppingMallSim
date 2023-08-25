using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClickableWorker : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image sR;

    int specie;
    private void Start()
    {
        specie = Random.Range(0, sprites.Length);
        sR.sprite = sprites[specie];
    }


    public void Collected()
    {
        GameObject.FindGameObjectWithTag("UnhiredWorkers").GetComponent<UnhiredWorkers>().collectRandomWorker((species)specie);
        GameObject.FindGameObjectWithTag("UnhiredWorkers").GetComponent<UnhiredWorkers>().showList(true);
        Destroy(gameObject);
    }
}
