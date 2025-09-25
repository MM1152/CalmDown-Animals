using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    Die,
    Hit,
}

public class ParticleSpawner : PoolingManager<ParticleType, Particle>
{
    public Particle die;
    public Particle hit;

    public bool useParticle1;

    public void Awake()
    {
        poolingQueue.Add(ParticleType.Hit, new Queue<Particle>());
        poolingQueue.Add(ParticleType.Die, new Queue<Particle>());
    }

    protected override Particle CreateInstance(ParticleType key)
    {
        Particle item = null;
        if (key == ParticleType.Die)
        {
            item = Instantiate(die, transform);
        }
        else if(key == ParticleType.Hit)
        {
            item = Instantiate(hit, transform);
        }

        item.Init(this, key);

        return item;
    }
}