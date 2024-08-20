using UnityEngine;
using System.Collections.Generic;
using static FileManager;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;
    public Transform spawnPoint;
    public int numberOfEnemies = 15;
    public float spawnDuration = 45f;

    [Header("Team Settings")]
    public GameObject[] teamPrefabs;
    public List<GameObject> teamUnits;

    [Header("Level Settings")]
    public string missionType;
    public Transform[] waypoints;
    public int numberOfEmerald;
    public bool hasGameEnded = false;
    public Vector3 spawnPosition;
    public float spawnPositionDifference;
    public float movesetUIYOffset;


    [Header("UI References")]
    public GameObject canvas;
    public GameObject cameraObj;
    public GameObject moveset;

    [Header("Dialogue")]
    public DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;
    private float spawnInterval;
    private float spawnCountdown;
    private int spawnedEnemies;
    public bool gameIsPaused;

    [Header("Dialogue Lines")]
    public Dialogue winDialogue;
    public Dialogue loseDialogue;
     
    private void Start()
    {


        dialogueTrigger.TriggerDialogue(false);
        InitializeLevel();
    }

    private void Update()
    {
        if (gameIsPaused || hasGameEnded) return;

        HandleEnemySpawning();
        CheckGameOverCondition();
        CheckWinCondition();
    }

    private void InitializeLevel()
    {
        spawnPoint = waypoints[0];
        spawnInterval = spawnDuration / numberOfEnemies;
        spawnCountdown = 2.5f;
        spawnedEnemies = 0;

        ResetTeamActiveStatus();
        HealTeam();
        SpawnTeam();
    }

    private void HandleEnemySpawning()
    {
        if (spawnedEnemies >= numberOfEnemies) return;

        spawnCountdown -= Time.deltaTime;

        if (spawnCountdown <= 0f)
        {
            SpawnEnemy();
            spawnCountdown = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        int randNum = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randNum], spawnPoint.position, spawnPoint.rotation);

        EnemyController enemyController = enemy.GetComponent<EnemyController>();

        enemyController.waypoints = waypoints;
        enemyController.levelManager = this;

        string[] monsterNames = { "Cotton", "Leaflutter", "Emberdash", "Aquaphion" };
        enemyController.thisCreature = new Monster(monsterNames[randNum], Random.Range(1, 6));

        spawnedEnemies++;
    }

    private void CheckGameOverCondition()
    {
        if (missionType == "Defense" && numberOfEmerald <= 0)
        {
            Debug.Log("test");
            GameOver();
        }
    }

    private void CheckWinCondition()
    {
        if (missionType == "Elimination" && spawnedEnemies >= numberOfEnemies && AreAllEnemiesDefeated())
        {
            WinGame();
        }
        else if (missionType == "Defense" && numberOfEmerald > 0 && spawnedEnemies >= numberOfEnemies && AreAllEnemiesDefeated())
        {
            WinGame();
        }
    }

    private void GameOver()
    {
        if (hasGameEnded) return;

        hasGameEnded = true;
        gameIsPaused = true;

        var dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.OnEndDialogueForLevelComplete += ReturnToMenu;
        dialogueManager.StartDialogue(loseDialogue, true);

    }

    private void WinGame()
    {
        if (hasGameEnded) return;

        hasGameEnded = true;
        gameIsPaused = true;

        var dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.OnEndDialogueForLevelComplete += ReturnToMenu;
        dialogueManager.StartDialogue(winDialogue, true);

    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }


    private bool AreAllEnemiesDefeated()
    {
        EnemyController[] activeEnemies = FindObjectsOfType<EnemyController>();
        return activeEnemies.Length == 0;
    }

    public void SpawnTeam()
    {
        Vector3 spawnPos = spawnPosition;
        LoadGameFile();

        foreach (var creature in gameFile.team)
        {
            if (creature == null || !CanSpawn(creature.ID)) continue;

            GameObject teamCreature = InstantiateTeamCreature(creature, spawnPos);

            var monsterController = teamCreature.GetComponent<MonsterController>();
            monsterController.levelManagerObject = gameObject;
            monsterController.monster = creature;
            monsterController.levelManager = this;

            teamUnits.Add(teamCreature);

            //1.35f
            spawnPos.y -= movesetUIYOffset;
            monsterController.CreateMoveset(moveset, cameraObj.GetComponent<Camera>().WorldToScreenPoint(spawnPos));
            spawnPos.y += movesetUIYOffset;
            spawnPos.x += spawnPositionDifference;
        }
    }

    private GameObject InstantiateTeamCreature(Monster creature, Vector3 spawnPos)
    {
        int prefabIndex = GetPrefabIndex(creature.name);
        return Instantiate(teamPrefabs[prefabIndex], spawnPos, Quaternion.identity);
    }

    private int GetPrefabIndex(string name)
    {
        switch (name)
        {
            case "Cotton": return 0;
            case "Leaflutter": return 1;
            case "Emberdash": return 2;
            case "Aquaphion": return 3;
            default: return 0;
        }
    }

    private bool CanSpawn(string ID)
    {
        return !teamUnits.Exists(creature =>
            creature.GetComponent<MonsterController>().monster.ID == ID &&
            TeamMemberIsActive(creature.GetComponent<MonsterController>().monster));
    }

    public void DeleteTeam()
    {
        for (int i = teamUnits.Count - 1; i >= 0; i--)
        {
            var unit = teamUnits[i];
            var creatureController = unit.GetComponent<MonsterController>();

            if (creatureController != null && !TeamMemberIsActive(creatureController.monster))
            {
                teamUnits.RemoveAt(i);
                Destroy(unit);
            }
        }
    }
    public void PauseGame()
    {
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
    }
}
