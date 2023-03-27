using CCSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


// The Music AudioSource must be attached to the Game Manager game object.

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{

    private int score = 0;
    public int Score { get { return score; } }


    private int multiplier = 2;
    public int Multiplier { get { return multiplier; } }


    private int lowMultiplierCount = 0;
    public int LowMultiplierCount { get { return lowMultiplierCount; } }


    private bool gameOver = false;
    public bool GameOver { get { return gameOver; } }


    private bool gamePaused = true;
    public bool GamePaused { get { return gamePaused; } }


    [SerializeField] AnimationOnBPM[] animationOnBPMObject;

    [SerializeField] private LayerMask playerMask;
    public LayerMask PlayerMask { get { return playerMask; } }

    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;


    public Transform Head { get { return head; } }
    public Transform LeftHand { get { return leftHand; } }
    public Transform RightHand { get { return rightHand; } }


    [HideInInspector] public AudioSource musicAudioSource;

    [SerializeField] private AudioSource multiplierAudioSource;  
    [SerializeField] private AudioSource perfectAudioSource;
    [SerializeField] private CanvasGroup perfectCanvasGroup;

    [SerializeField] private Database[] musics;
    public Database[] Musics { get { return musics; } }

    private int actualMusicIndex = 0;
    public int ActualMusicIndex { get { return actualMusicIndex; } }


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

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI comboText;


    public UnityEvent OnBeat;
    private int nextBeat = 0;


    private void Start()
    {
#if UNITY_EDITOR
        CheckMusicDatabases();
#endif

        BuildMusicChoiceDropdown();

        FindAndStartMenuMusic();

        SetAnimatedObjectsBPM();
    }


    private void FindAndStartMenuMusic()
    {
        for (int i = 0; i < musics.Length; i++)
        {
            if (musics[i].musicDatas.menuMusic)
            {
                musicAudioSource.clip = musics[i].musicDatas.audioClip;
                return;
            }
        }
    }



    private void BuildMusicChoiceDropdown()
    {
        musicChoiceDropdown.ClearOptions();

        // Crée une liste d'options à partir du tableau de musiques.
        var options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < musics.Length; i++)
        {
            if (!musics[i].musicDatas.menuMusic)
                options.Add(new TMP_Dropdown.OptionData(musics[i].musicDatas.audioClip.name));
        }

        // Ajoute les options au Dropdown.
        musicChoiceDropdown.AddOptions(options);
    }


    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // On se cale sur le rythme du morceau. Chaque unité de musicTime vaut non pas une seconde mais un écart entre deux beats.
        // Et on tient compte du délai initial, pour que les spawns se calent bien sur les beats de la musique :
        float musicTime = (musicAudioSource.time * musics[actualMusicIndex].musicDatas.Bpm / 60) - musics[actualMusicIndex].musicDatas.firstBpmDelay;

        if (musicTime >= nextBeat)
        {
            nextBeat++;
            OnBeat.Invoke();
        }

        if (!lastObjectSpawned && !gamePaused && !GameOver)
        {
            if (musics[actualMusicIndex].ObjectSpawns.Length > 0 && musicTime >= musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].SpawnBeat)
            {
                print(musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].elementType);
                spawner.SpawnObject(musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].elementType, musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].Lane);

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

        PlayMultiplierSoundWithCustonPitch(multiplier);
        comboText.text = "X" + multiplier;

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



    private void PlayMultiplierSoundWithCustonPitch(int mult)
    {
        multiplierAudioSource.pitch = 0.5f + (mult * 0.15f);
        multiplierAudioSource.Play();
    }



    public void UpdateScore(int pointsToAdd, bool isPerfect)
    {
        score += pointsToAdd * multiplier;
        scoreText.text = score + " points";

        if (isPerfect)
        {
            perfectAudioSource.Play();
            StartCoroutine(PerfectCanvasGroupFadeOut(2));            
        }
    }


    private IEnumerator PerfectCanvasGroupFadeOut(float duration)
    {
        float elapsedTime = 0.0f;
        float alpha = 1;

        while (alpha >= 0.0f)
        {
            alpha = 1 - (elapsedTime / duration);
            perfectCanvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

  
    public bool TogglePause()
    {
        gamePaused = !gamePaused;

        if (gamePaused) { musicAudioSource?.Pause(); }
        else
        {
            if (musicAudioSource.clip != musics[actualMusicIndex].musicDatas.audioClip)
                musicAudioSource.clip = musics[actualMusicIndex].musicDatas.audioClip;

            musicAudioSource?.Play();
        }

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
        if (animationOnBPMObject.Length > 0)
        {
            for (int i = 0; i < animationOnBPMObject.Length; i++)
            {
                animationOnBPMObject[i].SetAnimatorBPM(musics[actualMusicIndex].musicDatas.Bpm, musics[actualMusicIndex].musicDatas.firstBpmDelay);
            }
        }
    }



    private void CheckMusicDatabases()
    {
        for (int i = 0; i < musics.Length; i++)
        {
            if (!musics[i].musicDatas.menuMusic)
            {
                if (musics[i].musicDatas.Bpm == 0)
                {
                    Debug.LogError("Le BPM de la musique " + musics[i].musicDatas.audioClip.name + " n'a pas été renseigné !");
                }

                for (int j = 0; j < musics[i].ObjectSpawns.Length; j++)
                {
                    if (j > 0 && musics[i].ObjectSpawns[j].SpawnBeat < musics[i].ObjectSpawns[j - 1].SpawnBeat)
                    {
                        Debug.LogError("L'objet " + j + " de la musique " + musics[i].musicDatas.audioClip.name + " a un spawn beat inférieur à l'objet qui le précède dans leur base de données. Les deux objets doivent être intervertis.");
                    }

                    int originalLane = musics[i].ObjectSpawns[j].Lane;
                    musics[i].ObjectSpawns[j].Lane = Mathf.Clamp(musics[i].ObjectSpawns[j].Lane, 1, 3);

                    if (originalLane != musics[i].ObjectSpawns[j].Lane)
                    {
                        Debug.Log("L'objet numero " + j + " de la musique " + musics[i].musicDatas.audioClip.name + " n'a pas de piste assignée. Il a été placé par défaut sur la piste 1.");
                    }
                }
            }
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
