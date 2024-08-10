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
        collider.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        collider.enabled = true;

        if (hit.transform != null)
        {
            if (hit.transform.tag == "CollectionItem")
            {
                //CollectionItem targetItem = hit.collider.GetComponent<CollectionItem>();

            }
            else if (hit.transform.tag == "CollectionSlot")
            {
                CollectionSlotManager targetSlot = hit.collider.GetComponent<CollectionSlotManager>();
                if (targetSlot != null && targetSlot != originalSlot)
                {

                    if (targetSlot.monster == null)
                    {

                        targetSlot.SetMonster(this.monster);
                        originalSlot.ClearSlot();

                    }
                    FileManager.LoadGameFile();

                    FileManager.gameFile.collection[targetSlot.index] = targetSlot.monster;
                    FileManager.gameFile.collection[originalSlot.index] = originalSlot.monster;
                    FileManager.SaveGameFile();
                }
            }
            
            collectionManager.DeleteAllCreatedObjects();
            collectionManager.UpdateTeam();
            collectionManager.UpdateCollection();
        }

        transform.position = initialPosition;
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
