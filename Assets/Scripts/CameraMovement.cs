using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
  
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal")!=0)
        {
            transform.position += transform.right * Input.GetAxis("Horizontal")*Time.deltaTime*10;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.position += transform.up * Input.GetAxis("Vertical")*10*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (transform.position.y < 90)
                transform.position += transform.forward*-1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if(transform.position.y>10)
                transform.position += transform.forward;
        }
        



    }
}
