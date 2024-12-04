using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour // ���׸� Ȱ��
{
    public Queue<T> createObj = new Queue<T>(); // ť ��� (��Ȱ��ȭ �� ������Ʈ�� ������� : �̹� Ȱ��ȭ�� ������Ʈ�� �����ؼ� ����ϸ� �ȵǱ� ����)
    private T prefab; // ������ ������
    private T newObj; // ������ ������Ʈ �ӽ� ����
    private int initialSize; // ���� ��
    private Transform parent; // �θ� ������Ʈ

    public Queue<T> CreateObject(T prefab, int initialSize, Transform parent = null) // �����ڸ� ���� �ʱ� ������Ʈ ����
    {
        for (int i = 0; i < initialSize; i++)
        {
            newObj = GameObject.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            createObj.Enqueue(newObj); // ��Ȱ��ȭ �� ������Ʈ�� ť�� ����
        }
        return createObj;
    }
    public T GetObject() 
    {
        if (createObj.Count > 0) // ť�� ���� �������� �ʴٴ� ���� ��Ȱ��ȭ�� ������Ʈ�� ���ٴ� ������ �ٽ� �����ؾ� ���� �ǹ��� 
            return createObj.Dequeue(); // ���� �����ϱ⿡ ť���� ������ ����
        else
            return null; // ���� ���� ������ null�� ����
    }
    public void DisableObject(T obj) // �Ѿ��� ������ų� ���� ���� �� ����Ǵ� �Լ��� ť�� �ٽ� �����͸� �Ҵ��Ͽ� ��Ȱ��ȭ
    {
        createObj.Enqueue(obj); // ť�� ����
        obj.gameObject.SetActive(false); // ��Ȱ��ȭ
    }
}
