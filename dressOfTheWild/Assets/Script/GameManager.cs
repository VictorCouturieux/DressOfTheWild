using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Created by COUTURIEUX VICTOR
/// The main class GameManager
/// instantiate automatically a game manager if it not exist
/// </summary>
/// <remarks>
/// can get all instant of all specific manager
/// and can be call in all unity scene because GameManager is a singleton instance
/// </remarks>
public class GameManager : MonoBehaviour {
    /// <value>Instance of GameManager</value>
    public static GameManager
        instance = null; //Static instance of GameManager which allows it to be accessed by any other script

    public static bool winGame = false;
    
    ///<value>SoundManager singleton manager</value> 
    [SerializeField] private SFB_SoundManager _soundManager; //SoundManager prefab to instantiate.

    public SFB_SoundManager SoundManager {
        get { return _soundManager; }
        set { _soundManager = value; }
    }
    
    ///<summary>
    /// Awake is always called before any Start functions
    /// </summary>
    void Awake() {
//        Debug.Log("init GameManager");
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (GenericElements.CountAllHappy != 0 && GenericElements.LenghtElemList != 0) {
            float result = (float) GenericElements.CountAllHappy / GenericElements.LenghtElemList;
            if ( result >= 0.8f && winGame == false) {
                winGame = true;
                Debug.Log("WIN");
                StartCoroutine("CinematicEndWin");
            }
        }
    }

    IEnumerator CinematicEndWin() {
        Debug.Log("Cinematique");
        yield return null;
    }

}