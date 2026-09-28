using UnityEngine;
using System.Collections.Generic;

public class Tweener : MonoBehaviour
{
    private List<Tween> tweens = new List<Tween>();

    public bool AddTween(Transform target, Vector3 startPos, Vector3 endPos, float duration)
    {
        // Prevent stacking multiple tweens on the same object
        foreach (Tween existing in tweens)
        {
            if (existing.Target == target)
            {
                return false;
            }
        }

        tweens.Add(new Tween(target, startPos, endPos, Time.time, duration));
        return true;
    }

    public bool IsTweening(Transform target)
    {
        // Check if the target is currently being tweened
        foreach (Tween tween in tweens)
        {
            if (tween.Target == target)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = tweens.Count - 1; i >= 0; i--)
        {
            Tween tween = tweens[i];

            float elapsed = Time.time - tween.StartTime;
            float progress = elapsed / tween.Duration;

            if (progress >= 1f)
            {
                tween.Target.position = tween.EndPos;
                tweens.RemoveAt(i);
            }
            else
            {
                tween.Target.position = tween.StartPos + (tween.EndPos - tween.StartPos) * progress;
            }
        }
    }
}
