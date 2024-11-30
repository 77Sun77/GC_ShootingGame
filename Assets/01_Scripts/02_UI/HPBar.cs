using UnityEngine;
using UnityEngine.UI;
public class HPBar : MonoBehaviour
{
    public Image value;
    public void UpdateHPBar(AirplaneData airplaneData)
    {
        value.fillAmount = airplaneData.health / airplaneData.maxHealth;
    }

    
}
