using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;

public class CountDownBeforeMusic : MonoBehaviour
{
    //  [SerializeField] private float startDelay = 0;
    [SerializeField] private float delay = 1;
    [SerializeField] private int startNumber = 3;

    [SerializeField] private GameObject menuBackgroundImage;  
    [SerializeField] private TextMeshProUGUI countDownText;

    private int repeatCount = 0;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip countAudioClip;

    public void LetsGo()
    {
        repeatCount = 0;
        countDownText.gameObject.SetActive(true);
        StartCoroutine(Repeating());
        menuBackgroundImage.gameObject.SetActive(false);
    }

    IEnumerator Repeating()
    {
        countDownText.text = (startNumber - repeatCount).ToString();
        if (audioSource != null && countAudioClip != null) { audioSource.PlayOneShot(countAudioClip); }
        yield return new WaitForSeconds(delay);
        repeatCount++;
        if (repeatCount < startNumber)
        {
            StartCoroutine(Repeating());
        }
        else
        {
            countDownText.text = "";            
            countDownText.gameObject.SetActive(false);          
         
            GameManager.Instance.TogglePause();

        }

    }
}