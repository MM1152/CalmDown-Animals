using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class InTileAnimal : MonoBehaviour
{
    public GameManager manager;
    public List<Enemy> list = new List<Enemy>();

    public int killStack = 0;

    public event Action<EnemyHealth> CheckOutAnimal;
    private MeshRenderer mesh;
    private Color prevColor = Color.white;
    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        
    }

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

    public void ChangeColor(Color color)
    {
        prevColor = mesh.materials[0].color;
        mesh.materials[0].color = color; 
    }

    public void ResetColor()
    {
        mesh.materials[0].color = Color.white;
    }

    public Enemy Get(AnimalSize size)
    {
        var newList = list.Where(x => (int)(x.GetSize() & size) > 1).ToList();
        if(newList.Count > 0)
        {
            return newList[0];
        }
        return null;
    }
}
