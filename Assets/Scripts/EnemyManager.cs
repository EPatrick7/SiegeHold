using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
public class OnLevelWin
    {
        public float FinalWave;
        public GameObject WinGUI;
        [Header("Unlock Scene <Give Scene Name>")]
        public string UnlockScene;
    }

    [System.Serializable]
    public class WaveObject
    {
        public List<GameObject> Enemies;

        [Min(1)]
        public int EnemyCount;
        [Min(0)]
        public float TimeDelay;
        [Min(0)]
        public float SpawnDelay;
        public bool WaitTillAllDead = true;
    }
    public List<WaveObject> Waves;

    public bool LoopFinalWave = false;
    public static int WaveID;
    bool HasWon;
    
    public void FixedUpdate()
    {
        //List<Vector2> Path = this.GetComponent<PathManager>().GridPath;
        if (WaveID >= Waves.ToArray().Length)
        {
            if (LoopFinalWave)
            {
                WaveID = Waves.ToArray().Length - 1;
                Waves[WaveID].SpawnDelay /= 2;
                Waves[WaveID].EnemyCount++;
                if (Waves[WaveID].SpawnDelay <= 0.01f)
                    Waves[WaveID].SpawnDelay = 0.01f;
                if (Waves[WaveID].EnemyCount >= 500)
                    Waves[WaveID].EnemyCount = 500;
            }
            else
            {
                if (DoneWave)
                    DoneWave = false;
            }
        }
        if (WinData != null)
        {
            if (GameDataManager.Wave > WinData.FinalWave && !HasWon)
            {
                HasWon = true;
                if (WinData.WinGUI != null)
                    WinData.WinGUI.SetActive(true);
                PlayerPrefs.SetInt("SceneUnlocked_"+WinData.UnlockScene, 1);
            }
        }
        if(DoneWave)
        {
            DoneWave = false;
            StartCoroutine(DelayWave(Waves[WaveID]));
        }

    }
    public void CreateEnemy(GameObject g)
    {
        Vector3 pos = this.transform.position;

        pos = new Vector3(this.GetComponent<PathManager>().Spawn.x, this.GetComponent<PathManager>().Spawn.y,0);
        Instantiate(g, pos, this.transform.rotation, this.transform);
    }
    public IEnumerator DelayWave(WaveObject wav)
    {
        yield return new WaitForSeconds(wav.TimeDelay);

        for (int i =0;i < wav.EnemyCount;i++)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            CreateEnemy(wav.Enemies[Random.Range(0,wav.Enemies.ToArray().Length)]);
            if (wav.SpawnDelay <= 0)
                yield return new WaitForEndOfFrame();
            else
            yield return new WaitForSeconds(wav.SpawnDelay);
        }
      if(wav.WaitTillAllDead)
        {
            while(this.transform.childCount >= 1)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        WaveID++;
        DoneWave = true;
        GameDataManager.Wave++;
    }
    bool DoneWave;
    public OnLevelWin WinData;

    private void Start()
    {
        DoneWave = true;
        WaveID = 0;
        HasWon=false;
    }
   

    int gizmosrefresh=100;
    private void OnDrawGizmos()
    {
        gizmosrefresh++;

        if (!Application.isPlaying)
        {
            if (this.transform.childCount > 0)
            {
                if (gizmosrefresh >= this.transform.childCount)
                    gizmosrefresh = 0;
                Transform c = this.transform.GetChild(gizmosrefresh);
                // Debug.Log(c.gameObject);
                if (c != null && c.gameObject.GetComponent<Enemy>() != null)
                {
                    //    c.GetComponent<Enemy>().Path = this.GetComponent<PathManager>().getPath(new Vector2Int(Mathf.RoundToInt(c.position.x),Mathf.RoundToInt(c.position.y)));
                    c.GetComponent<Enemy>().Path = this.GetComponent<PathManager>().getPath();



                }
            }
            
        }
    }

    
}
