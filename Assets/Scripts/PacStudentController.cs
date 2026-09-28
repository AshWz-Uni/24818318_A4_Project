using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 3f;
    [SerializeField] private Tweener tweener;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource movingAudioSource;

    private int currentWaypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = waypoints[0].position;

        if (movingAudioSource != null)
        {
            movingAudioSource.loop = true;
            movingAudioSource.Play();
        }

        StartNextLeg();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tweener.IsTweening(transform))
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            StartNextLeg();
        }
    }

    private void StartNextLeg()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = waypoints[(currentWaypointIndex + 1) % waypoints.Length].position;

        float distance = Vector3.Distance(startPos, endPos);
        float duration = distance / speed;

        tweener.AddTween(transform, startPos, endPos, duration);

        UpdateAnimationDirection(endPos - startPos);
    }

    private void UpdateAnimationDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetInteger("Direction", direction.x > 0 ? 3 : 2);  // Right : Left
        }
        else
        {
            animator.SetInteger("Direction", direction.y > 0 ? 1 : 0);  // Up : Down
        }
    }
}
