using UnityEngine;
using System.Collections.Generic;
using static FileManager;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

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
    public string nextChapterName;
    public string provinceName;


    [Header("UI References")]
    public GameObject canvas;
    public GameObject cameraObj;
    public GameObject moveset;

    [Header("Level Menu")]
    public TextMeshProUGUI waveCountdown;

    [Header("Dialogue")]
    public DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;
    private float spawnInterval;
    public float spawnCountdown;
    public int spawnedEnemies;
    public bool gameIsPaused;

    [Header("Dialogue Lines")]
    public Dialogue winDialogue;
    public Dialogue loseDialogue;
    public Dialogue chapter3BossDialogue;

    [Header("Chpater3 Boss Fight")]
    public bool isChapter3Boss;


    private void Start()
    {
        dialogueTrigger.TriggerDialogue(false);
        InitializeLevel();

        WaveCountdown();
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
    public void WaveCountdown()
    {
        waveCountdown.text = "Enemies Left: " + (numberOfEnemies - spawnedEnemies - 1);
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
        if (isChapter3Boss && (spawnedEnemies % 5) == 0 && spawnedEnemies <= 30 && spawnedEnemies >= 3)
        {
            SpawnBoss();
        }

        if (spawnedEnemies < numberOfEnemies)
        {
            WaveCountdown(); 

            int randNum = Random.Range(0, 2);
            GameObject enemy = Instantiate(enemyPrefabs[randNum], spawnPoint.position, spawnPoint.rotation);

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.waypoints = waypoints;
            enemyController.levelManager = this;

            string[] monsterNames = { "Cotton", "Leaflutter", "Emberdash" };
            enemyController.thisCreature = new Monster(monsterNames[randNum], Random.Range(1, 6));

            spawnedEnemies++;
        }
    }

    private void SpawnBoss()
    {
        GameObject sheepBoss = Instantiate(enemyPrefabs[0], spawnPoint.position, spawnPoint.rotation);
        sheepBoss.transform.localScale = Vector3.one * 2.25f;
        EnemyController sheepController = sheepBoss.GetComponent<EnemyController>();
        sheepController.waypoints = waypoints;
        sheepController.levelManager = this;

        sheepController.thisCreature = new Monster("Cotton", Random.Range(9, 10));
        sheepController.thisCreature.baseHealth = 150;
        sheepController.thisCreature.currentHealth = 150;
        sheepController.thisCreature.speed = 0.75f;

        SpriteRenderer renderer = sheepController.healthbar[1].GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = new Color(54 / 255f, 110 / 255f, 231 / 255f);
        }

        spawnedEnemies++;
    }



    private void CheckGameOverCondition()
    {
        if (missionType == "Defense" && numberOfEmerald <= 0)
        {
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

        if(provinceName != null && nextChapterName != null)
        {
            FileManager.UnlockChapter(provinceName, nextChapterName);
        }


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
        spawnPos.x -= spawnPositionDifference;
        foreach (var creature in gameFile.team)
        {
            spawnPos.x += spawnPositionDifference;
            if (creature == null || !CanSpawn(creature.ID))
            {
                continue; 
            }
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
