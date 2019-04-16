using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodAnimator : MonoBehaviour
{
    
    void Start()
    {
        GameObject.Find("EditorUI").GetComponent<PartBuilder>().podMenu = GetComponent<Animator>();
    }
}
