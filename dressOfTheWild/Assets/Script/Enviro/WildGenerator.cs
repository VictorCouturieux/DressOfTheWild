﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WildGenerator : MonoBehaviour {
    public GameObject HappyPrefabs;
    public GameObject SadPrefabs;
    
    
    [HideInInspector] public bool IsHappy;

    // Start is called before the first frame update
    void Start() {
//        ToSad();
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
            DeleteAllChild(transform);
            Instantiate(HappyPrefabs, transform.position, Quaternion.identity, transform);
            GetComponent<Collider>().enabled = false;
            IsHappy = true;
            GenericElements.CountAllHappy++;
            /*Debug.Log(GenericElements.CountAllHappy 
                      + "/" + GenericElements.LenghtElemList 
                      + "=" + GenericElements.CountAllHappy/GenericElements.LenghtElemList);*/
        }
        else {
            Debug.LogError("NullPointerException : HerbePrefabs in GrassGenerator");
        }
    }

    private void ToSad() {
        if (SadPrefabs != null) {
            DeleteAllChild(transform);
            Instantiate(SadPrefabs, transform.position, Quaternion.identity, transform);
            GetComponent<Collider>().enabled = false;
            IsHappy = false;
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