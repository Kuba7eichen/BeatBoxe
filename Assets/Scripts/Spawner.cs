using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovingElement;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private Transform player;

    [SerializeField] private int nbOfEachPrefabToInstantiate = 8;

    [SerializeField] private float objectsSpeed = 2;

    [Tooltip("This time is in beats, not in seconds.")]
    [SerializeField] private float apparitionTimeBeforeReachingPlayerPosition = 4;

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



    public void InitialiseSpawnerPosition()
    {
        // On place le spawner en fonction de la vitesse des objets à spawner, du BPM de la musique et du temps (en nombre de beats)
        // avant qu'ils arrivent à la position du joueur :
        float newZPosition = player.position.z + ((apparitionTimeBeforeReachingPlayerPosition * objectsSpeed) /
                                                  (GameManager.Instance.Musics[GameManager.Instance.ActualMusicIndex].musicDatas.Bpm / 60));

        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
    }


    // Initialize the pool at the beginning of the execution
    private void InstantiateObjectsForPool(int number)
    {
        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            for (int j = 0; j < number; j++)
            {
                InstantiateObject(prefabsToSpawn[i]);
            }
        }
    }


    private GameObject InstantiateObject(GameObject prefabToInstantiate)
    {
        GameObject monObjet = Instantiate(prefabToInstantiate);
        monObjet.GetComponent<PoolSignal>().spawner = this;

        monObjet.GetComponent<MovingElement>()._speed = objectsSpeed;
        monObjet.SetActive(false);
        return monObjet;
    }


    // Make a Moving element object of a certain type
    private MovingElement SpawnFromStack(Stack<MovingElement> Stack)
    {
        if (Stack.Count == 0)
        {
            Debug.Log("Stack empty. New object instantiated.");
            InstantiateObject(EnumToGameObject(StackToEnum(Stack)));
        }

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
        EnumToStack(objectToAdd._type).Push(objectToAdd);
    }


    public MovingElement SpawnObject(ElementType type, int lane)
    {
        MovingElement objectToSpawn = SpawnFromStack(EnumToStack(type));
        objectToSpawn.transform.position = spawnPoints[lane - 1].position;
        return objectToSpawn;
    }




    // Used to instantiate an object during the game, if the stack is empty.
    private ElementType StackToEnum(Stack<MovingElement> stack)
    {
        if (stack == disabled_Jab_Right) return ElementType.RIGHTJAB;
        else if (stack == disabled_Jab_Left) return ElementType.LEFTJAB;
        else if (stack == disabled_Uppercut_Right) return ElementType.RIGHTUPPERCUT;
        else if (stack == disabled_Uppercut_Left) return ElementType.LEFTUPPERCUT;
        else if (stack == disabled_Hook_Right) return ElementType.RIGHTHOOK;
        else if (stack == disabled_Hook_Left) return ElementType.LEFTHOOK;
        else if (stack == disabled_To_Dogde) return ElementType.TODODGE;
        else return ElementType.TOPARRY;
    }

    // Used to instantiate an object during the game, if the stack is empty.
    private GameObject EnumToGameObject(ElementType elementType)
    {
        GameObject objToReturn = null;
        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {
            if (prefabsToSpawn[i].GetComponent<MovingElement>()._type == elementType)
            {
                objToReturn = prefabsToSpawn[i];
                break;
            }
        }
        return objToReturn;
    }
}
