using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Monster 
{
    public string ID;
    public string name;
    public int level;
    public float experience;
    public float baseHealth;
    public float currentHealth;
    public float speed;
    public float damage;


    public float nextLevelExpThreshold;
    public float expThresholdIncrease;
    public float healthIncrease;
    public float damageIncrease;
    public bool isActive;

    public Move[] moveSet = new Move[3];
    public int activeMoveIndex;
    public Monster()
    {

    }
    public Monster(string ID, string name, int level, float experience, float baseHealth, float speed, float damage, float nextLevelExpThreshold, float expThresholdIncrease, float healthIncrease, float damageIncrease)
    {
        this.ID = ID;
        this.name = name;
        this.level = level;
        this.experience = experience;
        this.baseHealth = baseHealth;
        this.currentHealth = baseHealth; 
        this.speed = speed;
        this.damage = damage;
        this.nextLevelExpThreshold = nextLevelExpThreshold;
        this.expThresholdIncrease = expThresholdIncrease;
        this.healthIncrease = healthIncrease;
        this.damageIncrease = damageIncrease;
        isActive = false;
        Move punch = new Move("Punch", "Basic", 1f, 1f, 7f);
        Move thunderPunch = new Move("Thunder Punch", "Electric", 1.25f, 1f, 11f);

        moveSet[0] = punch;
        moveSet[1] = thunderPunch;
        moveSet[2] = null;
        activeMoveIndex = 0;
    }
    public Monster(string name, int level)
    {
        float baseHealth = 10f + (level - 1) * 1.25f; //  base health scaling
        float speed = 2f + (level - 1) * 0.35f; //  speed scaling
        float damage = 1.5f + (level - 1) * 0.65f; //  damage scaling
        float nextLevelExpThreshold = 30f * Mathf.Pow(1.45f, level - 1); //  exp threshold scaling
        float expThresholdIncrease = 1.45f;
        float healthIncrease = 4f;
        float damageIncrease = 1.5f;

        this.ID = IDGenerator.GenerateID();
        this.name = name;
        this.level = level;
        this.baseHealth = baseHealth;
        this.currentHealth = baseHealth;
        this.speed = speed;
        this.damage = damage;
        this.nextLevelExpThreshold = nextLevelExpThreshold;
        this.expThresholdIncrease = expThresholdIncrease;
        this.healthIncrease = healthIncrease;
        this.damageIncrease = damageIncrease;

        Move punch = new Move("Punch", "Basic", 1f, 1f, 7f);
        Move thunderPunch = new Move("Thunder Punch", "Electric", 1.25f, 1f, 11f);

        moveSet[0] = punch;
        moveSet[1] = thunderPunch;
        moveSet[2] = null;
        activeMoveIndex = 0;
    }
    public Monster GetMonster()
    {
        return this;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
    public float GetDamage()
    {
        return damage + moveSet[activeMoveIndex].damage;
    }

}
