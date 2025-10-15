using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    Die,
    Hit,
}

public class ParticleSpawner : ObjectPool<ParticleType, Particle>
{
    public void Awake()
    {
        poolingQueue.Add(ParticleType.Hit, new Queue<Particle>());
        poolingQueue.Add(ParticleType.Die, new Queue<Particle>());
    }

    protected override Particle CreateInstance(ParticleType key)
    {
        Particle item = null;
        item = Instantiate(prefabs[(int)key], transform);
        item.Init(this, key);

        return item;
    }
}