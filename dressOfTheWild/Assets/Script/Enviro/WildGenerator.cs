using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WildGenerator : MonoBehaviour {
    public GameObject HappyPrefabs;
    public GameObject SadPrefabs;
    
    
    [HideInInspector] public bool IsHappy;
    private bool OnStart = true;
    public GameObject curentPrafab = null;

    // Start is called before the first frame update
    void Start() {
//        int rand = Random.Range(0, 100);
//        if (rand<15) {
//            ToSad();
//        }
        OnStart = false;
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter(Collider autre) {
        if (autre.gameObject.name.Equals("CharaVisual")) {
            ToHappy();
        }
    }

    private void ToHappy() {
        if (HappyPrefabs != null) {
//            if (curentPrafab!=null) {
//                Debug.Log("Destroy IT");
//                Destroy(curentPrafab);
//                curentPrafab = null;
//            }
//            curentPrafab = 
                Instantiate(HappyPrefabs, transform.position, Quaternion.identity, transform);
            GetComponent<Collider>().enabled = false;
            IsHappy = true;
            if (!OnStart)
                GenericElements.CountAllHappy++;
            Debug.Log(GenericElements.CountAllHappy 
                      + "/" + GenericElements.LenghtElemList 
                      + "=" + GenericElements.CountAllHappy/GenericElements.LenghtElemList);
        }
        else {
            Debug.LogError("NullPointerException : HerbePrefabs in GrassGenerator");
        }
    }

    private void ToSad() {
        if (SadPrefabs != null) {
//            if (curentPrafab!=null) {
//                Destroy(curentPrafab);
//                curentPrafab = null;
//            }
//            curentPrafab = 
                Instantiate(SadPrefabs, transform.position, Quaternion.identity, transform);
            GetComponent<Collider>().enabled = false;
            IsHappy = false;
            if (!OnStart)
                GenericElements.CountAllHappy--;
//            Debug.Log(GenericElements.CountAllHappy 
//                      + "/" + GenericElements.LenghtElemList 
//                      + "=" + GenericElements.CountAllHappy/GenericElements.LenghtElemList);
        }
        else {
            Debug.LogError("NullPointerException : HerbePrefabs in GrassGenerator");
        }
    }

    private void DeleteAllChild(Transform root) {
        Debug.Log(root.childCount);
        if (root.childCount>0) {
            if (root.GetChild(0).gameObject!=null) {
                Debug.Log("Destroy");
                Destroy(root.GetChild(0).gameObject);
            }
        }
    }
}