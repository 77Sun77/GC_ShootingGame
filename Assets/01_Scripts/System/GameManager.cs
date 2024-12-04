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

    [SerializeField] private int stageCount = 0; // �������� ī��Ʈ
    public StageInfo[] AllStages; // ��ũ���ͺ� ������Ʈ�� StageInfo�� �޴� �迭
    private StageInfo curStage; // ���� ���������� ���� StageInfo�� ����
    private List<StageEnemyInfo> curStageEnemyInfo; // ���� StageInfo�� ���� ��ȯ�ؾ� �� �� ������ ����
    private StageEnemyInfo enemyInfoTemp; // ��ũ���ͺ� ������Ʈ�̱⿡ ������ ������ ��ġ�� �ʵ��� �����Ͽ� �Ҵ�
    [SerializeField] private int enemyDeathCount; // ���� ���� ���ڰ� �� ���� ���ڿ� ���� �� ���������� Ŭ�����Ͽ��ٰ� �Ǵ�
    public void EnemyCountPlus() => enemyDeathCount++; // ���� ������ ����

    public TextBox t_Box; // �� ������������ �����ִ� �ؽ�Ʈ
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
        if (stageCount == AllStages.Length) // �ִ� ����������� ����
            return;

        curStage = AllStages[stageCount]; // ���� ���������� StageInfo
        curStage.ResetValue();
        curStageEnemyInfo = curStage.stageEnemyInfos.ToList(); // ������ ����

        StartCoroutine(EnemySpawn());
        t_Box.InitTxtBox("Wave" + (stageCount + 1)); // �������� ���۽� �ؽ�Ʈ ��ȯ
    }
    public void EndStage(bool isWin) // �÷��̾ �װų� ��� ���� �������� �� ����
    {
        StopAllCoroutines();

        stageCount = isWin ? stageCount + 1 : stageCount; 
        t_Box.InitTxtBox(isWin ? "Win":"");

        if (!isWin)
            SceneManager.LoadScene("GameScene"); // �й�� �ٽ� ����

        Invoke("NextStage", 2f);

        EnemyManager.Instance.targetAirplane.Heal(); // ���� ����� �÷��̾� ü�� ȸ��
        enemyDeathCount = 0; // ������ �ʱ�ȭ
        curStage.ResetValue();
    }

    private IEnumerator EnemySpawn()
    {
        
        WaitForSeconds delayTime = new WaitForSeconds(curStage.spawnDelay); // ���� �ֱ⸦ ����
        for (int i=0; i< curStage.GetMaxSpawnCount(); i++) // �� ���� ���ڸ�ŭ �ݺ�
        {
            enemyInfoTemp = curStageEnemyInfo[Random.Range(0, curStageEnemyInfo.Count)]; // ������ ���� �������� �����Ͽ� �ӽ� �Ҵ�

            EnemyManager.Instance.SpawnEnemy(enemyInfoTemp.enemyType, enemyInfoTemp.rank); // ���� ������Ŵ (���� Ÿ��, ���� ��ũ�� ������)
            enemyInfoTemp.spawnCount--;
            if (enemyInfoTemp.spawnCount == 0) // �����ɶ����� -= 1 ���ְ� 0�̶�� ���� ����Ʈ���� ���ܽ��� ���� �����ؼ� �̴� ����Ʈ���� ���ܵ�
                curStageEnemyInfo.Remove(enemyInfoTemp);
            yield return delayTime; // ���� �ֱ⸸ŭ ���
        }
        WaitUntil isAllEnemyDie = new WaitUntil(() => enemyDeathCount == curStage.GetMaxSpawnCount()); // ���� �� == ���� �� ���ڸ� �Ǻ��Ͽ� True�϶��� ���� �۾����� �Ѿ (Until ���)
        yield return isAllEnemyDie;
        EndStage(true); 
    }
}
