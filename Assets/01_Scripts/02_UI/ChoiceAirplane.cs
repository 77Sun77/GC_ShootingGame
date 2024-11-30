using UnityEngine;

public class ChoiceAirplane : MonoBehaviour
{
    public GameObject ChoiceWindow;
    public GameObject AirplaneObj;
    public void OnClick_Btn()
    {
        ChoiceWindow.SetActive(false);
        AirplaneObj.SetActive(true);
        GameManager.Instance.StartGame();
        EnemyManager.Instance.Init(AirplaneObj.transform);
    }
}
