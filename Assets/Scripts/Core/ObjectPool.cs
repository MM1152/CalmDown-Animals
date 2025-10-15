using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ObjectPool<TKey,T> : MonoBehaviour where T : MonoBehaviour
{
    protected Dictionary<TKey, Queue<T>> poolingQueue = new Dictionary<TKey, Queue<T>>();
    public List<T> prefabs = new List<T>();

    public T ShowObject(TKey key)
    {
        if (!Variable.onFPX) return null;

        T item = null;

        if (poolingQueue[key].Count <= 0)
        {
            item = CreateInstance(key);              
        }else
        {
            item = poolingQueue[key].Dequeue();
        }

        return item;
    }

    public void ReturnObject(TKey key,  T obj)
    {   
        poolingQueue[key].Enqueue(obj);
    }
    
    protected abstract T CreateInstance(TKey key);
}
