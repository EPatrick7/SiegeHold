using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHider : MonoBehaviour
{
    [Header("Unlock Scene <Give Scene Name>")]
    public string SceneName;
    public bool IsGameOver;
    private void Start()
    {
        if (this.isActiveAndEnabled)
            SetState();
    }
    private void Awake()
    {
        if(this.isActiveAndEnabled)
        SetState();
    }
    public void SetState()
    {
        if(IsGameOver)
        {
            this.transform.GetChild(0).gameObject.SetActive(GameDataManager.Gameover);
        }
        else
      this.gameObject.SetActive(PlayerPrefs.GetInt("SceneUnlocked_"+SceneName, 0)>=1);
    }
}
