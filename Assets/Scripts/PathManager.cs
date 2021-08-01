using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoyT.AStar;
using UnityEngine.Tilemaps;
public class PathManager : MonoBehaviour
{

    public static int Size;
    private Tilemap map;
    public static RoyT.AStar.Grid grid;
    [Range(10,100)]
    public int size = 100;
    public bool AddGranulation = true;
    private Gradient g;
    private void Initialize()
    {
        map = this.GetComponent<Tilemap>();
        grid = new RoyT.AStar.Grid(size, size, 100000);
        Size = size;
            GradientAlphaKey[] ak = new GradientAlphaKey[2];
            ak[0].alpha=1;
            ak[0].time=0;
            ak[1].time=1;
            ak[1].alpha=1;
        GradientColorKey[] ck = new GradientColorKey[2];
       ck[0].color = Color.white;
            ck[0].time=0;
            ck[1].time=1;
           ck[1].color=Color.black;
        g = new Gradient();
        g.alphaKeys = ak;
        g.colorKeys = ck;
        g.mode = GradientMode.Blend;
        Random.InitState((int)Time.time);
    for(int x=0;x<size;x++)
        {
            for(int y=0;y<size;y++)
            {
                if(map.GetTile(new Vector3Int(x,y,0))!=null)
                {
                    int n = 1;

                    for(int x1=-1;x1<=1;x1++)
                    {
                        for (int y1 = -1; y1 <= 1; y1++)
                        {
                            if (map.GetTile(new Vector3Int(x + x1, y + y1, 0))==null)
                                n+=10;
                        }
                    }

                    grid.SetCellCost(new RoyT.AStar.Position(x, y), n+Random.Range(0,10));
                    map.SetTileFlags(new Vector3Int(x, y, 0),TileFlags.None);
                    if(AddGranulation)
                    map.SetColor(new Vector3Int(x, y, 0),g.Evaluate(Mathf.PerlinNoise(x/2f,y/2f)/3f));
                    else
                        map.SetColor(new Vector3Int(x, y, 0), Color.white);

                    // map.SetEditorPreviewColor(new Vector3Int(x, y, 0),);
                }
            }
        }
        gridPath=toVector(grid.GetPath(new RoyT.AStar.Position(Spawn.x, Spawn.y), new RoyT.AStar.Position(End.x, End.y)));

        GridPath = new List<Vector2>();
        foreach(Vector2 v in gridPath)
        {
            GridPath.Add(v);
        }
    }
     int gizmosrefresh;
    public Vector2Int Spawn;
    public Vector2Int End= new Vector2Int(99,99);
    Vector2[] gridPath;
    [HideInInspector]
    public List<Vector2> GridPath;

    int ranoffset;
    public List<Vector2> getPath()
    {

        Random.InitState(ranoffset);
        ranoffset++;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (grid.GetCellCost((new RoyT.AStar.Position(x, y))) <= 1000)
                    {
                    int n = 1;

                    for (int x1 = -1; x1 <= 1; x1++)
                    {
                        for (int y1 = -1; y1 <= 1; y1++)
                        {
                            if (map.GetTile(new Vector3Int(x + x1, y + y1, 0)) == null)
                                n += 10;
                        }
                    }
                    grid.SetCellCost(new RoyT.AStar.Position(x, y), n + Random.Range(0, 10));
                }
            }
        }
                gridPath = toVector(grid.GetPath(new RoyT.AStar.Position((int)Spawn.x, (int)Spawn.y), new RoyT.AStar.Position(End.x, End.y)));
        GridPath = new List<Vector2>();
        foreach (Vector2 v in gridPath)
        {
            GridPath.Add(v);
        }
        if (ranoffset >= this.transform.childCount)
            ranoffset = 0;
        return GridPath;
    }
    private void OnDrawGizmos()
    {
        Spawn = new Vector2Int(Mathf.Min(Mathf.Max(Spawn.x, 0), size - 1), Mathf.Min(Mathf.Max(Spawn.y, 0), size - 1));
        End = new Vector2Int(Mathf.Min(Mathf.Max(End.x, 0), size - 1), Mathf.Min(Mathf.Max(End.y, 0), size - 1));

        Initialize();
        gizmosrefresh++;
        if (gizmosrefresh >= 10|| gridPath == null)
        {
            gridPath = toVector(grid.GetPath(new RoyT.AStar.Position(Spawn.x, Spawn.y), new RoyT.AStar.Position(End.x, End.y)));
  
            gizmosrefresh = 0;
        }
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(size / 2, size / 2, 0), new Vector3(size, size, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(Spawn.x,Spawn.y,0), 1);
        Gizmos.DrawWireSphere(new Vector3(End.x, End.y,0), 1);
        Gizmos.color = new Color(Color.green.r,Color.green.g,Color.green.b,0.5f);
        for (int i = 0; i < gridPath.Length; i++)
        {
            Vector2 o = gridPath[Mathf.Max(i - 1, 0)];
            Vector2 t = gridPath[i];

            Gizmos.DrawLine(o, t);

        }
    }
    private Vector2[] toVector(RoyT.AStar.Position[] rp)
    {
        List<Vector2> v = new List<Vector2>();
      
        foreach(RoyT.AStar.Position p in rp)
        {
            v.Add(new Vector2(p.X, p.Y));
        }
        return v.ToArray();
    }
    void Start()
    {
        Initialize();
    }
    
    void Update()
    {
        
    }
}
