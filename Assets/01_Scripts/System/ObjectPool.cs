using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    public Queue<T> createObj = new Queue<T>();
    private T prefab;
    private T newObj;
    private int initialSize;
    private Transform parent;

    public Queue<T> CreateObject(T prefab, int initialSize, Transform parent = null)
    {
        for (int i = 0; i < initialSize; i++)
        {
            newObj = GameObject.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            createObj.Enqueue(newObj);
        }
        return createObj;
    }
    public T GetObject()
    {
        if (createObj.Count > 0)
            return createObj.Dequeue();
        else
            return null;
    }
    public void DisableObject(T obj)
    {
        createObj.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}
