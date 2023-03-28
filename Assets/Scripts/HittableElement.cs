using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableElement : MovingElement
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //print("entered a collision: " + other);
        base.OnTriggerEnter(other);
        if ((_playerMask.value & (1 << other.transform.gameObject.layer)) > 0) // check if collision with player
        {
            if (CheckedAttackDirection(other, ElementTypeToPositionIndex(_type)) && CheckedAttackHand(other)) // attack is correct only if the correct hand was used AND the correct type of attack was performed
            {
                bool isPerfect = false; //is perfect is true when the timing is perfect
                Debug.Log("Good attack type detected");
                _gameManager.UpdateScore(CalculateScore(other, ElementTypeToPositionIndex(_type)),isPerfect);
                _gameManager.UpdateMultiplier(isPerfect? 2:1);
            }
            else
            {
                Debug.Log("Attack detected, but not the right one");
                _gameManager.UpdateMultiplier(-1);
            }
        }
        else
        {
            Debug.Log("Missed the target");
            _gameManager.UpdateMultiplier(-1);
        }
        gameObject.SetActive(false);

    }
    //Checks from which side the attack came, thus telling weather it was the correct attack
    private bool CheckedAttackDirection(Collider other, int positionToCheckIndex)
    {
        return (Mathf.Abs(other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex]) > Mathf.Abs(other.transform.position[(positionToCheckIndex + 1) % 3] - transform.position[(positionToCheckIndex + 1) % 3])
                && Mathf.Abs(other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex]) > Mathf.Abs(other.transform.position[(positionToCheckIndex + 2) % 3] - transform.position[(positionToCheckIndex + 2) % 3]));
    }

    //Checks if the correct hand was used to attack the target
    private bool CheckedAttackHand(Collider other)
    {
        switch (_type)
        {
            case ElementType.LEFTHOOK: return other.gameObject.layer == 8; //LeftHandLayer
            case ElementType.RIGHTHOOK: return other.gameObject.layer == 9; //RightHandLayer
            case ElementType.LEFTJAB: return other.gameObject.layer == 8; //LeftHandLayer
            case ElementType.RIGHTJAB: return other.gameObject.layer == 9; //RightHandLayer
            case ElementType.LEFTUPPERCUT: return other.gameObject.layer == 8; //LeftHandLayer
            case ElementType.RIGHTUPPERCUT: return other.gameObject.layer == 9; //RightHandLayer
            default:
                Debug.LogError("No index associated to type or not a hittable type");
                return false;

        }
    }

    //Associates the axis to check for each type of attack
    private int ElementTypeToPositionIndex(ElementType type)
    {
        switch (type)
        {
            case ElementType.LEFTHOOK: return 0; //x value
            case ElementType.RIGHTHOOK: return 0;
            case ElementType.LEFTJAB: return 2; //z value
            case ElementType.RIGHTJAB: return 2;
            case ElementType.LEFTUPPERCUT: return 1; //y value
            case ElementType.RIGHTUPPERCUT: return 1;
            default:
                Debug.LogError("No index associated to type or not a hittable type");
                return 0;

        }
    }

    protected override int CalculateScore(Collider other, int positionToCheckIndex)
    {
        SphereCollider fist, target;
        fist = (SphereCollider) other;
        target = GetComponent<SphereCollider>();
        Vector3 controllerSpeed;
        if (other.gameObject.layer == 9)
            controllerSpeed = ControllerVelocity.GetControllerVelocity(true);
        else
            controllerSpeed = ControllerVelocity.GetControllerVelocity(false);

        if(Mathf.Abs(other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex])< HitPrecisionTreshold)
        {
            return (int) (MaxScore * controllerSpeed.magnitude);
        }
        else
        {
            return (int) (MaxScore * controllerSpeed.magnitude * ((other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex]) / (fist.radius + target.radius))); // multipling the max score by the distance between colliders divided by the minimum distance that they can be in accross the given axis. The distance being always bigger, it makes the score lower the less "precise" the given hit is
        }
    }
}
