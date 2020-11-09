using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static readonly int RANDOM_PLAYER = -1;

    public int playerID;

    public GameObject controllerObject;
    GameController controller;

    public int startX;
    public int startY;

    public bool controllable;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode attackKey;

    public Color color;

    Dictionary<KeyCode, System.Action<KeyCode>> behaviors;
    Dictionary<KeyCode, (int, int)> moveDirections;

    private void Awake()
    {
        controller = controllerObject.GetComponent<GameController>();
        behaviors = new Dictionary<KeyCode, System.Action<KeyCode>>();
        if (controllable)
        {
            behaviors[upKey] = behaviors[downKey] = behaviors[leftKey] = behaviors[rightKey] = OnMoveInput;
            behaviors[attackKey] = OnAttackInput;
            moveDirections = new Dictionary<KeyCode, (int, int)>() {
                {upKey,   (0, 1) },
                {downKey, (0, -1)},
                {leftKey, (-1, 0)},
                {rightKey,(1, 0) }
            };
        }
        
    }

    void OnMoveInput(KeyCode code)
    {
        Debug.Log(code);
        var direction = moveDirections[code];
        if (controller.DoMove(playerID, direction.Item1, direction.Item2) != 0)
        {
            Debug.Log("failed");
        }
    }

    void OnAttackInput(KeyCode code)
    {
        controller.DoAttack(playerID);
    }

    void CheckInput()
    {
        foreach (var pair in behaviors)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                pair.Value(pair.Key);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        controller.RegisterPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }
}
