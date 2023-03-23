using CCSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class Music
{
    public AudioClip musicClip;
    public Database musicDatabase;
}



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




    [SerializeField] private LayerMask playerMask;
    public LayerMask PlayerMask { get { return playerMask; } }

    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;


    public Transform Head { get { return head; } }
    public Transform LeftHand { get { return leftHand; } }
    public Transform RightHand { get { return rightHand; } }


    private AudioSource musicAudioSource;

    [SerializeField] AudioSource multiplierAudioSource;
    [SerializeField] Music[] musics;
    private int actualMusicIndex = 0;

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
           
    private int nextSpawnIndex = 0;

    [SerializeField] private Spawner spawner;

    private bool lastObjectSpawned = false;


    [SerializeField] TMP_Dropdown musicChoiceDropdown;


    // DEBUG : LANCEMENT DE LA MUSIQUE AU CHARGEMENT POUR TESTER LE SPAWN
    private void Start()
    {
        musicAudioSource.Play();

        BuildMusicChoiceDropdown();
        CheckMusicDatabase();
    }



    private void CheckMusicDatabase()
    {        
        for (int i = 0; i < musics.Length; i++)
        {
            for (int j = 0; j < musics[i].musicDatabase.ObjectSpawns.Length; j++)
            {
                int originalLane = musics[i].musicDatabase.ObjectSpawns[j].Lane;
                musics[i].musicDatabase.ObjectSpawns[j].Lane = Mathf.Clamp(musics[i].musicDatabase.ObjectSpawns[j].Lane, 1, 3);
                
                if (originalLane != musics[i].musicDatabase.ObjectSpawns[j].Lane)
                {
                    Debug.Log("Vous avez oublié de définir une lane pour un objet. Il a été placé par défaut sur la lane 1.");
                }
            }           
        }       
    }

    private void BuildMusicChoiceDropdown()
    {
        musicChoiceDropdown.ClearOptions();

        // Crée une liste d'options à partir du tableau de musiques
        var options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < musics.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(musics[i].musicClip.name));
        }

        // Ajoute les options au Dropdown
        musicChoiceDropdown.AddOptions(options);
    }


    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // On se cale sur le rythme du morceau. Chaque unité de musicTime vaut non pas une seconde mais un écart entre deux beats :
        float musicTime = musicAudioSource.time * musics[actualMusicIndex].musicDatabase.musicDatas.Bpm / 60;

        if (!lastObjectSpawned)
        {
            if (musicTime >= musics[actualMusicIndex].musicDatabase.ObjectSpawns[nextSpawnIndex].SpawnBeat)
            {
                MovingElement objectToSpawn = musics[actualMusicIndex].musicDatabase.ObjectSpawns[nextSpawnIndex].prefabToSpawn.GetComponent<MovingElement>();

                spawner.SpawnObject(objectToSpawn.type, musics[actualMusicIndex].musicDatabase.ObjectSpawns[nextSpawnIndex].Lane);

                if (nextSpawnIndex < musics[actualMusicIndex].musicDatabase.ObjectSpawns.Length - 1)
                {
                    nextSpawnIndex++;
                }
                else if (nextSpawnIndex == musics[actualMusicIndex].musicDatabase.ObjectSpawns.Length - 1) lastObjectSpawned = true;
            }
        }
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


    public void ChangeMusic(int musicIndex)
    {
        musicIndex = Mathf.Clamp(musicIndex, 0, musics.Length - 1);
        musicAudioSource.clip = musics[musicIndex].musicClip;
        actualMusicIndex = musicIndex;
        musicAudioSource.Play();       
    }



    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }


}
