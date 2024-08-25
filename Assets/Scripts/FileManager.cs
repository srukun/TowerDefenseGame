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

    //Key
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
            //no space
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

    /*    public static void SaveGameFile()
        {
            string json = JsonConvert.SerializeObject(gameFile, Formatting.Indented);
            File.WriteAllText(Application.dataPath + "/Game1.json", json);
        }*/

    public static void SaveGameFile()
    {
        string json = JsonConvert.SerializeObject(gameFile, Formatting.Indented);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();  // Ensure it's saved
    }
    /*    public static void LoadGameFile()
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
        }*/

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
        }

        if (gameFile.progression.provinces.Count == 0)
        {
            InitializeProvinces();
        }

    }

    public static void InitializeProvinces()
    {
        gameFile.progression.provinces["Windwar Province"] = new ProvinceData { unlocked = true, chapters = new Dictionary<string, ChapterData>() };
        gameFile.progression.provinces["Windwar Province"].chapters["Introduction"] = new ChapterData { unlocked = false };
        gameFile.progression.provinces["Windwar Province"].chapters["Chapter 1"] = new ChapterData { unlocked = false };
        gameFile.progression.provinces["Windwar Province"].chapters["Chapter 2"] = new ChapterData { unlocked = false };

        gameFile.progression.provinces["Flamevale Province"] = new ProvinceData { unlocked = false, chapters = new Dictionary<string, ChapterData>() };
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

    /* public static bool ProvinceUnlocked(string provinceName)
     {
         LoadGameFile();
         if (gameFile.progression.provinces.ContainsKey(provinceName))
         {
             return gameFile.progression.provinces[provinceName].unlocked;
         }
         return false;
     }

     public static bool ChapterUnlocked(string provinceName, string chapterName)
     {
         LoadGameFile();
         if (gameFile.progression.provinces.ContainsKey(provinceName))
         {
             var province = gameFile.progression.provinces[provinceName];
             if (province.chapters.ContainsKey(chapterName))
             {
                 return province.chapters[chapterName].unlocked;
             }
         }
         return false;
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
             else
             {
                 Debug.Log($"Chapter '{chapterName}' not found in province '{provinceName}'.");
             }
         }
         else
         {
             Debug.Log($"Province '{provinceName}' not found.");
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
                     Debug.Log($"First chapter '{firstChapter.Current}' in province '{provinceName}' has been unlocked.");
                 }
             }

             SaveGameFile();
             Debug.Log($"Province '{provinceName}' has been unlocked.");
         }
         else
         {
             Debug.LogError($"Province '{provinceName}' not found.");
         }
     }*/


    public static bool ProvinceUnlocked(string provinceName)
    {
        LoadGameFile();
        Debug.Log(provinceName);
        return PlayerPrefs.GetInt($"{provinceName}ProvinceUnlocked", 0) == 1;
    }

    public static bool ChapterUnlocked(string provinceName, string chapterName)
    {
        LoadGameFile();

        return PlayerPrefs.GetInt($"{provinceName}{chapterName}Unlocked", 0) == 1;
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

                PlayerPrefs.SetInt($"{provinceName}{chapterName}Unlocked", 1);
                PlayerPrefs.Save();
                SaveGameFile();
            }
            else
            {
            }
        }
        else
        {
        }
    }

    public static void UnlockProvince(string provinceName)
    {
        LoadGameFile();

        if (gameFile.progression.provinces.ContainsKey(provinceName))
        {
            var province = gameFile.progression.provinces[provinceName];

            province.unlocked = true;

            PlayerPrefs.SetInt($"{provinceName}ProvinceUnlocked", 1);
            PlayerPrefs.Save();

            if (province.chapters.Count > 0)
            {
                var firstChapter = province.chapters.Keys.GetEnumerator();
                if (firstChapter.MoveNext())
                {
                    province.chapters[firstChapter.Current].unlocked = true;

                    PlayerPrefs.SetInt($"{provinceName}{firstChapter.Current}Unlocked", 1);
                }
            }

            SaveGameFile(); 
        }
        else
        {
        }
    }

    //progression

    public static void SaveProgression(ProgressionData progressionData)
    {
        PlayerPrefs.SetInt("WindwarProvinceUnlocked", progressionData.provinces["Windwar Province"].unlocked ? 1 : 0);
        PlayerPrefs.SetInt("FlamevaleProvinceUnlocked", progressionData.provinces["Flamevale Province"].unlocked ? 1 : 0);

        PlayerPrefs.SetInt("IntroUnlocked", progressionData.provinces["Windwar Province"].chapters["Introduction"].unlocked ? 1 : 0);
        PlayerPrefs.SetInt("Chapter1Unlocked", progressionData.provinces["Windwar Province"].chapters["Chapter 1"].unlocked ? 1 : 0);
        PlayerPrefs.SetInt("Chapter2Unlocked", progressionData.provinces["Windwar Province"].chapters["Chapter 2"].unlocked ? 1 : 0);

        PlayerPrefs.SetInt("FlamevaleChapter1Unlocked", progressionData.provinces["Flamevale Province"].chapters["Chapter 1"].unlocked ? 1 : 0);
        PlayerPrefs.SetInt("FlamevaleChapter2Unlocked", progressionData.provinces["Flamevale Province"].chapters["Chapter 2"].unlocked ? 1 : 0);

        PlayerPrefs.Save(); 
    }

    public static ProgressionData LoadProgression()
    {
        var progressionData = new ProgressionData
        {
            provinces = new Dictionary<string, ProvinceData>
            {
                { "Windwar Province", LoadProvince("Windwar") },
                { "Flamevale Province", LoadProvince("Flamevale") }
            }
        };

        return progressionData;
    }

    private static ProvinceData LoadProvince(string provinceName)
    {
        return new ProvinceData
        {
            unlocked = PlayerPrefs.GetInt($"{provinceName}ProvinceUnlocked", 0) == 1,
            chapters = new Dictionary<string, ChapterData>
            {
                { "Introduction", LoadChapter($"{provinceName}Intro") },
                { "Chapter 1", LoadChapter($"{provinceName}Chapter1") },
                { "Chapter 2", LoadChapter($"{provinceName}Chapter2") }
            }
        };
    }

    private static ChapterData LoadChapter(string chapterName)
    {
        return new ChapterData
        {
            unlocked = PlayerPrefs.GetInt($"{chapterName}Unlocked", 0) == 1
        };
    }
}
