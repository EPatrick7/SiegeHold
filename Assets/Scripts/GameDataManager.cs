using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public PathManager PathManager;
    public static float Balance;
    public static int Wave;
    public static int BestWave;
    public float Cash;
    public static int Health;
    public int HP;
    public static bool Gameover;
    private void Start()
    {
        Health = HP;
        Gameover = false;
        Balance = Cash;
        Wave = 1;
        int val = PlayerPrefs.GetInt("HighScore_" + SceneManager.GetActiveScene().name);
        BestWave = val;
    }
    private void FixedUpdate()
    {
        Cash = Balance;
        HP = Health;
        if(Health<0)
        {
            Gameover = true;
            Health = 10;
            
            SceneManager.LoadSceneAsync(0);
        }

        int val = PlayerPrefs.GetInt("HighScore_" + SceneManager.GetActiveScene().name,0);

        if(Wave>val)
        {
            PlayerPrefs.SetInt("HighScore_" + SceneManager.GetActiveScene().name,Wave);

        }

        BestWave = val;

    }
    private void OnDrawGizmos()
    {
        if(PathManager != null)
        {
            this.transform.position = new Vector3(PathManager.size / 2, PathManager.size / 2, -10);
            this.GetComponent<Camera>().orthographicSize = PathManager.size/2f;
        }
    }
}
