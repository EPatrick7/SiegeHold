using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;
public class BackgroundManager : MonoBehaviour
{
    public Tile DefaultTile;
    public PathManager PathManager;
    public Gradient HeightGradient;
    int gizmosrefresh=12;
    public float Scale;
    public int Seed;
    
    private void OnDrawGizmos()
    {
        gizmosrefresh++;
        if(gizmosrefresh>11)
        {
            gizmosrefresh = 0;
            DrawBackground();
        }
    }
    void Start()
    {
        DrawBackground();
    }
    public void DrawBackground()
    {
        Tilemap map = this.GetComponent<Tilemap>();
        map.ClearAllTiles();
        for (int x = 0; x < PathManager.size; x++)
        {
            for (int y = 0; y < PathManager.size; y++)
            {
                float height=Mathf.PerlinNoise((x+ Seed )/ Scale,(y+ Seed)/ Scale);
                Color c = HeightGradient.Evaluate(height);
                Color temp=DefaultTile.color;
                DefaultTile.color = c;
                map.SetTile(new Vector3Int(x, y, 0), DefaultTile);
                map.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.LockColor);
                DefaultTile.color = temp;
            }
        }
        if (PathManager != null)
        {
            Tilemap map2 = PathManager.GetComponent<Tilemap>();

            for (int x = 0; x < PathManager.size; x++)
            {
                for (int y = 0; y < PathManager.size; y++)
                {
                    if (map2.GetTile(new Vector3Int(x, y, 0)) != null)
                        map.SetTile(new Vector3Int(x, y, 0), null);
                }
            }

        }
    
    }
}
