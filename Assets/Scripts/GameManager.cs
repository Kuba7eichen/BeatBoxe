using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused { get; private set; }

    public int score = 0;

    public int multiplier = 1;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

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
    }


    void Start()
    {

    }


    void Update()
    {

    }
}
