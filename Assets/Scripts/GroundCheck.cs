using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            PlayerMovement.instance.isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            PlayerMovement.instance.isGrounded = false;
        }
    }
}
