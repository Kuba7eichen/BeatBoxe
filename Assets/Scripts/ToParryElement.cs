using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToParryElement : MovingElement
{
    private bool isBlockCorrect = false;

    private float _maxBlockingAngle;
    private float _maxBlockingDistance;



    private Transform _head, _leftHand, _rightHand;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _head = _gameManager.Head;
        _leftHand = _gameManager.LeftHand;
        _rightHand = _gameManager.RightHand;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    new private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (CheckIfBlocking(other) && (playerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Hit with Layermask");
        }

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
