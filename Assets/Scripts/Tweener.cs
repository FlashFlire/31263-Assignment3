using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{

    //private Tween activeTween;
    private List<Tween> activeTweens = new List<Tween>();

    public bool TweenExists(Transform target) {
        foreach (Tween t in activeTweens) {
            if (t.Target == target) return true;
        }
        return false;
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration) {
        if (!TweenExists(targetObject)) {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        } else {
            return false;
        }
    }


    public void Reset() {
        activeTweens = new List<Tween>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < activeTweens.Count; i++) {

            Tween activeTween = activeTweens[i];

            if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f) {
                float normalisedTime = (Time.time - activeTween.StartTime) / activeTween.Duration;
                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, normalisedTime);
            } else {
                activeTween.Target.position = activeTween.EndPos;
                activeTweens.Remove(activeTween);
                i--; // don't want to skip over any elements, need to decrement the index to not miss stuff
                // (for loop will run with the same index next iteration, but it'll still be the "next" element since we removed one)
            }

        }

    }
}
