using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour {
    public GameObject HerbePrefabs;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter(Collider autre) {
        if (autre.gameObject.name.Equals("Visual")) {
            if (HerbePrefabs != null) {
                Instantiate(HerbePrefabs, transform.position, Quaternion.identity, transform);
                GetComponent<Collider>().enabled = false;
            }
            else {
                Debug.LogError("NullPointerException : HerbePrefabs in GrassGenerator");
            }
        }
    }
}