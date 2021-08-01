using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TMPDataSampler : MonoBehaviour
{
    public enum Type {Health,Money,Wave,BestWave };
    public Type MyType;
    void Update()
    {
        if (MyType == Type.Health)
            this.GetComponent<TextMeshPro>().text = GameDataManager.Health+"<#FF0000>♥";
        if (MyType == Type.Money)
            this.GetComponent<TextMeshPro>().text = GameDataManager.Balance+ "<#1BBF00>$";
        if (MyType == Type.Wave)
            this.GetComponent<TextMeshPro>().text = "Wave: <#0043FF>"+ GameDataManager.Wave;
        if (MyType == Type.BestWave)
            this.GetComponent<TextMeshPro>().text = "Best Wave: <#47949F>" + GameDataManager.BestWave;
    }
}
