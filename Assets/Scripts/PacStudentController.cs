using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private Tweener tweener;
    [SerializeField] private float tilesPerSecond = 5f;
    [SerializeField] private Tilemap[] tilemaps;
    [SerializeField] private Vector3 startPosition = new Vector3(1.5f, 4.5f, 0f);

    private Vector3 lastInput = Vector3.zero;      // last key pressed, never cleared
    private Vector3 currentInput = Vector3.zero;   // direction we are actually travelling

    void Start()
    {
        transform.position = startPosition;
    }

    void Update()
    {
        // Only changes lastInput when a new key is pressed
        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.aKey.wasPressedThisFrame) lastInput = Vector3.left;
            if (kb.dKey.wasPressedThisFrame) lastInput = Vector3.right;
            if (kb.sKey.wasPressedThisFrame) lastInput = Vector3.down;
            if (kb.wKey.wasPressedThisFrame) lastInput = Vector3.up;
        }

        if (tweener.IsTweening(transform)) return;

        if (TryMove(lastInput))
        {
            currentInput = lastInput;
        }
        else
        {
            TryMove(currentInput); 
        }
    }

    bool TryMove(Vector3 direction)
    {
        if (direction == Vector3.zero) return false;

        Vector3 start = transform.position;
        Vector3 end = start + direction;
        if (!IsWalkable(end)) return false;

        return tweener.AddTween(transform, start, end, 1f / tilesPerSecond);
    }

    bool IsWalkable(Vector3 worldPos)
    {
        foreach (Tilemap tm in tilemaps)
        {
            TileBase tile = tm.GetTile(tm.WorldToCell(worldPos));
            if (tile != null && tile.name != "Grid Tiles_4" && tile.name != "Grid Tiles_5")
                return false; // any tile that isn't a pellet is a wall
        }
        return true;
    }
}