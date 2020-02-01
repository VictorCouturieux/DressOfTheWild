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

    private Transform visual;
    private Camera mainCamera;
    
    private Vector3 moveDirection = Vector3.zero;
    CharacterController Cc;
    PaintingGround paint;

    // Start is called before the first frame update
    void Start() {
        Cc = GetComponent<CharacterController>();
        paint = GetComponent<PaintingGround>();
        visual = transform.GetChild(0);
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {

        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }
        
        prevState = state;
        state = GamePad.GetState(playerIndex);
        
        moveDirection = new Vector3(state.ThumbSticks.Left.X,state.ThumbSticks.Left.Y,0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        
        transform.GetChild(0).localRotation = new Quaternion(
            mainCamera.transform.rotation.x,
            mainCamera.transform.rotation.y,
            mainCamera.transform.rotation.z,
             transform.rotation.w);
        
        Cc.Move(moveDirection * Time.deltaTime);

        paint.RaycastGround();
    }

    void OnGUI()
    {
        string text = "Use left stick to turn the cube, hold A to change color\n";
        text += string.Format("IsConnected {0} Packet #{1}\n", state.IsConnected, state.PacketNumber);
        text += string.Format("\tTriggers {0} {1}\n", state.Triggers.Left, state.Triggers.Right);
        text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
        text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state.Buttons.Start, state.Buttons.Back, state.Buttons.Guide);
        text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
        text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
        text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);
    }
    
}