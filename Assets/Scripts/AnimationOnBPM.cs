using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOnBPM : MonoBehaviour
{
    [SerializeField] Animator animator;

    
    public void SetAnimatorBPM(float bpm)
    {
        animator.speed = bpm / 60;
    }
}
