using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    public Image value; // ü�¿� ���� ȭ�鿡 �޶����� �̹���
    public void UpdateHPBar(AirplaneData airplaneData) // ü���� ���̰ų� ȸ���ɶ����� ����
    {
        value.fillAmount = airplaneData.health / airplaneData.maxHealth;
    }

    
}
