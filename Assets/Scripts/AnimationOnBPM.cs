using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationOnBPM : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] float timeOffset = 0;

    private float musicDelay = 0;

    private float BPM = 0;

    private bool musicBegun = false;


    void Update()
    {
        if (GameManager.Instance.GamePaused)
        {
            animator.speed = 0;
        }
        else
        {
            if (musicBegun) // Si la musique a déjà commencé (càd si on sort du menu pause, et non pas du main menu), pas besoin d'appliquer à nouveau le time offset.
            {
                animator.speed = BPM;
            }
            else
            {
                StartCoroutine(ApplyTimeOffset());
            }
        }
    }


    IEnumerator ApplyTimeOffset()
    {
        yield return new WaitForSeconds(timeOffset + musicDelay);
        animator.speed = BPM;
        musicBegun = true;
    }



    public void SetAnimatorBPM(float bpm, float delay)
    {
        BPM = bpm / 60;
        animator.speed = BPM;
        musicBegun = false;
        musicDelay = delay;
    }
}
