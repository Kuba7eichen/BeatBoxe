using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The Music AudioSource must be attached to the Game Manager game object.

public class GameManager : MonoBehaviour
{
    public bool isPaused { get; private set; }


    private int score = 0;
    public int Score
    {
        get { return score; }
    }


    private int multiplier = 2;
    public int Multiplier
    {
        get { return multiplier; }
    }


    private int lowMultiplierCount = 0;
    public int LowMultiplierCount
    {
        get { return lowMultiplierCount; }
    }


    private bool gameOver = false;
    public bool GameOver
    {
        get { return gameOver; }
    }


    private bool gamePaused = false;
    public bool GamePaused
    {
        get { return gamePaused; }
    }



    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private AudioSource musicAudioSource;

    [SerializeField] AudioSource multiplierAudioSource;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance != null) return instance;
            else
            {
                instance = new GameManager();
                return instance;
            }
        }
    }


    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
    }



    public void UpdateMultiplier(int pointsToAdd)
    {
        multiplier = Mathf.Clamp(multiplier + pointsToAdd, 1, 8);
        if (multiplier == 1)
        {
            lowMultiplierCount++;
        }
        else
        {
            lowMultiplierCount = 0;
        }

        if (lowMultiplierCount >= 5)
        {
            gameOver = true;
        }

    }



    public void UpdateScore(int pointsToAdd)
    {
        score += pointsToAdd * multiplier;
    }



    public bool TogglePause()
    {
        gamePaused = !gamePaused;

        if (gamePaused) { musicAudioSource?.Pause(); }
        else { musicAudioSource?.Play(); }

        return gamePaused;
    }



   
}
