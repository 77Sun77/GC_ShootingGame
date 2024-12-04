using UnityEngine;

public class ChoiceAirplane : MonoBehaviour // �÷��̾ �����ϱ� ���� Ŭ������ ��ư 6���� Ŭ������ ��������, ���� �ٸ� Airplane�� �Ҵ��Ѵ�
{
    public GameObject ChoiceWindow; // UI �ֻ�� ������Ʈ
    public GameObject AirplaneObj; // ���ý� ����� �÷��̾�
    public void OnClick_Btn() // OnClick �Լ�
    {
        ChoiceWindow.SetActive(false); // UI ��Ȱ��ȭ 
        AirplaneObj.SetActive(true); 
        GameManager.Instance.StartGame(); // �÷��̾� ���ð� ���ÿ� ���� ����
        EnemyManager.Instance.Init(AirplaneObj.transform); // EnemyManager�� Init ����
    }
}
