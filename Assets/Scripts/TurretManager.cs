using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{[HideInInspector]
    public List<Enemy> CollidingEnemies;

    public float Damage = 2;
    public float SpeedModifier = 1;
    public float DamageModifier = 1;
    public float RewardModifier=1;
    [HideInInspector]
    public int Cost;
    bool Looked;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.GetComponent<ParticleSystem>() != null)
        {
            if(!this.GetComponent<ParticleSystem>().isPlaying)
            this.GetComponent<ParticleSystem>().Play();
        }
        if(this.GetComponent<AudioSource>()!=null)
        {
            if (!this.GetComponent<AudioSource>().isPlaying)
            {

                if (this.GetComponent<ParticleSystem>() != null)
                {
                    if (!this.GetComponent<ParticleSystem>().isEmitting)
                        this.GetComponent<AudioSource>().Play();
                }
                else
                    this.GetComponent<AudioSource>().Play();
            }
        }
        if (Rotate)
        {
            if (!Looked)
            {
                Looked = true;

                Vector3 diff = collision.gameObject.transform.position - transform.position;
                diff.Normalize();

                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation =Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z ),0.33f);
            }
        }

    }
    public bool Rotate = true;
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            if (!CollidingEnemies.Contains(other.gameObject.GetComponent<Enemy>()))
            {
                CollidingEnemies.Add(other.gameObject.GetComponent<Enemy>());
            }
        }
    }
    private void Start()
    {
        CollidingEnemies = new List<Enemy>();
    }
    private void FixedUpdate()
    {
        Looked = false;
        for(int i =0;i<CollidingEnemies.ToArray().Length;i++)
        {
           

            Enemy e = CollidingEnemies[i];
            if (e!=null)
            {
                e.SpeedModifier = (e.SpeedModifier+SpeedModifier)/2;
                e.DamageModifier = (DamageModifier+e.DamageModifier)/2;
                e.RewardModifier = (RewardModifier + e.RewardModifier) /2;
                e.Health-=Damage*e.DamageModifier;
            }
        }
        CollidingEnemies = new List<Enemy>();
    }
}
