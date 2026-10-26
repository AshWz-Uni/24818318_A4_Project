using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tweener tweener;
    [SerializeField] private Tilemap[] tilemaps;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float tilesPerSecond = 5f;
    [SerializeField] private Vector3 startPosition = new Vector3(1.5f, 4.5f, 0f);

    [Header("Audio")]
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private AudioClip pelletMoveClip; 
    [SerializeField] private AudioClip pacStudentMoveClip;

    private Vector3 lastInput = Vector3.zero;      // last key pressed, never cleared
    private Vector3 currentInput = Vector3.zero;   // direction actually travelled
    private bool wasMoving;

    void Start()
    {
        if (tweener == null) tweener = FindAnyObjectByType<Tweener>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (tilemaps == null || tilemaps.Length == 0)
            tilemaps = FindObjectsByType<Tilemap>(FindObjectsInactive.Include);
        
        transform.position = startPosition;
        if (movementAudio != null) movementAudio.loop = true;

        SetAnimation(Vector3.right, false);
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

        if (wasMoving)
        {
            EatPellet(transform.position);
            wasMoving = false;
        }

        if (TryMove(lastInput))
        {
            currentInput = lastInput;
        }
        else if (!TryMove(currentInput))
        {
            StopAudio();
            if (animator != null) animator.speed = 0f;
        }
    }

    bool TryMove(Vector3 direction)
    {
        if (direction == Vector3.zero) return false;

        Vector3 start = transform.position;
        Vector3 end = start + direction;
        if (!IsWalkable(end)) return false;

        if (!tweener.AddTween(transform, start, end, 1f / tilesPerSecond)) return false;

        wasMoving = true;
        SetAnimation(direction, true);
        PlayAudio(HasPellet(end));
        return true;
    }

    // ---------------- animation ----------------
    void SetAnimation(Vector3 direction, bool moving)
    {
        if (animator == null) return;

        // Direction parameter: 0 = down, 1 = up, 2 = left, 3 = right
        int value = 3;
        if (direction == Vector3.down) value = 0;
        else if (direction == Vector3.up) value = 1;
        else if (direction == Vector3.left) value = 2;

        animator.SetInteger("Direction", value);
        animator.speed = moving ? 1f : 0f;
    }

    // ---------------- audio ----------------
    void PlayAudio(bool aboutToEat)
    {
        if (movementAudio == null) return;

        AudioClip wanted = aboutToEat ? pelletMoveClip : pacStudentMoveClip;
        if (wanted == null) return;

        if (movementAudio.clip != wanted || !movementAudio.isPlaying)
        {
            movementAudio.clip = wanted;
            movementAudio.Play();
        }
    }

    void StopAudio()
    {
        if (movementAudio != null && movementAudio.isPlaying) movementAudio.Stop();
    }

    // ---------------- tilemap queries ----------------
    bool IsPellet(TileBase t) => t != null && (t.name == "Grid Tiles_4" || t.name == "Grid Tiles_5");

    bool IsWalkable(Vector3 worldPos)
    {
        foreach (Tilemap tm in tilemaps)
        {
            TileBase tile = tm.GetTile(tm.WorldToCell(worldPos));
            if (tile != null && !IsPellet(tile)) return false;   // non-pellet tile = wall
        }
        return true;
    }

    bool HasPellet(Vector3 worldPos)
    {
        foreach (Tilemap tm in tilemaps)
            if (IsPellet(tm.GetTile(tm.WorldToCell(worldPos)))) return true;
        return false;
    }

    void EatPellet(Vector3 worldPos)
    {
        foreach (Tilemap tm in tilemaps)
        {
            Vector3Int cell = tm.WorldToCell(worldPos);
            if (IsPellet(tm.GetTile(cell))) tm.SetTile(cell, null);
        }
    }
}