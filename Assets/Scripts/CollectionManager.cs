using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public GameObject[] team = new GameObject[5];
    public List<GameObject> collection = new List<GameObject>();
    public GameObject monsterItem;
    public int pageIndex = 0;
    public List<GameObject> createdObjects;
    void Start()
    {
        UpdateTeam();
        UpdateCollection();
    }


    void Update()
    {

    }
    public void DeleteAllCreatedObjects()
    {
        foreach (GameObject obj in createdObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        createdObjects.Clear();
    }
    public void UpdateTeam()
    {
        FileManager.LoadGameFile();
        for (int i = 0; i < 5; i++)
        {
            team[i].GetComponent<BoxCollider2D>().enabled = false;

            team[i].GetComponent<CollectionSlotManager>().monster = null;

            if (FileManager.gameFile.team[i] != null)
            {
                GameObject item = Instantiate(monsterItem, new Vector3(team[i].transform.position.x, team[i].transform.position.y, -1), Quaternion.identity);
                team[i].GetComponent<CollectionSlotManager>().slotType = "Team";
                item.GetComponent<CollectionItem>().originalSlot = team[i].GetComponent<CollectionSlotManager>();

                item.GetComponent<CollectionItem>().monster = FileManager.gameFile.team[i].GetMonster();
                team[i].GetComponent<CollectionSlotManager>().monster = FileManager.gameFile.team[i].GetMonster();

                item.GetComponent<CollectionItem>().UpdateImage();
                team[i].GetComponent<BoxCollider2D>().enabled = true;
                createdObjects.Add(item);
            }
            team[i].GetComponent<CollectionSlotManager>().index = i;

            team[i].GetComponent<BoxCollider2D>().enabled = true;

        }
    }
    public void UpdateCollection()
    {
        FileManager.LoadGameFile();
        for (int i = 0; i < 21; i++)
        {
            collection[i].GetComponent<BoxCollider2D>().enabled = false;
            collection[i].GetComponent<CollectionSlotManager>().monster = null;
            if (FileManager.gameFile.collection[i + pageIndex * 21] != null)
            {
                GameObject item = Instantiate(monsterItem, new Vector3(collection[i].transform.position.x, collection[i].transform.position.y, -1), Quaternion.identity);
                item.GetComponent<CollectionItem>().monster = FileManager.gameFile.collection[i + pageIndex * 21];
                collection[i].GetComponent<CollectionSlotManager>().monster = FileManager.gameFile.collection[i].GetMonster();
                item.GetComponent<CollectionItem>().originalSlot = collection[i].GetComponent<CollectionSlotManager>();
                item.GetComponent<CollectionItem>().UpdateImage();
                createdObjects.Add(item);
            }
            collection[i].GetComponent<CollectionSlotManager>().slotType = "Collection";
            collection[i].GetComponent<CollectionSlotManager>().index = i + pageIndex * 21;

            collection[i].GetComponent<BoxCollider2D>().enabled = true;

        }
    }
}
