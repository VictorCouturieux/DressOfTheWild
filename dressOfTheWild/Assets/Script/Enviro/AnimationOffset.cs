using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    public Vector2 speedRange = new Vector2(0.5f, 2f);
    public Vector2 scaleXRange = new Vector2(0.9f, 1.2f);
    public Vector2 scaleYRange = new Vector2(0.9f, 1.1f);
    public Gradient tintThanCanBeGive = new Gradient();

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Animator>().speed = Random.Range(0.5f, 2f);
        RandomSize();
        RandomTint();
    }

    [ContextMenu("RandomTint")]
    public void RandomTint()
    {
        GetComponentInChildren<SpriteRenderer>().color = tintThanCanBeGive.Evaluate(Random.Range(0f, 1f));
    }

    [ContextMenu("RandomSize")]
    public void RandomSize()
    {
        Vector3 scale = this.transform.localScale;
        scale.x = Random.Range(scaleXRange.x, scaleXRange.y);
        scale.y = Random.Range(scaleYRange.x, scaleYRange.y);
        this.transform.localScale = scale;
    }

}
