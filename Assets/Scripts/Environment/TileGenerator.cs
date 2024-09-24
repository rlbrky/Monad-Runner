using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct Tiles
    {
        public GameObject[] tiles;
    }

    [Header("Tiles")]
    public Tiles[] themedTiles;

    [Header("Generating Properties")]
    [SerializeField] private int tileNum;
    [SerializeField] private int tilePos;
    [SerializeField] private int tileTheme;
    [SerializeField] private bool isCreatingTile;

    private void Update()
    {
        if(!isCreatingTile)
        {
            isCreatingTile = true;
            GenerateTiles();
        }
    }

    private void FixedUpdate()
    {
        // As of now adds 150 units into the position. We will optimize the spawn by doing it after the player reaches 120 unit mark.
        // Previously it would spawn after 3 seconds, which is not ideal for optimization.
        if (PlayerMovement.instance.transform.position.z >= (tilePos - 200))
        {
            Debug.Log("Creating tiles in the next frame.");
            isCreatingTile = false;
        }
    }

    private void GenerateTiles()
    {
        for(int i = 0; i < 3; i++)
        {
            tileNum = Random.Range(0, themedTiles[0].tiles.Length);
            Instantiate(themedTiles[0].tiles[tileNum], new Vector3(0, 0, tilePos), Quaternion.identity);
            tilePos += 50;
        }
    }
}
