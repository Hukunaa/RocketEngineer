using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorAnimator : MonoBehaviour
{

    void Start()
    {
        GameObject.Find("EditorUI").GetComponent<PartBuilder>().motorMenu = GetComponent<Animator>();
    }
}