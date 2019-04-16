using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelAnimator : MonoBehaviour
{

    void Start()
    {
        GameObject.Find("EditorUI").GetComponent<PartBuilder>().fuelMenu = GetComponent<Animator>();
    }
}