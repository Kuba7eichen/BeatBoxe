using CCSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


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


    [SerializeField] AnimationOnBPM[] animationOnBPMObject;

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
    [SerializeField] Database[] musics;
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
        CheckMusicDatabases();
        SetAnimatedObjectsBPM();
    }

    

    private void CheckMusicDatabases()
    {
        for (int i = 0; i < musics.Length; i++)
        {
            for (int j = 0; j < musics[i].ObjectSpawns.Length; j++)
            {
                int originalLane = musics[i].ObjectSpawns[j].Lane;
                musics[i].ObjectSpawns[j].Lane = Mathf.Clamp(musics[i].ObjectSpawns[j].Lane, 1, 3);

                if (originalLane != musics[i].ObjectSpawns[j].Lane)
                {
                    Debug.Log("Un objet � spawner n'a pas de lane assign�e. Il a �t� plac� par d�faut sur la lane 1.");
                }
            }
        }
    }

    private void BuildMusicChoiceDropdown()
    {
        musicChoiceDropdown.ClearOptions();

        // Cr�e une liste d'options � partir du tableau de musiques
        var options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < musics.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(musics[i].musicDatas.audioClip.name));
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
        // On se cale sur le rythme du morceau. Chaque unit� de musicTime vaut non pas une seconde mais un �cart entre deux beats.
        // Et tient compte du d�lai initial, pour que les spawns se calent bien sur les beats de la musique :
        float musicTime = (musicAudioSource.time * musics[actualMusicIndex].musicDatas.Bpm / 60) - musics[actualMusicIndex].musicDatas.firstBpmDelay;

        if (!lastObjectSpawned)
        {
            if (musics[actualMusicIndex].ObjectSpawns.Length > 0 && musicTime >= musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].SpawnBeat)
            {
                MovingElement objectToSpawn = musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].prefabToSpawn.GetComponent<MovingElement>();

                spawner.SpawnObject(objectToSpawn.type, musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].Lane);

                if (nextSpawnIndex < musics[actualMusicIndex].ObjectSpawns.Length - 1)
                {
                    nextSpawnIndex++;
                }
                else if (nextSpawnIndex == musics[actualMusicIndex].ObjectSpawns.Length - 1) lastObjectSpawned = true;
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
        musicAudioSource.clip = musics[musicIndex].musicDatas.audioClip;
        actualMusicIndex = musicIndex;
        musicAudioSource.Play();
        SetAnimatedObjectsBPM();
    }



    private void SetAnimatedObjectsBPM()
    {
        for (int i = 0; i < animationOnBPMObject.Length; i++)
        {
            animationOnBPMObject[i].SetAnimatorBPM(musics[actualMusicIndex].musicDatas.Bpm,
                                                   musics[actualMusicIndex].musicDatas.firstBpmDelay);
        }
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
