using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToParryElement : MovingElement
{

    private float _maxBlockingAngle;
    private float _maxBlockingDistance;



    private Transform _head, _leftHand, _rightHand;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _head = _gameManager.Head;
        _leftHand = _gameManager.LeftHand;
        _rightHand = _gameManager.RightHand;
        _type = ElementType.TOPARRY;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (CheckIfBlocking(other) && (_playerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Hit with Layermask and blocking");
            _gameManager.UpdateScore(CalculateScore(other, 0),false); // checking on the x axis and a parry cannot be timed
            _gameManager.UpdateMultiplier(1);
        }
        else
        {
            Debug.Log("Player not blocking on contact or missed");
            _gameManager.UpdateMultiplier(-1);
        }
        gameObject.SetActive(false);

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
    protected override int CalculateScore(Collider other, int positionToCheckIndex)
    {
        SphereCollider fist, target;
        fist = (SphereCollider)other;
        target = GetComponent<SphereCollider>();
        
        if (Mathf.Abs(other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex]) < ParryPrecisionTreshold)
        {
            return (int)(MaxScore);
        }
        else
        {
            return (int)(MaxScore * ((other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex]) / (fist.radius + target.radius))); // multipling the max score by the distance between colliders divided by the minimum distance that they can be in accross the given axis. The distance being always bigger, it makes the score lower the less "precise" the given parry is
        }
    }
}
