using UnityEngine;
using System.Collections.Generic;
using System;

public class InTileAnimal : MonoBehaviour
{
    public GameManager manager;
    public List<Enemy> list = new List<Enemy>();

    public event Action<EnemyHealth> CheckOutAnimal;

    private void Start()
    {
        manager = GameObject.FindWithTag(TagIds.GameManagerTag).GetComponent<GameManager>();
        manager.endWave += () => list.Clear();
    }

    public void InAnimal(Enemy animal)
    {
        list.Add(animal);
    }

    public void OutAnimal(Enemy animal)
    {
        list.Remove(animal);
        CheckOutAnimal?.Invoke(animal.GetComponent<EnemyHealth>());
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
