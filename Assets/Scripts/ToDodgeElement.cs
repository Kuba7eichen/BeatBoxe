using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDodgeElement : MovingElement
{

    private float _maxBlockingAngle;
    private float _maxBlockingDistance;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    new private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if ((playerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Hit with Layermask");
        }
    }   
}
