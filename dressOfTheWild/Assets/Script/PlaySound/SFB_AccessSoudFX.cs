using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by COUTURIEUX VICTOR
/// used player sound effect when you click on the button menu to go on a other menu
/// </summary>
/// <remarks>
/// can be optimised with the SFB_BackSoundFX
/// created in a very short time
/// </remarks>
public class SFB_AccessSoudFX : MonoBehaviour {
    /// <value>Audio source reference</value>
    public AudioSource efxSource;
    public SFB_SoundManager.SoundFX soundFX;

    /// <summary>
    /// play the sound effect relative to access of to another menu
    /// </summary>
    public void PlayAccessSoundFX() {
        GameManager.instance.SoundManager.PlaySoundFX(efxSource, soundFX);
    }
}