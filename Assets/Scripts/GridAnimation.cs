// Attach to Main Camera
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
    [SerializeField] private Tilemap Spikes;
    [SerializeField] private Tile Spike;
    [SerializeField] private GameObject Ghost;
    private int LastX;
    private int CurrentEnemyCoord;

    void Start()
    {
        LastX = LeftX;
        CurrentEnemyCoord = LastX + GridWidth;
        GenerateSpikes(PrerenderDistance);
        if (PrerenderDistance > 0)
        {
            foreach (Transform tilemapTransform in GridObject.transform)
            {
                Tilemap tilemap = tilemapTransform.gameObject.GetComponent<Tilemap>();
                // Skip spikes layer
                if (tilemap.name == "Spikes") continue;
                for (int i = GridWidth + LeftX; i < GridWidth + PrerenderDistance + LeftX; i++)
                {
                    for (int j = LeftY; j > LeftY - GridHeight; j--)
                    {
                        TileBase replaceTile = tilemap.GetTile(new Vector3Int(i - GridWidth, j, 0));
                        tilemap.SetTile(new Vector3Int(i, j, 0), replaceTile);
                    }
                }
            }
            LeftX += PrerenderDistance + GridWidth - 1;
        }
    }

    public void GenerateBackground(int calls)
    {
        while (calls > 0)
        {
            foreach (Transform tilemapTransform in GridObject.transform)
            {
                Tilemap tilemap = tilemapTransform.gameObject.GetComponent<Tilemap>();
                // Skip spikes layer
                if (tilemap.name == "Spikes") continue;
                for (int j = LeftY; j > LeftY - GridHeight; j--)
                {
                    TileBase replaceTile = tilemap.GetTile(new Vector3Int(LeftX - GridWidth + 1, j, 0));
                    tilemap.SetTile(new Vector3Int(LeftX + 1, j, 0), replaceTile);
                }
            }
            LeftX += 1;
            calls -= 1;
        }       
    }

    public void ClearUnusedBackground(int leftX)
    {
        foreach (Transform tilemapTransform in GridObject.transform)
        {
            Tilemap tilemap = tilemapTransform.gameObject.GetComponent<Tilemap>();
            for (int i = LastX; i < leftX - GridWidth; i++)
            {
                for (int j = LeftY; j > LeftY - GridHeight; j--)
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
        }
        LastX = leftX - GridWidth;
    }

    public void GenerateEnemies(int length)
    {   while (length > 0)
        {
            int levelLength;
            int levelChoice = Random.Range(1, 5);
            if (length > 20)
            {
                levelLength = Random.Range(5, 21);
                length -= levelLength;
            }
            else
            {
                levelLength = Random.Range(1, length + 1);
                length -= levelLength;
            }
            if (levelChoice >= 3)
            {
                GenerateGhosts(levelLength);
            }
            else
            {
                GenerateSpikes(levelLength);
            }
        }        
    }

    public void GenerateSpikes(int length)
    {
        // One empty tile at start
        CurrentEnemyCoord += 1;
        length -= 1;
        while (length > 0)
        {
            if (length > 5)
            {
                int currentLineLength = Random.Range(1, 6);
                length -= currentLineLength;
                while (currentLineLength > 0)
                {
                    Spikes.SetTile(new Vector3Int(CurrentEnemyCoord, -3, 0), Spike);
                    CurrentEnemyCoord++;
                    currentLineLength--;

                }
                CurrentEnemyCoord += Random.Range(5, 10);
            }
            else
            {
                // Pass last empty
                CurrentEnemyCoord += length;
                length = 0;
            }          
        }
    }

    public void GenerateGhosts(int length)
    {
        // One empty tile at start
        CurrentEnemyCoord += 1;
        length -= 1;
        int fixedPos = 0;
        while (length > 0)
        {
            float height;
            if (fixedPos > 0)
            {
                height = Random.Range(-0.2f, 0.4f);
                fixedPos -= 1;
            }
            else
            {
                height = Random.Range(-0.8f, 0.4f);
            }           
            Instantiate(Ghost, new Vector3((CurrentEnemyCoord * 0.33f) + 1.577f, height, 0), new Quaternion(0, 0, 0, 0));
            if (height < -0.2f)
            {
                fixedPos = 2;
            }
            length -= 6;
            CurrentEnemyCoord += 5;
        }
    }
}
