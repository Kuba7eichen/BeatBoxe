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

    [SerializeField] private float _speed;

    [SerializeField] public ElementType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (_speed * Time.deltaTime) * Vector3.back;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bounds"))
        {
            gameObject.SetActive(false);
        }
    }
    
}
