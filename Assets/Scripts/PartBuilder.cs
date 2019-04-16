using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBuilder : MonoBehaviour
{
    Animator animator;

    public Animator fuelMenu;
    public Animator motorMenu;
    public Animator podMenu;

    void Start()
    {

    }

    public void FuelMenu()
    {
        fuelMenu.SetTrigger("OpenMenu");
        motorMenu.SetTrigger("CloseMenu");
        podMenu.SetTrigger("CloseMenu");
    }
    public void MotorMenu()
    {
        fuelMenu.SetTrigger("CloseMenu");
        motorMenu.SetTrigger("OpenMenu");
        podMenu.SetTrigger("CloseMenu");
    }

    public void PodMenu()
    {
        fuelMenu.SetTrigger("CloseMenu");
        motorMenu.SetTrigger("CloseMenu");
        podMenu.SetTrigger("OpenMenu");
    }

}
