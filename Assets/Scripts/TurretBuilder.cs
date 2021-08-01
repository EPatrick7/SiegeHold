using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuilder : MonoBehaviour
{
    public static GameObject BuildOption;
    public static int Cost;
    public static bool Demolishing;
    public GameObject Cursor;
    public void Update()
    {
        Cursor.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition)+ new Vector3(0,0,1);
        if(BuildOption==null)
        {
            Cursor.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            Cursor.GetComponent<SpriteRenderer>().sprite = BuildOption.GetComponent<SpriteRenderer>().sprite;

        }
        if(Input.GetMouseButtonDown(0)&&BuildOption!=null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;

            if(pos.x>=0&&pos.y>=0)
            {
                if(pos.x<=PathManager.Size&&pos.y<=PathManager.Size)
                {
                    if(Demolishing)
                    {
                        GameObject sel=null;
                        float dist = float.MaxValue;
                        foreach (Transform c in this.transform)
                        {
                            if (c.GetComponent<TurretManager>() != null)
                            {
                                if (Vector2.Distance(pos, c.transform.position) <= dist)
                                {
                                    sel = c.gameObject;
                                    dist = Vector2.Distance(pos, c.transform.position);
                                }
                            }
                        }
                        if (TurretBuilder.Demolishing && sel != null)
                        {
                            if (Vector2.Distance(pos, sel.transform.position) <= 3)
                            {
                                Instantiate(TurretBuilder.BuildOption, sel.transform.position, sel.transform.rotation, sel.transform.parent);
                                if (sel.GetComponent<TurretManager>() != null)
                                    GameDataManager.Balance += Mathf.RoundToInt(sel.GetComponent<TurretManager>().Cost * 0.75f);
                                Destroy(sel.gameObject);
                            }
                        }
                    }

                    if (GameDataManager.Balance >= Cost&&!Demolishing)
                    {
                        bool b=true;
                        foreach(Transform c in this.transform)
                        {
                            if (Vector2.Distance(pos, c.transform.position) <= 2)
                                b = false;
                        }
                            if (PathManager.grid.GetCellCost(new RoyT.AStar.Position(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y))) <= 100)
                                b = false;
                        
                        if (b)
                        {
                            if (this.GetComponent<AudioSource>() != null)
                                this.GetComponent<AudioSource>().Play();
                            GameDataManager.Balance -= Cost;
                          GameObject g=  Instantiate(BuildOption, pos, BuildOption.transform.rotation, this.transform);
                            if(g.GetComponent<TurretManager>()!=null)
                            g.GetComponent<TurretManager>().Cost = Cost;
                        }
                    }
                }
            }

        }
    }
}
