using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    private GameObject currentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (currentObject == null)
            {
                
                AlignObjectToCenter(other);
                currentObject = other.gameObject;
            }
            else
            {
                ExpelObject(other);
            }
        }
    }

    private void AlignObjectToCenter(Collider other)
    {
      
        Rigidbody rb = other.attachedRigidbody;
        rb.isKinematic = true;

        Vector3 centerPosition = transform.position;
        centerPosition.y += 1.0f; 

        rb.transform.position = centerPosition;

       
        rb.transform.rotation = Quaternion.identity;

       
        rb.transform.localScale = Vector3.one; 
    }

    private void ExpelObject(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        
        rb.isKinematic = false;

        
        Vector3 expelDirection = (other.transform.position - transform.position).normalized;
        rb.AddForce(expelDirection * 50f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentObject == other.gameObject)
        {
            currentObject = null;
        }
    }
}