using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{

    [SerializeField] private Image mainMenuBackgroundImage;
    [SerializeField] private Image pauseMenuBackgroundImage;


    public void PauseGameIfNotInMainOrPauseMenus()
    {
        if (!mainMenuBackgroundImage.gameObject.activeInHierarchy && !pauseMenuBackgroundImage.gameObject.activeInHierarchy)
        {
            GameManager.Instance.PauseGame(true);
            pauseMenuBackgroundImage.gameObject.SetActive(true);
        }
    }
}
