﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGenerator : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("Visual")) {
            CharactereMove characMove = other.GetComponentInParent<CharactereMove>();

            characMove.Explosion(this.transform.position);
            //need to redraw the ground !

        //move around the flower

            //DeactivateHimself();
        }
        
    }

    
}