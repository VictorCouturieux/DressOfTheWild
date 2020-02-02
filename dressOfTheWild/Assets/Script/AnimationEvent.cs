using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public AudioSource efxSource;

    public GameObject onde;

    private List<GameObject> ondes = new List<GameObject>();
    
    public void Jump()
    {
        GameManager.instance.SoundManager.PlaySoundFX(efxSource, SFB_SoundManager.SoundFX.RabbitSteps);
        
        //Maybe add a little more position to anticipate the placement of the "lapin"

        GameObject instan = Instantiate(onde, this.transform.position, Quaternion.identity);
        ondes.Add(instan);
        Invoke("DestroyOnde",1f);

    }

    void DestroyOnde()
    {
        GameObject.Destroy((ondes[0]));
        ondes.RemoveAt(0);
    }

}
