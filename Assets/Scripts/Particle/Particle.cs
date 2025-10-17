using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particle : MonoBehaviour
{
    private ParticleSystem particle;
    private ParticleSpawner spawner;
    private ParticleType key;

    private Coroutine co;
    public void Init(ParticleSpawner spawner , ParticleType key)
    {
        particle = GetComponent<ParticleSystem>();
        this.spawner = spawner;
        this.key = key;
        gameObject.SetActive(false);
    }

    public bool Play()
    {
        if(co != null)
        {
            StopCoroutine(co);
            spawner.ReturnToPool(key, this);
            return false;
        }
        gameObject.SetActive(true);
        co = StartCoroutine(WaitForPlayCo());
        return true;
    }

    private IEnumerator WaitForPlayCo()
    {
        yield return new WaitForSeconds(particle.main.duration);
        spawner.ReturnToPool(key , this);
        gameObject.SetActive(false);
        co = null;
    }
}
