using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameObject[] DestroysInto;
    public GameObject Explosion;
    private Color color;
    private Sprite sprite;
    public float Reward;
    [Range(0.01f, 20f)]
    public float Speed = 1;

    public float SpeedModifier = 1;
    public float DamageModifier = 1;
    public float RewardModifier = 1;
    int gizmosrefresh;
    [Min(1)]
    public float Health = 10;
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 1f);
        for (int i = index; i < Path.ToArray().Length; i++)
        {
            Vector2 o = Path[Mathf.Max(i - 1, 0)];
            Vector2 t = Path[i];

            Gizmos.DrawLine(new Vector3(o.x, o.y, 10), new Vector3(t.x, t.y, 10));

        }
    }

    [HideInInspector]
    public List<Vector2> Path;
    [HideInInspector]
    public int index;
    [HideInInspector]
    public bool HasPath;
    private void Start()
    {
        color = this.GetComponent<SpriteRenderer>().color;
        sprite = this.GetComponent<SpriteRenderer>().sprite;

        if (this.transform.parent == null||this.transform.parent.GetComponent<EnemyManager>()==null)
        {
            this.transform.parent = GameObject.FindObjectOfType<EnemyManager>().transform;
        }
        if (!HasPath)
        {
            int ind = 0;
            HasPath = true;
            float dist = float.MaxValue;
            if (this.Path != null && this.Path.ToArray().Length > 0)
            {
                for (int i = 0; i < this.Path.ToArray().Length; i++)
                {
                    if (Vector2.Distance(this.transform.position, this.Path[i]) < dist)
                    {
                        ind = i;
                        dist = Vector2.Distance(this.transform.position, this.Path[i]);
                    }
                }
            }

            index = ind;
        }
    }
    private void FixedUpdate()
    {
        UpdateMove();
        if (Health <= 0)
            Destroy(this.gameObject);

        Vector3 dv = new Vector3(DamageModifier, SpeedModifier, RewardModifier);
        dv = Vector3.MoveTowards(dv, new Vector3(1, 1,1), 0.001f);
        DamageModifier = dv.x;
        SpeedModifier = dv.y;
        RewardModifier = dv.z;

    }
    public void UpdateMove()
    {

        if (this.GetComponent<Enemy>().Path == null || this.GetComponent<Enemy>().Path.ToArray().Length <= 0)
        {
            //Debug.Log("PathQueried");
            //    c.GetComponent<Enemy>().Path = this.GetComponent<PathManager>().getPath(new Vector2Int(Mathf.RoundToInt(c.position.x),Mathf.RoundToInt(c.position.y)));
           this.Path = this.transform.parent.GetComponent<PathManager>().getPath();

            int ind = 0;
            float dist = float.MaxValue;
            for(int i=0;i<this.Path.ToArray().Length;i++)
            {
                if(Vector2.Distance(this.transform.position,this.Path[i])<dist)
                {
                    ind = i;
                    dist = Vector2.Distance(this.transform.position, this.Path[i]);
                }
            }

            index = ind;

        }

        Enemy e = this;

        List<Vector2> Path = e.Path;

        if (Path != null && Path.Count >= 1)
        {

            if (e.index < 0 )
            {
                

                e.index = 0;
                e.transform.position = Path[0];
            }

        if (e.index >= Path.Count)
        {
                ClosingTime = true;
                GameDataManager.Health--;
                e.index = 0;
                Reward = 0;
                Destroy(this.gameObject);
        }

            float speed =( e.Speed*e.SpeedModifier) / 25f;

            Vector2 target = Path[e.index];
            Vector2 p = e.transform.position;

            e.transform.position = Vector2.MoveTowards(p, target, speed);

            if (p.x > e.transform.position.x)
                e.GetComponent<SpriteRenderer>().flipX = true;
            else
                e.GetComponent<SpriteRenderer>().flipX = false;

            if (Vector2.Distance(e.transform.position, target) <= 1)
                e.index++;
        }
     }
    bool ClosingTime;
    private void OnApplicationQuit()
    {
        ClosingTime = true;
    }
    private void OnDestroy()
    {
        if(!ClosingTime)
        {
            ClosingTime = Application.isLoadingLevel;
        }
        if (!ClosingTime)
        {
            GameDataManager.Balance +=Mathf.RoundToInt( Reward* RewardModifier);
            if (DestroysInto != null)
            {
                for (int i = 0; i < DestroysInto.Length; i++)
                {
                    if (DestroysInto[i] != null)
                    {
                        GameObject g=Instantiate(DestroysInto[i], this.transform.position, this.transform.rotation, this.transform.parent);
                        if(g.GetComponent<Enemy>()!=null)
                        {
                            g.GetComponent<Enemy>().Path = Path;
                            g.GetComponent<Enemy>().index = index;
                            g.GetComponent<Enemy>().HasPath = true;

                        }
                    }
                }
            }
            if (Explosion != null)
            {

                Instantiate(Explosion, this.transform.position, this.transform.rotation, this.transform.parent);
            }
        }
    }
}
