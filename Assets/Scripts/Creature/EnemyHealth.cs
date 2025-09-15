using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour , IDamageAble
{
    protected int MaxHp { get; private set; }
    public int Hp { get; private set; }

    public bool IsDie { get => isDie; }
    private bool isDie;

    public Hpbar slider;

    public event Action onDie;

    public void Init(int maxHp)
    {
        isDie = false;
        MaxHp = maxHp;
        Hp = MaxHp;
    }

    public bool Hit(int damage)
    {
        Hp -= damage;
        slider.SetValue(Hp, MaxHp);
        if(Hp <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    public virtual void Die()
    {
        isDie = true;
        onDie?.Invoke();
        Destroy(gameObject);
    }
}
