using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenControl : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player") 
        {
            transform.position += new Vector3(0, 0, collision.transform.localScale.z + 2);
        }
    }
}
