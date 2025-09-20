using UnityEngine;
using System.Collections.Generic;
using System;

public class InTileAnimal : MonoBehaviour
{
    public GameManager manager;
    public List<Enemy> list = new List<Enemy>();

    public int killStack = 0;

    public event Action<EnemyHealth> CheckOutAnimal;

    private void Start()
    {
        killStack = 0;
        manager = GameObject.FindWithTag(TagIds.GameManagerTag).GetComponent<GameManager>();
        manager.endWave += () => list.Clear();
    }

    public void InAnimal(Enemy animal)
    {
        list.Add(animal);
        animal.health.onDie += InTileAnimalDie;
    }

    public void OutAnimal(Enemy animal)
    {
        list.Remove(animal);
        animal.health.onDie -= InTileAnimalDie;
        CheckOutAnimal?.Invoke(animal.GetComponent<EnemyHealth>());
    }

    public void InTileAnimalDie()
    {
        killStack++;
    }

    public Enemy Get()
    {
        if(list.Count > 0)
        {
            return list[0];
        }
        return null;
    }
}
