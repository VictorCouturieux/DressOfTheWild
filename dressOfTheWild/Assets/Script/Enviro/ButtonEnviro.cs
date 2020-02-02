using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnviro : MonoBehaviour
{
   

    [ContextMenu("Rename child correctly")]
    public void RenameCorrectChild()
    {
        int i = 0;
        foreach(Transform tr in transform.GetComponentsInChildren<Transform>())
        {
            if (tr.name.Contains("default"))
                tr.name = "default-" +i++;
        }
    }

}
