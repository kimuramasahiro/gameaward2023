using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class AutoRun : MonoBehaviour
{
    [SerializeField]
    private List<int> dir;
    PlayerMovement player;

    public List<int> GetDir()
    {
        return dir;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        
        player.Walk += AddPlayerMove;
    }
    void AddPlayerMove()
    {
        if(player.PressKey_W)
        {
            dir.Add(0);
        }
        else if (player.PressKey_A)
        {
            dir.Add(1);
        }
        else if (player.PressKey_S)
        {
            dir.Add(2);
        }
        else if (player.PressKey_D)
        {
            dir.Add(3);
        }
    }

    
}
