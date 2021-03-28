using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("World Attributes")]
    public float tileWidth;
    public float tileLength;
    public List<GameObject> tilePrefabs;
    public List<GameObject> activeTiles;

    // Start is called before the first frame update
    void Start()
    {
        for (int width = 0; width < tileWidth; width++)
        {
            for (int length = 0; length < tileLength; length++)
            {
                int randomTileIndex = Random.Range(0, tilePrefabs.Count);
                Vector3 randomTilePosition = new Vector3(width * 16, 0.0f, length * 16);
                float randomTileRotation = Random.Range(0, 4) * 90.0f;
                GameObject randomTile = Instantiate(tilePrefabs[randomTileIndex], randomTilePosition, Quaternion.Euler(0.0f, randomTileRotation, 0.0f));

                randomTile.transform.parent = this.transform;
                activeTiles.Add(randomTile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
