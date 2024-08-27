using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private GameObject[] tiles;

    [Header("Generating Properties")]
    [SerializeField] private int tileNum;
    [SerializeField] private int tilePos;
    [SerializeField] private bool isCreatingTile;

    private void Update()
    {
        if(!isCreatingTile)
        {
            isCreatingTile = true;
            StartCoroutine(GenerateTile());
            StartCoroutine(GenerateTile());
            StartCoroutine(GenerateTile());
        }
    }

    private IEnumerator GenerateTile()
    {
        tileNum = Random.Range(0, tiles.Length);
        Instantiate(tiles[tileNum], new Vector3(0, 0, tilePos), Quaternion.identity);
        tilePos += 50;
        yield return new WaitForSeconds(3);
        isCreatingTile = false;
    }
}
