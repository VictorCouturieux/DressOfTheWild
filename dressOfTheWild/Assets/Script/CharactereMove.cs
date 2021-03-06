﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class CharactereMove : MonoBehaviour {
    
    public static CharactereMove
        instance = null;
    
    public float speed = 6f;
    private float speedNormal;
    [HideInInspector] public Vector3 lastMovement = Vector3.zero;
    [HideInInspector] public float lastSpeed = 0;


    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private Transform visual;
    private Camera mainCamera;

    private Vector3 moveDirection = Vector3.zero;
    CharacterController Cc;
    PaintingGround paint;
    Animator visualAnimator;
    public float slowDownAnimator = 1f;
    public AnimationCurve zoneSize;

    private Collider MineCollider;

    //Explosion
    public float explosionForce = 20f;
    public float explosionControllerSpeed = 0.3f;
    public float howLong = 1f;
    public AnimationCurve explosionCurve;
    public AnimationCurve explosionPPCurve;

    public UnityEngine.Rendering.Volume boomPostProcess;
    

    public Transform RotatePointTotem;
    public GameObject TotemPrefab;

    private GameObject CurrentTotem = null;

    void Awake() {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);        
    }

    // Start is called before the first frame update
    void Start() {
        Cc = GetComponent<CharacterController>();
        paint = GetComponent<PaintingGround>();
        visual = transform.GetChild(0);
        visualAnimator = GetComponentInChildren<Animator>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        speedNormal = speed;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();


        if (!playerIndexSet || !prevState.IsConnected) {
            for (int i = 0; i < 4; ++i) {
                PlayerIndex testPlayerIndex = (PlayerIndex) i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected) {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);

        moveDirection = new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, 0);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        float finalVelocity = moveDirection.magnitude;

        visualAnimator.SetFloat("MoveX",
            Mathf.Abs(state.ThumbSticks.Left.X) > 0.1f ? 1 * Mathf.Sign(state.ThumbSticks.Left.X) : 0);
        visualAnimator.SetFloat("MoveY",
            Mathf.Abs(state.ThumbSticks.Left.Y) > 0.1f ? 1 * Mathf.Sign(state.ThumbSticks.Left.Y) : 0);
        visualAnimator.speed = finalVelocity * slowDownAnimator;

        transform.GetChild(0).localRotation = new Quaternion(
            mainCamera.transform.rotation.x,
            mainCamera.transform.rotation.y,
            mainCamera.transform.rotation.z,
            transform.rotation.w);

        Vector3 positionTotem = new Vector3(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, 0).normalized * 1.5f;
        if (positionTotem != Vector3.zero) {
            RotatePointTotem.transform.localPosition = positionTotem;
        }

        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed) {
            if (CurrentTotem == null) {
                Debug.Log("spownTotem");
                Instantiate(TotemPrefab, RotatePointTotem.transform.position, Quaternion.identity);
            }
            else {
                Debug.Log("DestroyTotem");
                Destroy(CurrentTotem.transform.parent.gameObject);
            }
        }

        Cc.Move(moveDirection * Time.deltaTime);

        lastMovement = moveDirection;

        detectMine();

        float ratioFloat = finalVelocity / speed;
        ratioFloat = Mathf.Clamp01(ratioFloat);
        if (float.IsNaN(ratioFloat))
            ratioFloat = 0;
        paint.RaycastGround(zoneSize.Evaluate(ratioFloat));

        lastSpeed = ratioFloat;
    }

    void OnGUI() {
        /*string text = "Use left stick to turn the cube, hold A to change color\n";
        text += string.Format("IsConnected {0} Packet #{1}\n", state.IsConnected, state.PacketNumber);
        text += string.Format("\tTriggers {0} {1}\n", state.Triggers.Left, state.Triggers.Right);
        text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
        text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state.Buttons.Start, state.Buttons.Back, state.Buttons.Guide);
        text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
        text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
        text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);*/
    }

    private void detectMine() {
        if (MineCollider != null) {
            double dist = Vector3.Distance(MineCollider.transform.position, transform.position);
            if (dist <= ((SphereCollider) MineCollider).radius) {
                float ratio = (float) (1.0f - dist / ((SphereCollider) MineCollider).radius);
                ratio *= ratio;
//                Debug.Log("ratio : " + ratio);
                GamePad.SetVibration(playerIndex, ratio, ratio);
            }
            else {
                MineCollider = null;
                GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
            }
        }
    }


    public void Explosion(Vector3 source, List<MeshFilter> meshes)
    {
        StopAllCoroutines();
        StartCoroutine(explosionMove(source, meshes));
    }

    public IEnumerator explosionMove(Vector3 sourcePos, List<MeshFilter> meshes)
    {
        //Debug.Log("Start it ! Explode !");
        speed = explosionControllerSpeed;

        Vector3 explosionDirection = (this.transform.position - sourcePos);
        explosionDirection.z = 0;
        explosionDirection.Normalize();
        Debug.DrawRay(sourcePos, explosionDirection, Color.red, 2f);
        //Debug.Log("explosionDirection = " + explosionDirection + " (this.transform.position - sourcePos) == (" + (this.transform.position) + " - " + sourcePos + ")");
        float timeStep = 1f / howLong;
        float lerp = 0;
        float currentIntensity = 1;

        //Couleur first

        paint.explodeIt(sourcePos, meshes);

        while (lerp < 1)
        {
            lerp += Time.deltaTime * timeStep;
            currentIntensity = explosionCurve.Evaluate(lerp) * explosionForce;
            Cc.Move(explosionDirection * Time.deltaTime * currentIntensity);

            //Post process value : animationCurve on his weight
            boomPostProcess.weight = explosionPPCurve.Evaluate(lerp);
            //Touch plant around (Ball of death expanding !!)

            yield return new WaitForSeconds(0.01f);
        }

        speed = speedNormal;
        //Debug.Log("Finish it ! Explode !");
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Contains("ColliderOtherTotem")) {
            CurrentTotem = other.gameObject;
            Debug.Log("OUI");
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.name.Contains("MineSpown")) {
//            Debug.Log("Stay");
            if (MineCollider == null) {
                MineCollider = other;
            }
            else {
                double firstDist = Vector3.Distance(MineCollider.transform.position, transform.position);
                double secondDist = Vector3.Distance(other.transform.position, transform.position);
                if (firstDist < secondDist) {
                    MineCollider = other;
                }
            }
        }
        else if (other.gameObject.name.Contains("ColliderOtherTotem")) {
            CurrentTotem = other.gameObject;
            Debug.Log(CurrentTotem!=null);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name.Contains("ColliderOtherTotem")) {
            CurrentTotem = null;
            Debug.Log("NON");
        }
    }

    private void OnApplicationQuit() {
        GamePad.SetVibration(playerIndex, 0.0f, 0.0f);
    }
}