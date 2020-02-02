using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public float offsetValue;
    public GameObject ondeSource;
    public AudioSource efxSource;

    public GameObject onde;

    private List<GameObject> ondes = new List<GameObject>();


    private CharactereMove charac;

    public void Start()
    {
        charac = GetComponentInParent<CharactereMove>();
    }

    public void Jump()
    {
        GameManager.instance.SoundManager.PlaySoundFX(efxSource, SFB_SoundManager.SoundFX.RabbitSteps);

        //Maybe add a little more position to anticipate the placement of the "lapin"
        Vector3 pos = ondeSource.transform.position + charac.lastMovement * offsetValue;

        GameObject instan = Instantiate(onde, pos, Quaternion.identity);
        ondes.Add(instan);
        Invoke("DestroyOnde",1f);
    }

    void DestroyOnde()
    {
        GameObject.Destroy((ondes[0]));
        ondes.RemoveAt(0);
    }

}
