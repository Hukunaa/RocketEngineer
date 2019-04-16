 using UnityEngine;
 using System.Collections;
 
 public class PlayerFollow : MonoBehaviour {
 
     public float turnSpeed = 0.0f;
     public Transform player;
 
     public Vector3 offset;
 
    private float xRot;
    private float yRot;
    private float distance;

     void Start () 
     {
         xRot = 0;
         yRot = 0;
         distance = -20;
         offset = transform.position;
         player = GameObject.FindObjectOfType<Pod>().transform;
     }
 
     void FixedUpdate()
     {
         if(Input.GetMouseButton(1))
         {
            xRot += Input.GetAxis("Mouse X") * turnSpeed;
            yRot += Input.GetAxis("Mouse Y") * turnSpeed * -1;
         }

        Quaternion finalRot = Quaternion.Euler(yRot, xRot, 0);

        distance += Input.GetAxis("Mouse ScrollWheel") * 8;
        transform.position = Vector3.Slerp(transform.position, finalRot * new Vector3(0, 0, distance) + player.transform.position, 0.9f);
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, 0.4f);

         /* offset.z += Input.GetAxis("Mouse ScrollWheel") * 4;
         if(Input.GetMouseButton(1))
         {
            offset = Quaternion.AngleAxis (Input.GetAxis("Mouse X") * turnSpeed, transform.up) * offset;
            offset = Quaternion.AngleAxis (-Input.GetAxis("Mouse Y") * turnSpeed, transform.right) * offset;

            if(offset.y > 6)
                offset.y = 6;

            if(offset.y < -6)
                offset.y = -6;

         }
         transform.position = player.position + offset; 
         transform.LookAt(player.position);*/
         
     }
 }