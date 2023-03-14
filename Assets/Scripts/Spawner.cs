using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] prefabsToSpawn;

    [SerializeField] private int nbOfEachPrefabToInstantiate = 8;


    private List<GameObject> disabled_Jab_Head_Right = new List<GameObject>();
    private List<GameObject> disabled_Jab_Head_Left = new List<GameObject>();

    private List<GameObject> disabled_Uppercut_Head_Right = new List<GameObject>();
    private List<GameObject> disabled_Uppercut_Head_Left = new List<GameObject>();

    private List<GameObject> disabled_Hook_Head_Left = new List<GameObject>();
    private List<GameObject> disabled_Hook_Head_Right = new List<GameObject>();


    private List<GameObject> disabled_To_Dogde_Head = new List<GameObject>();
    private List<GameObject> disabled_To_Parry_Head = new List<GameObject>();

    


    void SpawnObjectsForPool(int numberOfCubes)
    {
        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            for (int j = 0; j < nbOfEachPrefabToInstantiate; j++)
            {
                InstantiateObject(prefabsToSpawn[i]);
            }
        }



        // pour faire spawn 5 items 
        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject monObjet = InstantiateObject();

                    

            //on désactive l'objet
          
        }
    }

    private void InstantiateObject(GameObject prefabToInstantiate)
    {    
        GameObject monObjet = Instantiate(prefabToInstantiate);
        monObjet.GetComponent<PoolSignal>().spawner = this;
        monObjet.SetActive(false);

    }

    private Vector3 ComputeRandomPosition()
    {
    

        // & ici on fais en sorte que ça spawn dans le champ vision de la caméra
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(positionX, positionY, positionZ));
        return worldPosition;
    }

    // Cette fonction est appelée par PoolSignal lorsque le cube est désactivé
    public void AddToPool(GameObject objectToAdd)
    {
        disabledCubePool.Add(objectToAdd);
    }
}
