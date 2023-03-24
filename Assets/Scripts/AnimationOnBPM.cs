using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationOnBPM : MonoBehaviour
{
    private Animator animator;

    [SerializeField] float timeOffset = 0;

    private float BPM = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.Instance.GamePaused)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = BPM;
        }
    }


    public void SetAnimatorBPM(float bpm, float initialDelay)
    {
        StartCoroutine(ApplyInitialDelay(bpm, initialDelay));
    }

    private IEnumerator ApplyInitialDelay(float bpm, float initialDelay)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(initialDelay + timeOffset);
        BPM = bpm / 60;
        animator.speed = BPM;
    }
}
