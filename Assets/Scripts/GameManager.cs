using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused { get; private set; }

    public int score = 0;

    public int multiplier = 1;

    [SerializeField] Transform head;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static GameManager GameManagerInstance()
    {
        if (instance != null) return instance;
        else
        {
            instance = new GameManager();
            return instance;
        }
    }

    void Start()
    {

    }


    void Update()
    {

    }
}
