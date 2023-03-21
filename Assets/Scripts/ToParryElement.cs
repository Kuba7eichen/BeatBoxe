using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToParryElement : MovingElement
{
    private bool isBlockCorrect = false;

    private GameManager gameManager;
    private float maxBlockingAngle;
    private float maxBlockingDistance;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
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
            if (Mathf.Abs(Vector3.Angle(Vector3.forward, other.transform.position - head.position)) < maxBlockingAngle)
            {
                if(Mathf.Abs(Vector3.Angle(Vector3.right, righttHand.position - leftHand.position)) < maxBlockingAngle && Vector3.Distance(righttHand.transform.position, leftHand.transfrom.position < maxBlockingDistance))
                {
                    return true;
                }
            }

        }
        return false;

    }
}