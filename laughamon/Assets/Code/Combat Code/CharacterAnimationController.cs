using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationKey
{
    Idle
}

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    //Set the default state in this function
    public void Init()
    {

    }

    public void PlayAnimation(AnimationKey key)
    {

    }
}
