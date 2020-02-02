using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGenerator : MonoBehaviour {

    public bool touched = false;

    public List<MeshFilter> meshFilts = new List<MeshFilter>();
    
    private void OnTriggerEnter(Collider other) {
            return;
        if (touched)
        if (other.gameObject.name.Contains("CharaVisual")) {
            CharactereMove characMove = other.GetComponentInParent<CharactereMove>();

            characMove.Explosion(this.transform.position, meshFilts);
            //need to redraw the ground !

            //move around the flower

            //DeactivateHimself();
            touched = true;
            Invoke("UnTouched", 0.2f);
        }

    }

    public void Start()
    {
        meshFilts = GameManager.instance.allMeshFilterGround;
    }

    public void UnTouched()
    {
        touched = false;
    }
    
}