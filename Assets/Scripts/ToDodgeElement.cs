using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDodgeElement : MovingElement
{

    private GameManager _gameManager;
    private float _maxBlockingAngle;
    private float _maxBlockingDistance;

    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        layerMask = _gameManager.PlayerMask;
    }

    // Update is called once per frame
    void Update()
    {

    }

    new private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Hit with Layermask");
        }
    }   
}
