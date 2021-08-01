using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPurchaser : MonoBehaviour
{
    public GameObject Turret;
    public int Cost;



    private Color Original;
    private Color Target;
    private Color Disabled;
    private SpriteRenderer me;
    public bool Demolish;
    void Start()
    {
        me = this.GetComponent<SpriteRenderer>();

        Original = me.color;
        Target = Color.Lerp(Original, Color.black, 0.33f);
        Disabled = Color.Lerp(Target, Color.red, 0.33f);
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
    private void OnMouseDown()
    {
        if (TurretBuilder.BuildOption == Turret)
        {
            TurretBuilder.BuildOption = null;
            TurretBuilder.Cost = 0;
            TurretBuilder.Demolishing = false;
        }
        else
        {
            if(Demolish)
            TurretBuilder.Demolishing = true;
            else
                TurretBuilder.Demolishing = false;
            TurretBuilder.BuildOption = Turret;
            TurretBuilder.Cost = Cost;
        }
        me.color = Color.Lerp(Original, Color.black, 0.5f);
    }
    private bool CanAfford;
   public void FixedUpdate()
    {
        CanAfford = GameDataManager.Balance >= Cost;


        if (TurretBuilder.BuildOption == Turret&&!CanAfford)
        {
            TurretBuilder.BuildOption = null;

            TurretBuilder.Demolishing = false;
            TurretBuilder.Cost = 0;

        }

            Color targ = me.color;
        if (IsOver)
            targ = Target;
        else
        {
            targ = Original;

            if (TurretBuilder.BuildOption == Turret)
                targ = Target;

        }


        if (!CanAfford)
            targ = Disabled;

            me.color = Color.Lerp(me.color, targ, 0.33f);

    }
}
