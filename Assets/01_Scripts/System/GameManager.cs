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

    [SerializeField] private int stageCount = 0;
    public StageInfo[] AllStages;
    private StageInfo curStage;
    private List<StageEnemyInfo> curStageEnemyInfo;
    private StageEnemyInfo enemyInfoTemp;
    [SerializeField] private int enemyDeathCount;
    public void EnemyCountPlus() => enemyDeathCount++;

    public TextBox t_Box;
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
        if (stageCount == AllStages.Length)
            return;

        curStage = AllStages[stageCount];
        curStage.ResetValue();
        curStageEnemyInfo = curStage.stageEnemyInfos.ToList();
        StartCoroutine(EnemySpawn());
        t_Box.InitTxtBox("Wave" + (stageCount + 1));
    }
    public void EndStage(bool isWin)
    {
        StopAllCoroutines();

        stageCount = isWin ? stageCount + 1 : stageCount + 0;
        t_Box.InitTxtBox(isWin ? "Win":"");
        if (!isWin)
            SceneManager.LoadScene("GameScene");
        Invoke("NextStage", 2f);
        EnemyManager.Instance.targetAirplane.Heal();
        enemyDeathCount = 0;
        curStage.ResetValue();
    }

    private IEnumerator EnemySpawn()
    {
        
        WaitForSeconds delayTime = new WaitForSeconds(curStage.spawnDelay);
        for (int i=0; i< curStage.GetMaxSpawnCount(); i++)
        {
            enemyInfoTemp = curStageEnemyInfo[Random.Range(0, curStageEnemyInfo.Count)];
            EnemyManager.Instance.SpawnEnemy(enemyInfoTemp.enemyType, enemyInfoTemp.rank);
            enemyInfoTemp.spawnCount--;
            if (enemyInfoTemp.spawnCount == 0)
                curStageEnemyInfo.Remove(enemyInfoTemp);
            yield return delayTime;
        }
        WaitUntil isAllEnemyDie = new WaitUntil(() => enemyDeathCount == curStage.GetMaxSpawnCount());
        yield return isAllEnemyDie;
        EndStage(true);
    }
}
