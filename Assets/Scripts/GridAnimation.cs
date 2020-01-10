// Attach to Main Camera
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridAnimation : MonoBehaviour
{
    [SerializeField] private Grid GridObject;
    [SerializeField] private int PrerenderDistance;
    [SerializeField] private int LeftX;
    [SerializeField] private int LeftY;
    [SerializeField] private int GridHeight;
    [SerializeField] private int GridWidth;

    void Start()
    {
        if (PrerenderDistance > 0)
        {
            foreach (Transform tilemapTransform in GridObject.transform)
            {
                Tilemap tilemap = tilemapTransform.gameObject.GetComponent<Tilemap>();
                for (int i = GridWidth + LeftX; i < GridWidth + PrerenderDistance + LeftX; i++)
                {
                    for (int j = LeftY; j > LeftY - GridHeight; j--)
                    {
                        TileBase replaceTile = tilemap.GetTile(new Vector3Int(i - GridWidth, j, 0));
                        tilemap.SetTile(new Vector3Int(i, j, 0), replaceTile);
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }
}
