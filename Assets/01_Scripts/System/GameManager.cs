using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private int stageCount = 0; // 스테이지 카운트
    public StageInfo[] AllStages; // 스크립터블 오브젝트인 StageInfo를 받는 배열
    private StageInfo curStage; // 현재 스테이지의 대한 StageInfo를 가짐
    private List<StageEnemyInfo> curStageEnemyInfo; // 현재 StageInfo의 대한 소환해야 할 적 정보를 가짐
    private StageEnemyInfo enemyInfoTemp; // 스크립터블 오브젝트이기에 원본에 영향을 끼치지 않도록 복사하여 할당
    [SerializeField] private int enemyDeathCount; // 적의 죽은 숫자가 총 스폰 숫자와 같을 때 스테이지를 클리어하였다고 판단
    public void EnemyCountPlus() => enemyDeathCount++; // 적이 죽으면 실행

    public TextBox t_Box; // 몇 스테이지인지 보여주는 텍스트
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void StartGame()
    {
        Invoke("NextStage", 2f);
    }
    public void NextStage() 
    {
        if (stageCount == AllStages.Length) // 최대 스테이지라면 중지
            return;

        curStage = AllStages[stageCount]; // 현재 스테이지의 StageInfo
        curStage.ResetValue();
        curStageEnemyInfo = curStage.stageEnemyInfos.ToList(); // 데이터 복사

        StartCoroutine(EnemySpawn());
        t_Box.InitTxtBox("Wave" + (stageCount + 1)); // 스테이지 시작시 텍스트 소환
    }
    public void EndStage(bool isWin) // 플레이어가 죽거나 모든 적을 소탕했을 때 실행
    {
        StopAllCoroutines();

        stageCount = isWin ? stageCount + 1 : stageCount; 
        t_Box.InitTxtBox(isWin ? "Win":"");

        if (!isWin)
            SceneManager.LoadScene("GameScene"); // 패배시 다시 시작

        Invoke("NextStage", 2f);

        EnemyManager.Instance.targetAirplane.Heal(); // 라운드 종료시 플레이어 체력 회복
        enemyDeathCount = 0; // 데이터 초기화
        curStage.ResetValue();
    }

    private IEnumerator EnemySpawn()
    {
        
        WaitForSeconds delayTime = new WaitForSeconds(curStage.spawnDelay); // 스폰 주기를 저장
        for (int i=0; i< curStage.GetMaxSpawnCount(); i++) // 총 스폰 숫자만큼 반복
        {
            enemyInfoTemp = curStageEnemyInfo[Random.Range(0, curStageEnemyInfo.Count)]; // 스폰할 적을 랜덤으로 지정하여 임시 할당

            EnemyManager.Instance.SpawnEnemy(enemyInfoTemp.enemyType, enemyInfoTemp.rank); // 적을 스폰시킴 (적의 타입, 적의 랭크를 전해줌)
            enemyInfoTemp.spawnCount--;
            if (enemyInfoTemp.spawnCount == 0) // 스폰될때마다 -= 1 해주고 0이라면 스폰 리스트에서 제외시켜 위에 랜덤해서 뽑는 리스트에서 제외됨
                curStageEnemyInfo.Remove(enemyInfoTemp);
            yield return delayTime; // 스폰 주기만큼 대기
        }
        WaitUntil isAllEnemyDie = new WaitUntil(() => enemyDeathCount == curStage.GetMaxSpawnCount()); // 죽은 적 == 생성 적 숫자를 판별하여 True일때만 이후 작업으로 넘어감 (Until 기능)
        yield return isAllEnemyDie;
        EndStage(true); 
    }
}
