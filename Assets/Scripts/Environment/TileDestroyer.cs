using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDestroyer : MonoBehaviour
{
    public GameObject prevTile;

    private void OnTriggerStay(Collider other)
    {
        if(prevTile == null)
            prevTile = other.gameObject;
        else if(prevTile != other.gameObject)
            Destroy(prevTile.gameObject);
    }
}
