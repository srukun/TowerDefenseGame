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
        public Monster[] collection = new Monster[105];

        public ProgressionData progression = new ProgressionData();
    }

    [System.Serializable]
    public class ProgressionData
    {
        public Dictionary<string, ProvinceData> provinces = new Dictionary<string, ProvinceData>();
    }

    [System.Serializable]
    public class ProvinceData
    {
        public bool unlocked;
        public Dictionary<string, ChapterData> chapters = new Dictionary<string, ChapterData>();
    }

    [System.Serializable]
    public class ChapterData
    {
        public bool unlocked;
    }

    public static GameFile gameFile = new GameFile();

    // Key
    private const string SaveKey = "GameFileSaveData";

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
            if (teamMember != null)
            {
                teamMember.isActive = false;
            }
        }
        SaveGameFile();
    }

    public static bool TeamMemberIsActive(Monster monster)
    {
        LoadGameFile();
        foreach (Monster teamMember in gameFile.team)
        {
            if (teamMember != null && teamMember.ID == monster.ID && teamMember.isActive)
            {
                return true;
            }
        }
        return false;
    }

    public static int GetFirstEmptyIndexInTeam()
    {
        LoadGameFile();
        for (int i = 0; i < gameFile.team.Length; i++)
        {
            if (gameFile.team[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public static int GetFirstEmptyIndexInCollection()
    {
        LoadGameFile();
        for (int i = 0; i < gameFile.collection.Length; i++)
        {
            if (gameFile.collection[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public static void TameMonster(Monster monster)
    {
        LoadGameFile();
        if (GetFirstEmptyIndexInCollection() == -1 && GetFirstEmptyIndexInTeam() == -1)
        {
            return;
        }
        if (GetFirstEmptyIndexInTeam() != -1)
        {
            int index = GetFirstEmptyIndexInTeam();
            gameFile.team[index] = monster;
        }
        else
        {
            if (GetFirstEmptyIndexInCollection() == -1)
            {
                return;
            }
            int index = GetFirstEmptyIndexInCollection();
            gameFile.collection[index] = monster;
        }
        SaveGameFile();
    }

    public static void SaveGameFile()
    {
        string json = JsonConvert.SerializeObject(gameFile);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void LoadGameFile()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            gameFile = JsonConvert.DeserializeObject<GameFile>(json);
        }
        else
        {
            gameFile = new GameFile();
            InitializeProvinces();
            SaveGameFile();
        }

        if (gameFile.progression.provinces == null || gameFile.progression.provinces.Count == 0)
        {
            InitializeProvinces();
            SaveGameFile();
        }
    }

    public static void InitializeProvinces()
    {
        gameFile.progression = new ProgressionData
        {
            provinces = new Dictionary<string, ProvinceData>()
        };

        gameFile.progression.provinces["Windwar Province"] = new ProvinceData
        {
            unlocked = true,
            chapters = new Dictionary<string, ChapterData>()
        };

        gameFile.progression.provinces["Windwar Province"].chapters["Introduction"] = new ChapterData { unlocked = false };
        gameFile.progression.provinces["Windwar Province"].chapters["Chapter 1"] = new ChapterData { unlocked = false };
        gameFile.progression.provinces["Windwar Province"].chapters["Chapter 2"] = new ChapterData { unlocked = false };

        gameFile.progression.provinces["Flamevale Province"] = new ProvinceData
        {
            unlocked = false,
            chapters = new Dictionary<string, ChapterData>()
        };

        gameFile.progression.provinces["Flamevale Province"].chapters["Chapter 1"] = new ChapterData { unlocked = false };
        gameFile.progression.provinces["Flamevale Province"].chapters["Chapter 2"] = new ChapterData { unlocked = false };
    }

    public static void ResetSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        gameFile = new GameFile();
    }

    public static void HealTeam()
    {
        LoadGameFile();
        foreach (Monster mon in gameFile.team)
        {
            if (mon != null)
            {
                mon.currentHealth = mon.baseHealth;
            }
        }
        SaveGameFile();
    }

    public static string GetImage(string name)
    {
        switch (name)
        {
            case "Cotton":
                return "Monsters/Cotton/CottonForward1";
            case "Leaflutter":
                return "Monsters/Leaflutter/LeafflutterForward1";
            case "Emberdash":
                return "Monsters/Emberdash/EmberdashFront1";
            case "Aquaphion":
                return "Monsters/Aquaphion/AquaphionForward1";
            default:
                return null;
        }
    }

    public static bool ProvinceUnlocked(string provinceName)
    {
        LoadGameFile();
        return gameFile.progression.provinces.ContainsKey(provinceName) && gameFile.progression.provinces[provinceName].unlocked;
    }

    public static bool ChapterUnlocked(string provinceName, string chapterName)
    {
        LoadGameFile();
        return gameFile.progression.provinces.ContainsKey(provinceName) && gameFile.progression.provinces[provinceName].chapters.ContainsKey(chapterName) && gameFile.progression.provinces[provinceName].chapters[chapterName].unlocked;
    }

    public static void UnlockChapter(string provinceName, string chapterName)
    {
        LoadGameFile();

        if (gameFile.progression.provinces.ContainsKey(provinceName))
        {
            var province = gameFile.progression.provinces[provinceName];
            if (province.chapters.ContainsKey(chapterName))
            {
                province.chapters[chapterName].unlocked = true;
                SaveGameFile();
            }
        }
    }

    public static void UnlockProvince(string provinceName)
    {
        LoadGameFile();

        if (gameFile.progression.provinces.ContainsKey(provinceName))
        {
            var province = gameFile.progression.provinces[provinceName];
            province.unlocked = true;

            if (province.chapters.Count > 0)
            {
                var firstChapter = province.chapters.Keys.GetEnumerator();
                if (firstChapter.MoveNext())
                {
                    province.chapters[firstChapter.Current].unlocked = true;
                }
            }

            SaveGameFile();
        }
    }

    public static ProgressionData LoadProgression()
    {
        return gameFile.progression;
    }
}
