using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoralItem : MonoBehaviour
{
    //public UnityEvent cEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        //collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;// useGravity = false;
        //collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        //Debug.Log("CoralItem.OnCollisionEnter: " + collision.gameObject.name + " is collide with " + this.gameObject.name);
    }
}
