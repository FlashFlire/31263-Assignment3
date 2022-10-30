using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePacStudentController : MonoBehaviour
{

    public Tweener tweener;

    IEnumerator WalkAroundTitle() {

        tweener.AddTween(transform, transform.position, transform.position + new Vector3(152f, 0f), 2.375f);
        yield return new WaitWhile(() => tweener.TweenExists(transform));
        transform.Rotate(0f, 0f, -90f);

        while (true) {
            tweener.AddTween(transform, transform.position, transform.position + new Vector3(0f, -272f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            transform.Rotate(0f, 0f, -90f);

            tweener.AddTween(transform, transform.position, transform.position + new Vector3(-272f, 0f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            transform.Rotate(0f, 0f, -90f);

            tweener.AddTween(transform, transform.position, transform.position + new Vector3(0f, 272f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            transform.Rotate(0f, 0f, -90f);

            tweener.AddTween(transform, transform.position, transform.position + new Vector3(272f, 0f), 4.25f);
            yield return new WaitWhile(() => tweener.TweenExists(transform));
            transform.Rotate(0f, 0f, -90f);

        }

    }


    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(WalkAroundTitle());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
