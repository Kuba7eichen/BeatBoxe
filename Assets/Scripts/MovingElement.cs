using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingElement : MonoBehaviour
{
    public enum ElementType
    {
        LEFTJAB,
        RIGHTJAB,
        LEFTUPPERCUT,
        RIGHTUPPERCUT,
        LEFTHOOK,
        RIGHTHOOK,
        TODODGE,
        TOPARRY
    }

    public float speed;

    [SerializeField] public ElementType type;

    protected GameManager _gameManager;
    protected LayerMask playerMask;


    // Start is called before the first frame update
    protected void Start()
    {
        _gameManager = GameManager.Instance;
        playerMask = _gameManager.PlayerMask;
    }

    // Update is called once per frame
    protected void Update()
    {
        transform.position += (speed * Time.deltaTime) * Vector3.back;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bounds"))
        {
            gameObject.SetActive(false);
        }
    }
    
}
