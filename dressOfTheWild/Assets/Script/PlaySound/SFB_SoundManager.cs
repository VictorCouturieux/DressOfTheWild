using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Created by COUTURIEUX VICTOR
/// The main class SoundManager
/// instantiate once and automatically in the GameManager 
/// </summary>
/// <remarks>
/// can get all instant of all specific manager
/// and can be call in all unity scene because GameManager is a singleton instance
/// </remarks>
/// <remarks>
/// contain once AudioSource to play music
/// to player different sound effect, drag her AudioSource reference in 'PlaySoundFX' function
/// </remarks>
/// <remarks>
/// all sound is play in local scene of the game not in network
/// </remarks>
public class SFB_SoundManager : MonoBehaviour {
    /// <value>
    /// Audio Source Music player
    /// once music played in all scene
    /// Drag a reference to the audio source which will play the music.
    /// </value>
    public AudioSource musicSource; //Drag a reference to the audio source which will play the music.

    /// <value>
    /// music library
    /// </value>
    public AudioClip HappyMusic;
    public AudioClip SadMusic;

    /// <value>
    /// sound effect library
    /// </value>
    public AudioClip ExplosionWithSound;
    public AudioClip MenirPlacement;
    public AudioClip[] RabbitSteps;
    public AudioClip Win;


    /// <value>
    /// for the crossfade for the two music
    /// </value>
    [Range(0, 3)]
    public float volumeMusic = 0.5f;
    [Range(0, 1)]
    public float fadeValue = 0f;
    public AnimationCurve fade;
    public AudioSource _sadHappy;
    public AudioSource _happyHappy;

    /// <value>
    /// Sound Effect enumeration can be call in 'PlaySoundFX' function to play related sound effect
    /// </value>
    public enum SoundFX {
        ExplosionWithSound,
        MenirPlacement,
        RabbitSteps,
        HappyMusic,
        SadMusic,
        Win
    }


    ///<summary>
    /// Awake is always called before any Start functions
    /// </summary>
    void Awake() {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    /// <summary>
    /// this is to call when the GameManager changes Unity scene.
    /// </summary>
    /// <param name="scene">scene to load</param>
    /// <param name="mode">load Scene mode (not used)</param>
    /// <remarks>Have specify instruction in all different scene</remarks>
    /// <remarks>
    /// duplicate code but it is explicit if we must make a particular change in a particular scene
    /// </remarks>
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
//        Debug.Log("Log Sound Manager");

        //by default the Music is playing in loop
        musicSource.loop = true;
        if (musicSource.clip != HappyMusic) {
            musicSource.clip = HappyMusic;
            musicSource.Play();
        }
    }

    // Start is called before the first frame  (not used)
    void Start() {
    }

    // Update is called before the first frame  (not used)
    void Update()
    {
        Crossfade();
    }


    /// <summary>
    /// crossfade between the sad music and the happy one
    /// </summary>
    public void Crossfade()
    {
        //Cross fade
        if (_happyHappy == null || _sadHappy == null)
            return;

        float value = fadeValue;
        _happyHappy.volume = fade.Evaluate(value) * volumeMusic;
        _sadHappy.volume = fade.Evaluate(1 - value) * volumeMusic;
    }

    /// <summary>
    /// can be call in all unity scene to play specific sound effect
    /// </summary>
    /// <param name="efxSource">specific AudioSource reference</param>
    /// <param name="soundF">specific sound effect reference play on this first param AudioSource reference</param>
    public void PlaySoundFX(AudioSource efxSource, SoundFX soundF) {
        // select specific sound effect according to sound effect reference
        AudioClip[] clips = null;
        
        switch (soundF) {
            case SoundFX.ExplosionWithSound:
                clips = new[] {ExplosionWithSound};
                break;
            case SoundFX.MenirPlacement:
                clips = new[] {MenirPlacement};
                break;
            case SoundFX.RabbitSteps:
                clips = RabbitSteps;
                break;
            case SoundFX.HappyMusic:
                clips = new[] {HappyMusic};
                break;
            case SoundFX.SadMusic:
                clips = new[] {SadMusic};
                break;
            case SoundFX.Win:
                clips = new[] {Win};
                break;
            default:
                Console.WriteLine("NO SOUND EFFECT FOR THIS");
                break;
        }

        if (clips != null) {
            //player random sound effect in library of the sound effect reference
            int randomIndex = Random.Range(0, clips.Length);
            efxSource.clip = clips[randomIndex];
            efxSource.loop = false;
            efxSource.Play();
        }
    }
}