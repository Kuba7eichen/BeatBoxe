using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingElement : MonoBehaviour
{
    public enum ElementType
    {
        LEFTJAB,
        RIGHTJAB,
        LEFTUPPERCUT,
        RIGHTUPPERCUT,
        LEFTHOOK,
        RIGHTHOOK,
        TODODGE,
        TOPARRY
    }
    [NonSerialized]
    public float _speed;

    public ElementType _type;

    [NonSerialized]
    public float HitPrecisionTreshold;
    [NonSerialized]
    public float ParryPrecisionTreshold;
    [NonSerialized]
    public float MaxScore;

    protected GameManager _gameManager;
    protected LayerMask _playerMask;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        _playerMask = _gameManager.PlayerMask;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += (_speed * Time.deltaTime) * Vector3.back;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //print("entered a trigger: " + other);
        if(other.CompareTag("Bounds"))
        {
            gameObject.SetActive(false);
        }
    }

    protected abstract int CalculateScore(Collider other, int positionToCheckIndex);
   
    
}
