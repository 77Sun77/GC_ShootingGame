using UnityEngine;

public class ChoiceAirplane : MonoBehaviour // 플레이어를 선택하기 위한 클래스로 버튼 6개의 클래스가 들어가있으며, 각각 다른 Airplane을 할당한다
{
    public GameObject ChoiceWindow; // UI 최상단 오브젝트
    public GameObject AirplaneObj; // 선택시 연결될 플레이어
    public void OnClick_Btn() // OnClick 함수
    {
        ChoiceWindow.SetActive(false); // UI 비활성화 
        AirplaneObj.SetActive(true); 
        GameManager.Instance.StartGame(); // 플레이어 선택과 동시에 게임 시작
        EnemyManager.Instance.Init(AirplaneObj.transform); // EnemyManager의 Init 실행
    }
}
