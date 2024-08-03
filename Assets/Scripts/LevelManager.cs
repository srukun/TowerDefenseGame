using UnityEngine;
using static FileManager;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Transform spawnPoint;   
    public int numberOfEnemies = 15; 
    public float spawnDuration = 45f; 

    private float spawnInterval; 
    private float spawnCountdown; 
    private int spawnedEnemies = 0;
    public Transform[] waypoints;

    public bool gameIsPaused;
    public int numberOfApples;
    public GameObject[] teamPrefabs;
    public List<GameObject> teamUnits;

    public GameObject canvas;
    public GameObject cam;
    void Start()
    {
        spawnPoint = waypoints[0];
        spawnInterval = spawnDuration / numberOfEnemies;
        spawnCountdown = 120f;
        ResetTeamActiveStatus();
        HealTeam();
        SpawnTeam();
    }

    void Update()
    {
        if (!gameIsPaused)
        {
            if (spawnedEnemies < numberOfEnemies)
            {
                spawnCountdown -= Time.deltaTime;

                if (spawnCountdown <= 0f)
                {
                    SpawnEnemy();
                    spawnCountdown = spawnInterval;
                }
            }
            if (numberOfApples <= 0)
            {
                Debug.Log("Game Over");
                gameIsPaused = true;
            }
        }

    }


    void SpawnEnemy()
    {
        if (!gameIsPaused)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemy.GetComponent<EnemyController>().waypoints = waypoints;
            enemy.GetComponent<EnemyController>().levelManager = GetComponent<LevelManager>();
            
            enemy.GetComponent<EnemyController>().thisCreature = new Monster("Cotton", Random.Range(1, 6));
            spawnedEnemies++;
        }
    }
    public bool CanSpawn(string ID)
    {
        foreach (var creature in teamUnits)
        {
            var monster = creature.GetComponent<MonsterController>().monster;
            if (monster.ID == ID && TeamMemberIsActive(monster))
            {
                return false;
            }
        }
        return true;
    }
    public void SpawnTeam()
    {

        Vector3 spawnPoint = new Vector3(-4, -2.75f, -1);
        LoadGameFile();
        foreach (var creature in gameFile.team)
        {

            if (creature != null)
            {
                if (CanSpawn(creature.ID))
                {
                    GameObject teamCreature = Instantiate(teamPrefabs[0], spawnPoint, Quaternion.identity);

                    MonsterController monsterController = teamCreature.GetComponent<MonsterController>();

                    monsterController.levelManager = this.gameObject;
                    monsterController.monster = creature;



                    teamUnits.Add(teamCreature);

                    //Spawn Movest
                    spawnPoint.y -= 1.35f;
                    monsterController.CreateMoveset(canvas, cam.GetComponent<Camera>().WorldToScreenPoint(spawnPoint));
                    spawnPoint.y += 1.35f;

                }
            }
            spawnPoint.x += 2;

        }
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
}
