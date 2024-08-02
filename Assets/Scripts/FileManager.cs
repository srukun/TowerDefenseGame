using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class FileManager 
{
    [System.Serializable]
    public class GameFile
    {
        public Monster[] team = new Monster[5];
        public List<Monster> collection = new List<Monster>();
    }

    public static GameFile gameFile = new GameFile();
    public static void SetMonsterActive(Monster monster, bool setActive)
    {
        LoadGameFile();
        foreach (Monster teamMember in gameFile.team)
        {
            if (teamMember != null && teamMember.ID == monster.ID)
            {
                teamMember.isActive = setActive;
            }
        }
        SaveGameFile();
    }
    public static void ResetTeamActiveStatus()
    {
        LoadGameFile();
        foreach (Monster teamMember in gameFile.team)
        {
            if(teamMember != null)
            {
                teamMember.isActive = false;
            }

        }
        SaveGameFile();
    }
    public static bool TeamMemberIsActive(Monster monster)
    {
        LoadGameFile();
        foreach(Monster teamMember in gameFile.team)
        {
            if(teamMember != null && teamMember.ID == monster.ID && teamMember.isActive)
            {
                return true;
            }
        }
        return false;
    }
    public static int GetFirstEmptyIndexInTeam()
    {
        LoadGameFile();
        for(int i = 0; i < gameFile.team.Length; i++)
        {
            if (gameFile.team[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
    public static void TameMonster(Monster monster)
    {
        LoadGameFile();

        Debug.Log(GetFirstEmptyIndexInTeam());
        if (GetFirstEmptyIndexInTeam() != -1)
        {
            int index = GetFirstEmptyIndexInTeam();
            gameFile.team[index] = monster;

        }
        else
        {
            gameFile.collection.Add(monster);

        }

        SaveGameFile();
    }
    public static void SaveGameFile()
    {
        string json = JsonConvert.SerializeObject(gameFile, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Game1.json", json);
    }
    public static void LoadGameFile()
    {
        string path = Application.dataPath + "/Game1.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gameFile = JsonConvert.DeserializeObject<GameFile>(json);
        }
        else
        {
            gameFile = new GameFile();
        }

    }
    public static void HealTeam()
    {
        LoadGameFile();
        foreach(Monster mon in gameFile.team)
        {
            if(mon != null)
            {
                mon.currentHealth = mon.baseHealth;
            }
        }
        SaveGameFile();
    }
}
