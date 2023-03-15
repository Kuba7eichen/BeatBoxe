using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSignal : MonoBehaviour
{

    public Spawner spawner;

    private MovingElement movingElement;

    private void Start()
    {        
        spawner = FindObjectOfType<Spawner>();

        movingElement = GetComponent<MovingElement>();
    }

    private void OnDisable()
    {
        spawner.AddToPool(movingElement);
    }
}
