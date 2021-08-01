using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class UIButton : MonoBehaviour
{
    public enum ButtonType {None,Quit,LoadNextScene,LoadScene,DisableParent,ToggleObject}
    public int LoadVal;
    public ButtonType MyType;
    private Color Original;
    private Color Target;
    private Color Disabled;
    private SpriteRenderer me;
    public GameObject ToggleObj;
    void Start()
    {
        me = this.GetComponent<SpriteRenderer>();

        Original = me.color;
        Target = Color.Lerp(Original, Color.black, 0.33f);
        Disabled = Color.Lerp(Target, Color.red, 0.33f);
    }
    private void OnMouseDown()
    {
        if (MyType == ButtonType.Quit)
            Application.Quit();
        if (MyType == ButtonType.LoadNextScene)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
      
        }

        if (MyType == ButtonType.LoadScene)
        {
            SceneManager.LoadSceneAsync(LoadVal);
        }
        if (MyType == ButtonType.DisableParent)
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    
        if (MyType == ButtonType.ToggleObject)
        {
            ToggleObj.SetActive(!ToggleObj.activeSelf);
        }
    }
    private bool IsOver;
    private void OnMouseEnter()
    {
        IsOver = true;
    }
    private void OnMouseExit()
    {
        IsOver = false;
    }
    private void FixedUpdate()
    {
        Color targ = me.color;
        if (IsOver)
            targ = Target;
        else
        {
            targ = Original;


        }

        
        me.color = Color.Lerp(me.color, targ, 0.33f);

    }
}
