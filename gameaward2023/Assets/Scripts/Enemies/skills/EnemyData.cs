using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --- “G‚Ì–¼‘O ---
public enum NAME
{
    Skeleton,
    Tate,
    Yoko,
}

// --- ƒXƒLƒ‹–¼ ---
public enum SKILL
{
    up,
    down,
    right,
    left,
    none,
}

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private List<int> replay;
    [SerializeField,HideInInspector]
    private bool save = false;
    [SerializeField,HideInInspector]
    private int[] map;
    [SerializeField,HideInInspector]
    private string[] imageMap;
    [SerializeField,HideInInspector]
    private int mapSize;
    public List<Enemy> enemies/* = new List<Enemy>()*/;
    [SerializeField, HideInInspector]
    private Vector2 heroPos;
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
    public Vector2 GetHeroPos()
    {
        return heroPos;
    }
    public void SetHeroPos(Vector2 data)
    {
        heroPos = data;
    }
    public void SetReplay(List<int> data)
    {
        replay = data;
    }
    public List<int> GetReplay()
    {
        return replay;
    }
}

[System.Serializable]
public class Enemy
{
    
    private string prefabAddress = "Prefabs/Enemies/";

    [SerializeField]
    private string tag = "ŽG‹›“G";

    [SerializeField]
    private NAME name;

    [SerializeField]
    private int health = 50;

    [SerializeField]
    private float moveSpeed = 3.0f;

    [SerializeField]
    private int skillTurn = 3;

    [SerializeField,HideInInspector]
    private Vector2 pos;

    [SerializeField]
    private SKILL skill;

    public string GetAddress()
    {
        if (name.ToString() == "Tate")
            return prefabAddress + "Enemy_02";
        else if (name.ToString() == "Yoko")
            return prefabAddress + "Enemy_03";
        else 
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
    public void SetPos(Vector2 data)
    {
        pos = data;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    
}