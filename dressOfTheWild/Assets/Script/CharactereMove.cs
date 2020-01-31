using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class CharactereMove : MonoBehaviour {
    public float speed = 6f;

    
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    
    
    private Vector3 moveDirection = Vector3.zero;
    CharacterController Cc;

    // Start is called before the first frame update
    void Start() {
        Cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        
        
        
        
    }
}