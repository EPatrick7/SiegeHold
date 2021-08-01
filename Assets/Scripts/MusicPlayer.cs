using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public List<AudioClip> MusicSelections;
    public static float AudioTime;
    public static int MusicID = -1;
    private AudioSource me;
    private void Start()
    {
        me = this.GetComponent<AudioSource>();
        if (me == null)
        {
            this.enabled = false;
            Debug.Log("Music Player Lacks Audio Source Component");
        }
        else
        {
            if (MusicID >= 0)
            {
                if (MusicID <= MusicSelections.ToArray().Length)
                {
                    me.Play();
                    me.clip = MusicSelections[MusicID];
                    me.time = AudioTime;
                }
            }
        }
    }
    private void Update()
    {
        if(me!=null)
        {
            if(!me.isPlaying)
            {
                AudioTime = 0;
                Random.InitState(System.DateTime.Now.Second);
                MusicID = Random.Range(0, MusicSelections.ToArray().Length);

                me.clip = MusicSelections[MusicID];
                me.time = 0;
                me.Play();

            }
            else
            {
                AudioTime = me.time;
            }
        }
    }
}
