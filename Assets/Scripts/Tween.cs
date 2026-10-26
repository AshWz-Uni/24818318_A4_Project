using UnityEngine;

public class Tween
{
    public Transform Target { get; set; }
    public Vector3 StartPos { get; set; }
    public Vector3 EndPos { get; set; }
    public float StartTime { get; set; }
    public float Duration { get; set; }

    public Tween(Transform target, Vector3 startPos, Vector3 endPos, float startTime, float duration)
    {
        Target = target;
        StartPos = startPos;
        EndPos = endPos;
        StartTime = startTime;
        Duration = duration;
    }
}
