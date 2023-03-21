using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused { get; private set; }

    public int score = 0;

    public int multiplier = 1;
    
    
    [SerializeField] private LayerMask playerMask;
    public LayerMask PlayerMask { get { return playerMask; } }

    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;


    public Transform Head { get { return head; } }
    public Transform LeftHand { get { return leftHand; } }
    public Transform RightHand { get { return rightHand; } }


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
