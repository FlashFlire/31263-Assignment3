using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovementController : MonoBehaviour
{

    public List<AudioClip> audioClips;
    public Vector3 movementDirection;

    private Tweener tweener;
    private AudioSource audioSource;
    private Animator animator;


    IEnumerator WalkInCircle() {
        while (true) {
            for (int i = 0; i < 5; i++) {
                movementDirection = Vector3.right;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
            for (int i = 0; i < 4; i++) {
                movementDirection = Vector3.up;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
            for (int i = 0; i < 5; i++) {
                movementDirection = Vector3.left;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
            for (int i = 0; i < 4; i++) {
                movementDirection = Vector3.down;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        movementDirection = Vector3.zero;
        tweener = GetComponent<Tweener>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        animator.speed = 0;

        StartCoroutine(WalkInCircle());
    }

    // Update is called once per frame
    void Update()
    {
        if (!tweener.TweenExists(transform)) {

            if (movementDirection == Vector3.zero) {
                animator.speed = 0;
            } else {
                animator.speed = 1;
                tweener.AddTween(transform, transform.position, transform.position + movementDirection, 0.25f);
                // 0.25s to move 1 tile to line up with animation (one tile per 'waka')

                if (movementDirection == Vector3.right) animator.SetInteger("Direction", 0);
                else if (movementDirection == Vector3.up) animator.SetInteger("Direction", 1);
                else if (movementDirection == Vector3.left) animator.SetInteger("Direction", 2);
                else animator.SetInteger("Direction", 3);

                // Play the appropriate sound clip
                // Normally you'd check if the tile you're moving into contains a pellet or not.
                // In this case we just play the pellet sound effect every time
                audioSource.PlayOneShot(audioClips[1]);

                // Zero out the movement direction
                movementDirection = Vector3.zero;
                
            }
            
        }
    }
}
