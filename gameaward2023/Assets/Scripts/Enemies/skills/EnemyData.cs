using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --- “G‚Ì–¼‘O ---
public enum NAME
{
    Skeleton,
    Enemy_02,
    Enemy_03,
}

// --- ƒXƒLƒ‹–¼ ---
public enum SKILL
{
    up,
    down,
    right,
    left,
}

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField,HideInInspector]
    private bool save = false;
    [SerializeField,HideInInspector]
    private int[] map;
    [SerializeField,HideInInspector]
    private string[] imageMap;
    [SerializeField,HideInInspector]
    private int mapSize;
    public List<Enemy> enemies = new List<Enemy>();
    public int[] GetMap()
    {
        return map;
    }
    public void SetMap(int[] data)
    {
        map = data;
    }
    public string[] GetImageMap()
    {
        return imageMap;
    }
    public void SetImageMap(string[] data)
    {
        imageMap = data;
    }
    public void Save()
    {
        save = true;
    }
    public void Reset()
    {
        save = false;
    }
    public bool isSaved()
    {
        return save;
    }
    public int GetMapSize()
    {
        return mapSize;
    }
    public void SetMapSize(int size)
    {
        mapSize = size;
    }
}

[System.Serializable]
public class Enemy
{
    
    private string prefabAddress = "Prefabs/Enemies/";

    [SerializeField]
    private string tag = "ŽG‹›“G‚P";

    [SerializeField]
    private NAME name;

    [SerializeField]
    private int health;

    [SerializeField]
    private float moveSpeed = 3.0f;

    [SerializeField]
    private int skillTurn;

    [SerializeField]
    private Vector2 pos;

    [SerializeField]
    private SKILL skill;

    public string GetAddress()
    {
        return prefabAddress + name.ToString();
    }
    public NAME GetName()
    {
        return name;
    }
    public string GetTag()
    {
        return tag;
    }
    public int GetHealth()
    {
        return health;
    }
    public int GetSkillTurn()
    {
        return skillTurn;
    }

    public SKILL GetSkill()
    {
        return skill;
    }
    public Vector2 GetPos()
    {
        return pos;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    
}