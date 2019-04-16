using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour {

    public float tankFuel;
    private GameObject parentTank;
    private bool foundParent;

    public FuelTank tank;

    private void Start()
    {
        foundParent = false;
        parentTank = gameObject;
    }

    private void Update()
    {
        if(!foundParent)
        {
             for(int i = 0; i < 10; ++i)
            {
                if( parentTank == gameObject)
                {
                    if(parentTank.transform.parent != null)
                        parentTank = parentTank.transform.parent.gameObject;
                }
                else
                {
                    foundParent = true;
                    break;
                }
            }
        }

        if(parentTank != gameObject)
        {
            if(parentTank.GetComponent<FuelTank>())
            {
                if(parentTank.GetComponent<FuelTank>().tankFuel > 0)
                    tank = parentTank.GetComponent<FuelTank>();
                else
                    tank = GetComponent<FuelTank>();
            }

        }
        else
            tank = GetComponent<FuelTank>();

    }

}
