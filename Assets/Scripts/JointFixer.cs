using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   
public class JointFixer : MonoBehaviour
{
    private GameObject MouseManager;
    private List<GameObject> PotentialNearJoints;
    public List<GameObject> Joints;

    private List<float> JointDistances;

    void Start()
    {
        MouseManager = GameObject.Find("MouseManager");
    }

    void Update()
    {
        //Finding Closest Joint
        PotentialNearJoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("PartJoints"));
        JointDistances = new List<float>(PotentialNearJoints.ToArray().Length);
    }

    public void StickPart()
    {
         foreach(GameObject joint in Joints)
        {
            foreach(GameObject otherJoint in PotentialNearJoints)
            {
                if (otherJoint != joint 
                && Vector3.Distance(otherJoint.transform.position, joint.transform.position) < 0.5f)
                {
                    transform.position = otherJoint.transform.position + (transform.position - joint.transform.position);
                    transform.rotation = otherJoint.transform.parent.rotation;
                    transform.parent = (otherJoint.transform.parent);
                }
            }
        }
        GameObject.FindObjectOfType<Pod>().RecalculateRocket();
    }
}
