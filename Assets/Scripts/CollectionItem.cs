using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionItem : MonoBehaviour
{
    private bool isDragging;
    private Vector3 initialPosition;
    public Monster monster;
    public SpriteRenderer spriteRenderer; 

    public CollectionSlotManager originalSlot;
    private bool isInitializing = true;
    public CollectionManager collectionManager;


    void Start()
    {
        initialPosition = transform.position;
        UpdateImage();
        StartCoroutine(DisableInitializationAfterFrame());
        collectionManager = GameObject.Find("CollectionManager").GetComponent<CollectionManager>();
    }

    void Update()
    {

    }
    private IEnumerator DisableInitializationAfterFrame()
    {
        yield return new WaitForEndOfFrame(); 
        isInitializing = false;
    }
    private void OnMouseDown()
    {
        isDragging = true;
        initialPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, -9);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        Collider2D collider = GetComponent<Collider2D>();
        //.enabled = false;
        int layerMask = LayerMask.GetMask("Slot");

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

        //collider.enabled = true;


        if (hit.transform != null && hit.transform != transform)
        {
            CollectionSlotManager targetSlot = hit.collider.GetComponent<CollectionSlotManager>();
            CollectionSlotManager thisSlot = originalSlot;


            if (targetSlot != null && targetSlot != originalSlot)
            {
                if (thisSlot.slotType == "Team" && targetSlot.slotType == "Collection")
                {

                    SwapMonsters(thisSlot, targetSlot, "TeamToCollection");
                }
                else if (thisSlot.slotType == "Collection" && targetSlot.slotType == "Team")
                {
                    SwapMonsters(thisSlot, targetSlot, "CollectionToTeam");
                }
                else if (thisSlot.slotType == targetSlot.slotType)
                {

                    string swapType = thisSlot.slotType == "Team" ? "TeamToTeam" : "CollectionToCollection";
                    SwapMonsters(thisSlot, targetSlot, swapType);
                }

            }
        }
        transform.position = initialPosition;
        collectionManager.DeleteAllCreatedObjects();
        collectionManager.UpdateTeam();
        collectionManager.UpdateCollection();
    }

    private void SwapMonsters(CollectionSlotManager thisSlot, CollectionSlotManager targetSlot, string swapType)
    {
        Monster tempMonster = targetSlot.monster;

        targetSlot.SetMonster(this.monster);
        thisSlot.SetMonster(tempMonster);


        switch (swapType)
        {
            case "TeamToCollection":
                FileManager.LoadGameFile();


                FileManager.gameFile.team[thisSlot.index] = thisSlot.monster;
                FileManager.gameFile.collection[targetSlot.index] = targetSlot.monster;

                FileManager.SaveGameFile();
                break;

            case "CollectionToTeam":
                FileManager.LoadGameFile();

                FileManager.gameFile.team[targetSlot.index] = targetSlot.monster;
                FileManager.gameFile.collection[thisSlot.index] = thisSlot.monster;

                FileManager.SaveGameFile();
                break;

            case "TeamToTeam":
                FileManager.LoadGameFile();
                FileManager.gameFile.team[targetSlot.index] = targetSlot.monster;
                FileManager.gameFile.team[thisSlot.index] = thisSlot.monster;
                FileManager.SaveGameFile();
                break;

            case "CollectionToCollection":
                FileManager.LoadGameFile();
                FileManager.gameFile.collection[targetSlot.index] = targetSlot.monster;
                FileManager.gameFile.collection[thisSlot.index] = thisSlot.monster;
                FileManager.SaveGameFile();
                break;


            default:
                Debug.LogWarning("Unhandled swap type: " + swapType);
                break;
        }
    }




    public void UpdateImage()
    {
        if (monster != null)
        {
            string path = FileManager.GetImage(monster.name);
            if (!string.IsNullOrEmpty(path))
            {
                Sprite sprite = Resources.Load<Sprite>(path);
                if (sprite != null)
                {
                    spriteRenderer.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("Sprite not found at path: " + path);
                }
            }
            else
            {
                Debug.LogWarning("Invalid monster name: " + monster.name);
            }
        }
    }
}
