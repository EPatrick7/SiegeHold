using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMeOverTime : MonoBehaviour
{
    public float Delay;
    public enum Type {Generic,Audio,Particle };
    public Type MyType;
    void Start()
    {
        if(MyType==Type.Generic)
        {
            Destroy(this.gameObject, Delay);
        }
    }
    private void FixedUpdate()
    {
        if(MyType==Type.Audio)
        {
            if (!this.GetComponent<AudioSource>().isPlaying)
                Destroy(this.gameObject,Delay);
        }
        if (MyType == Type.Particle)
        {
            if (!this.GetComponent<ParticleSystem>().isPlaying)
                Destroy(this.gameObject, Delay);
        }
    }
}
