using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public AudioSource efxSource;
    
    public void Jump()
    {
        GameManager.instance.SoundManager.PlaySoundFX(efxSource, SFB_SoundManager.SoundFX.RabbitSteps);
    }
}
