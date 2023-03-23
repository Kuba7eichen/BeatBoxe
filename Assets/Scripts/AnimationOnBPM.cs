using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationOnBPM : MonoBehaviour
{
    private Animator animator;

    [SerializeField] float timeOffset = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void SetAnimatorBPM(float bpm, float initialDelay)
    {        
        StartCoroutine(ApplyInitialDelay(bpm, initialDelay));
    }

    private IEnumerator ApplyInitialDelay(float bpm, float initialDelay)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(initialDelay + timeOffset);
        animator.speed = bpm / 60;
    }
}
