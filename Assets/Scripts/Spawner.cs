using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovingElement;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] prefabsToSpawn;

    [SerializeField] private int nbOfEachPrefabToInstantiate = 8;


    private List<GameObject> disabled_Jab_Right = new List<GameObject>();
    private List<GameObject> disabled_Jab_Left = new List<GameObject>();

    private List<GameObject> disabled_Uppercut_Right = new List<GameObject>();
    private List<GameObject> disabled_Uppercut_Left = new List<GameObject>();

    private List<GameObject> disabled_Hook_Right = new List<GameObject>();
    private List<GameObject> disabled_Hook_Left = new List<GameObject>();


    private List<GameObject> disabled_To_Dogde = new List<GameObject>();
    private List<GameObject> disabled_To_Parry = new List<GameObject>();

    private void Start()
    {
        SpawnObjectsForPool(nbOfEachPrefabToInstantiate);
    }


    void SpawnObjectsForPool(int numberOfCubes)
    {
        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            for (int j = 0; j < nbOfEachPrefabToInstantiate; j++)
            {
                GameObject newObject = InstantiateObject(prefabsToSpawn[i]);


            }
        }


    }

    private GameObject InstantiateObject(GameObject prefabToInstantiate)
    {
        GameObject monObjet = Instantiate(prefabToInstantiate);
        monObjet.GetComponent<PoolSignal>().spawner = this;
        monObjet.SetActive(false);
        return monObjet;
    }

    //private Vector3 ComputeRandomPosition()
    //{


    //    // & ici on fait en sorte que ça spawn dans le champ vision de la caméra
    //    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(positionX, positionY, positionZ));
    //    return worldPosition;
    //}

    // Cette fonction est appelée par PoolSignal lorsque le cube est désactivé
    public void AddToPool(GameObject objectToAdd)
    {
        MovingElement movingElement = GetComponent<MovingElement>();
        switch (movingElement.type)
        {
            case ElementType.RIGHTJAB:
                disabled_Jab_Right.Add(objectToAdd);
                break;
            case ElementType.LEFTJAB:
                disabled_Jab_Left.Add(objectToAdd);
                break;
            case ElementType.RIGHTUPPERCUT:
                disabled_Uppercut_Right.Add(objectToAdd);
                break;
            case ElementType.LEFTUPPERCUT:
                disabled_Uppercut_Left.Add(objectToAdd);
                break;
            case ElementType.RIGHTHOOK:
                disabled_Hook_Right.Add(objectToAdd);
                break;
            case ElementType.LEFTHOOK:
                disabled_Hook_Left.Add(objectToAdd);
                break;
            case ElementType.TODODGE:
                disabled_To_Dogde.Add(objectToAdd);
                break;
            case ElementType.TOPARRY:
                disabled_To_Parry.Add(objectToAdd);
                break;
        }





    }
}
