using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Player player;
    public int playerID;
    public int keyOfHead;
}

public class GameController : MonoBehaviour
{
    public GameObject containerPrefab;
    public int widthCount;
    public int heightCount;
    public float horizontalOffset;
    public float verticalOffset;
    public float widthOfBlock;
    public float heightOfBlock;

    Dictionary<int, Container> containers;
    Dictionary<int, PlayerData> players;

    public bool IsValidLocation((int, int) location)
    {
        return 0 <= location.Item1 && location.Item1 < heightCount &&
            0 <= location.Item2 && location.Item2 < widthCount;
    }

    public (int, int) KeyToLocation(int key)
    {
        return (key / widthCount, key % widthCount);
    }

    public int LocationToKey((int, int) location)
    {
        return location.Item1 * widthCount + location.Item2;
    }

    private void Awake()
    {
        containers = new Dictionary<int, Container>();
        float unit = containerPrefab.GetComponent<Renderer>().bounds.size.x;
        for (int i = 0; i < heightCount; ++i)
        {
            for (int j = 0; j < widthCount; ++j)
            {
                GameObject block = Instantiate(containerPrefab) as GameObject;
                block.transform.position = new Vector3(
                        horizontalOffset + j * widthOfBlock,
                        verticalOffset + i * heightOfBlock,
                        0
                    );
                containers.Add(LocationToKey((j,i)), block.GetComponent<Container>());
            }
        }

        players = new Dictionary<int, PlayerData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRandomGenerator();
    }

    public int RegisterPlayer(Player player)
    {
        if (players.ContainsKey(player.playerID)) return 1;
        var startLoc = (player.startX, player.startY);
        if (!IsValidLocation(startLoc)) return 1;

        var data = new PlayerData();
        data.player = player;
        data.playerID = player.playerID;
        data.keyOfHead = LocationToKey(startLoc);
        players[player.playerID] = data;
        if (player.playerID != Player.RANDOM_PLAYER)
            containers[data.keyOfHead].Owner = player;
        return 0;
    }

    

    public int DoMove(int playerID, int x, int y)
    {
        int key = players[playerID].keyOfHead;
        var loc = KeyToLocation(key);
        var target = (loc.Item1 + x, loc.Item2 + y);
        if (!IsValidLocation(target)) return 1;
        int newKey = LocationToKey(target);
        bool random = (containers[newKey].OwnerIs(Player.RANDOM_PLAYER));
        Container head = containers[key];
        Container newHead = containers[newKey];
        head.prev = newHead;
        newHead.next = head;
        Container curr = newHead;
        while (curr.next != null)
        {
            curr.CopyFrom(curr.next);
            curr = curr.next;
        }
        if (!random)
        {
            curr.prev.next = null;
            curr.Reset();
        }

        players[playerID].keyOfHead = newKey;
        return 0;
    }

    public int DoAttack(int playerID)
    {
        return 0;
    }

    void TryRandomGenerate()
    {
        int rx = Random.Range(0, widthCount);
        int ry = Random.Range(0, heightCount);
        int rkey = LocationToKey((rx, ry));
        if (containers[rkey].Owner == null)
        {
            containers[rkey].Owner = players[Player.RANDOM_PLAYER].player;
        }
    }

    const int RANDOM_INTERVAL = 600;
    int timer = 0;
    void UpdateRandomGenerator()
    {
        if (timer <= 0)
        {
            TryRandomGenerate();
            timer = RANDOM_INTERVAL;
        }
        timer -= 1;
    }
}
