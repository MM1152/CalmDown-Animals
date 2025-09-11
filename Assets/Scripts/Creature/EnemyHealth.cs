using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour , IDamageAble
{
    protected float MaxHp { get; private set; } = 55;
    public float Hp { get; private set; }
    public bool IsDie { get; private set; }
    
    public void Init()
    {
        Hp = MaxHp;
    }

    public void Hit(int damage)
    {
        Hp -= damage;

        if(Hp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        IsDie = true;
    }
}
