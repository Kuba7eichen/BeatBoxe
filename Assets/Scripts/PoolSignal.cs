using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSignal : MonoBehaviour
{

    public Spawner spawner;

    private void Start()
    {
        // Méthode 1
        //spawner = GameObject.FindObjectOfType<Spawner>();
    }

    private void OnDisable()
    {
        spawner.AddToPool(gameObject);
    }
}
