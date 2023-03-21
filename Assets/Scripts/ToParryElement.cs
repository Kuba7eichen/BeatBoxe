using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToParryElement : MovingElement
{
    private bool isBlockCorrect = false;

    private GameManager _gameManager;
    private float _maxBlockingAngle;
    private float _maxBlockingDistance;

    private Transform _head, _leftHand, _rightHand;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _head = _gameManager.head;
        _leftHand = _gameManager.leftHand;
        _rightHand = _gameManager.rightHand;
    }

    // Update is called once per frame
    void Update()
    {

    }

    new private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        isBlockCorrect = CheckIfBlocking(other);


    }
    //Checks from which side the attack came, thus telling weather it was the correct attack
    private bool CheckIfBlocking(Collider other)
    {
        if (other)// is a hand
        {
            if (Mathf.Abs(Vector3.Angle(Vector3.forward, other.transform.position - _head.position)) < _maxBlockingAngle)
            {
                if(Mathf.Abs(Vector3.Angle(Vector3.right, _rightHand.position - _leftHand.position)) < _maxBlockingAngle && Vector3.Distance(_rightHand.position, _leftHand.position) < _maxBlockingDistance)
                {
                    return true;
                }
            }

        }
        return false;

    }
}
