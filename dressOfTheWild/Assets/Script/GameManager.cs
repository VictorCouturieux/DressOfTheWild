using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    //This is Main Camera in the Scene
    private Camera m_MainCamera;
    public Image victoryImage;
    public Image fadeImage;
    public AnimationCurve CinematicCurve;
    public float timerStartFadeout = 10f;

    public List<MeshFilter> allMeshFilterGround = new List<MeshFilter>();

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

        m_MainCamera = Camera.main;
        victoryImage.color = Color.clear;
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (GenericElements.CountAllHappy != 0 && GenericElements.LenghtElemList != 0) {
            float result = (float) GenericElements.CountAllHappy / GenericElements.LenghtElemList;
            if (result >= 0.75f && winGame == false) {
                winGame = true;
//                Debug.Log("WIN");
                StartCoroutine("CinematicEndWin");
                StartCoroutine(VictoryImageFadeTo(1.0f, 5.0f));
            }
        }
    }

    IEnumerator CinematicEndWin() {
//        float normalizedTime = 0;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / timerStartFadeout) {
            m_MainCamera.transform.Translate(Vector3.back * CinematicCurve.Evaluate(t));

            yield return null;
        }

        float start = Time.time;
        float duration = 1;
        float elapsed = 0;
        while (elapsed < duration) {
            // calculate how far through we are
            elapsed = Time.time - start;
            float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
            fadeImage.color = Color.Lerp(Color.clear, Color.black, normalisedTime);
            // wait for the next frame
            yield return null;
        }
        
        Debug.Log("shutdown");
        
        
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

    }
    
    IEnumerator VictoryImageFadeTo(float aValue, float aTime) {
//        Debug.Log("Victory");
        float alpha = victoryImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            victoryImage.color = newColor;
            yield return null;
        }
    }
}