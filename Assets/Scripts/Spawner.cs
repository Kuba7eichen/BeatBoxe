using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovingElement;

public class Spawner : MonoBehaviour
{

    [SerializeField] private GameObject[] prefabsToSpawn;

    [SerializeField] private int nbOfEachPrefabToInstantiate = 8;


    private Stack<MovingElement> disabled_Jab_Right = new Stack<MovingElement>();
    private Stack<MovingElement> disabled_Jab_Left = new Stack<MovingElement>();

    private Stack<MovingElement> disabled_Uppercut_Right = new Stack<MovingElement>();
    private Stack<MovingElement> disabled_Uppercut_Left = new Stack<MovingElement>();

    private Stack<MovingElement> disabled_Hook_Right = new Stack<MovingElement>();
    private Stack<MovingElement> disabled_Hook_Left = new Stack<MovingElement>();


    private Stack<MovingElement> disabled_To_Dogde = new Stack<MovingElement>();
    private Stack<MovingElement> disabled_To_Parry = new Stack<MovingElement>();

    private void Start()
    {
        InstantiateObjectsForPool(nbOfEachPrefabToInstantiate);
    }

    // Initialize the pool at the beginning of the execution
    private void InstantiateObjectsForPool(int numberOfCubes)
    {
        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            for (int j = 0; j < nbOfEachPrefabToInstantiate; j++)
            {
                InstantiateObject(prefabsToSpawn[i]);
            }
        }
    }


    private GameObject InstantiateObject(GameObject prefabToInstantiate)
    {
        GameObject monObjet = Instantiate(prefabToInstantiate);
      //  monObjet.GetComponent<PoolSignal>().spawner = this;      Inutile car l'objet MovingElement cherche le spawner.
        monObjet.SetActive(false);
        return monObjet;
    }   


    // Make a Moving element object of a certain type
    private MovingElement SpawnFromStack(Stack<MovingElement> Stack)
    {
        MovingElement objectToSpawn = Stack.Pop();       
        objectToSpawn.gameObject.SetActive(true);
        return objectToSpawn;
    }


    // Returns the stack corresponding to the type of the MovingElement object
    private Stack<MovingElement> EnumToStack(ElementType @enum)
    {
        switch (@enum)
        {
            case ElementType.RIGHTJAB: return disabled_Jab_Right;
            case ElementType.LEFTJAB: return disabled_Jab_Left;
            case ElementType.RIGHTUPPERCUT: return disabled_Uppercut_Right;
            case ElementType.LEFTUPPERCUT: return disabled_Uppercut_Left;
            case ElementType.RIGHTHOOK: return disabled_Hook_Right;
            case ElementType.LEFTHOOK: return disabled_Hook_Left;
            case ElementType.TODODGE: return disabled_To_Dogde;
            case ElementType.TOPARRY: return disabled_To_Parry;
        }

        Debug.LogError("No Stack corresponding to the enum!");
        return null;
    }


    // This method is called by MovingElement when it is deactivated
    public void AddToPool(MovingElement objectToAdd)
    {
        EnumToStack(objectToAdd.type).Push(objectToAdd);
    }


    public MovingElement SpawnObject(ElementType type)
    {

        return SpawnFromStack(EnumToStack(type));

    }

}
