using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    public Vector2 speedRange = new Vector2(0.5f, 2f);
    public Gradient tintThanCanBeGive = new Gradient();

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Animator>().speed = Random.Range(0.5f, 2f);
        RandomTint();
    }

    [ContextMenu("RandomTint")]
    public void RandomTint()
    {
        GetComponentInChildren<SpriteRenderer>().color = tintThanCanBeGive.Evaluate(Random.Range(0f, 1f));
    }
    
}
