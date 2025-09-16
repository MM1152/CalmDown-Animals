using UnityEngine;
using System.Collections.Generic;

public class InTileAnimal : MonoBehaviour
{
    public List<Enemy> list = new List<Enemy>();

    public void InAnimal(Enemy animal)
    {
        list.Add(animal);
    }

    public void OutAnimal(Enemy animal)
    {
        list.Remove(animal);
    }

}
