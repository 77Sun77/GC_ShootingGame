using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour // 제네릭 활용
{
    public Queue<T> createObj = new Queue<T>(); // 큐 사용 (비활성화 된 오브젝트만 담고있음 : 이미 활성화된 오브젝트를 리턴해서 사용하면 안되기 때문)
    private T prefab; // 생성할 프리팹
    private T newObj; // 생성된 오브젝트 임시 저장
    private int initialSize; // 생성 수
    private Transform parent; // 부모 오브젝트

    public Queue<T> CreateObject(T prefab, int initialSize, Transform parent = null) // 생성자를 통해 초기 오브젝트 생성
    {
        for (int i = 0; i < initialSize; i++)
        {
            newObj = GameObject.Instantiate(prefab, parent);
            newObj.gameObject.SetActive(false);
            createObj.Enqueue(newObj); // 비활성화 된 오브젝트는 큐에 저장
        }
        return createObj;
    }
    public T GetObject() 
    {
        if (createObj.Count > 0) // 큐에 값이 존재하지 않다는 것은 비활성화된 오브젝트가 없다는 것으로 다시 생성해야 함을 의미함 
            return createObj.Dequeue(); // 값이 존재하기에 큐에서 빼내고 리턴
        else
            return null; // 값이 없기 때문에 null을 리턴
    }
    public void DisableObject(T obj) // 총알이 사라지거나 적이 죽을 때 실행되는 함수로 큐에 다시 데이터를 할당하여 비활성화
    {
        createObj.Enqueue(obj); // 큐에 저장
        obj.gameObject.SetActive(false); // 비활성화
    }
}
