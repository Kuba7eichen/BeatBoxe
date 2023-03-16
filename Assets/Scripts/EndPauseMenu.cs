using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class EndPauseMenu : MonoBehaviour
{
  //  [SerializeField] private float startDelay = 0;
    [SerializeField] private float delay = 1;
    [SerializeField] private int startNumber = 3;

    [SerializeField] TextMeshProUGUI countText;

    private int repeatCount = 0;


    
    public void LetsGo()
    {        
        StartCoroutine(Repeating());
    }
    
    IEnumerator Repeating()
    {
        countText.text = (startNumber - repeatCount).ToString();
        yield return new WaitForSeconds(delay);
        repeatCount++;
        if (repeatCount <= startNumber)
        {
            StartCoroutine(Repeating());
        }
        else
        {
            countText.gameObject.SetActive(false);
        }

    }
}
