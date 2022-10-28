using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLayout : MonoBehaviour
{

    public static int[,] level = new int[28, 29];

    public static int totalPellets = 0;

    private Tilemap map;

    [SerializeField]
    private List<Sprite> wallSprites;

    [SerializeField]
    private List<Sprite> pelletSprites;


    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();

        Tile tile;

        // copy the layout of the tilemap into the level array

        for (int x = -14; x < 14; x++) {
            for (int y = -14; y < 15; y++) {
                tile = map.GetTile<Tile>(new Vector3Int(x, y));
                if (tile == null) {
                    if (map.GetTile<AnimatedTile>(new Vector3Int(x, y)) != null) { // power pellet
                        level[x + 14, y + 14] = 3;
                        totalPellets++;
                    } else { // nothing
                        level[x + 14, y + 14] = 0;
                    }
                } else if (wallSprites.Contains(tile.sprite)) { // wall tile
                    level[x + 14, y + 14] = 1;
                } else if (pelletSprites.Contains(tile.sprite)) { // pellet
                    level[x + 14, y + 14] = 2;
                    totalPellets++;
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
