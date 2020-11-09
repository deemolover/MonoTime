using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Container prev; // container at front
    public Container next; // container behind
    public Player owner;
    public Player Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
            if (owner != null)
            {
                color = owner.color;
            }
            else
                color = defaultColor;
            
        }
    }
    public Color defaultColor;
    Color color;
    
    public bool OwnerIs(Player player)
    {
        return OwnerIs(player.playerID);
    }

    public bool OwnerIs(int playerID)
    {
        return Owner != null && Owner.playerID == playerID;
    }

    public void CopyFrom(Container source)
    {
        Owner = source.owner;
    }

    public void Reset()
    {
        prev = next = null;
        Owner = null;
    }

    private void Awake()
    {
        Reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
