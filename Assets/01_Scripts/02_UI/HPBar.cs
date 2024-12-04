using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    public Image value; // 체력에 따라 화면에 달라지는 이미지
    public void UpdateHPBar(AirplaneData airplaneData) // 체력이 깎이거나 회복될때마다 실행
    {
        value.fillAmount = airplaneData.health / airplaneData.maxHealth;
    }

    
}
