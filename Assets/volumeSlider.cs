using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class volumeSlider : MonoBehaviour
{
    [SerializeField] AudioSource aS;
    [SerializeField] Slider sl;
    [SerializeField] TextMeshProUGUI text;

    public void ChangeVolume()
    {
        aS.volume = sl.value/100f;
        text.text = $"{(int)sl.value}";
    }
    private void OnEnable()
    {
        sl.value = aS.volume * 100f;
        text.text = $"{(int)sl.value}";
    }


}
