using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArbreTree : MonoBehaviour
{
    public static CharactereMove charac = null;

    public SpriteRenderer spriteRndre;

    public float valueOffset = 0.3f;

    // Start is called before the first frame update
    void Awake()
    {
        if (charac == null)
            charac = GameObject.FindObjectOfType<CharactereMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y < ArbreTree.charac.transform.position.y)
        {
            if (spriteRndre == null)
                spriteRndre = GetComponentInChildren<SpriteRenderer>();

            Color arrColor = spriteRndre.color;
//            Debug.Log("Val = "+ ((this.transform.position.y - ArbreTree.charac.transform.position.y) * valueOffset));
            arrColor.a = 1 +( (this.transform.position.y - ArbreTree.charac.transform.position.y) * valueOffset);
            spriteRndre.color = arrColor;

        }
        else
        {

            if (spriteRndre == null)
                spriteRndre = GetComponentInChildren<SpriteRenderer>();

            Color arrColor = spriteRndre.color;
            arrColor.a = 1;
            spriteRndre.color = arrColor;
        }
        
    }
}
