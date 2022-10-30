using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGhostController : MonoBehaviour
{

    private Animator animator;
    public Tweener tweener;


    [SerializeField]
    private Transform redGhostTransform;
    // red ghost starts on the top-left corner: use this to "anchor" animation onto one of the corners, then go from there
    private Vector3 topLeft;


    IEnumerator WalkAroundTitle() {

        Vector3 differenceFromTopLeft = topLeft - transform.position;

        tweener.AddTween(transform, transform.position, topLeft, (differenceFromTopLeft.magnitude) / 64f);
        yield return new WaitWhile(() => tweener.TweenExists(transform));
        animator.SetTrigger("Rotate");

        while (true) {
            tweener.AddTween(transform, transform.position, transform.position + new Vector3(272f, 0f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            animator.SetTrigger("Rotate");

            tweener.AddTween(transform, transform.position, transform.position + new Vector3(0f, -272f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            animator.SetTrigger("Rotate");

            tweener.AddTween(transform, transform.position, transform.position + new Vector3(-272f, 0f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            animator.SetTrigger("Rotate");

            tweener.AddTween(transform, transform.position, transform.position + new Vector3(0f, 272f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            animator.SetTrigger("Rotate");
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        topLeft = redGhostTransform.position;

        StartCoroutine(WalkAroundTitle());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
