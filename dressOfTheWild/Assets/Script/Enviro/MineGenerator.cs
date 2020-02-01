using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGenerator : MonoBehaviour {
    public float explosionForce = 600.0f;
    public float explosionRadius = 3.25f;
    
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Visual")) {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null) {
                Debug.Log("Explsion!!!!!!!!!!");
                rb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius
                );
            }
        }
        
    }

    
}