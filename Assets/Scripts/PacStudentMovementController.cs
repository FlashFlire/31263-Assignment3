using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentMovementController : MonoBehaviour
{

    public List<AudioClip> audioClips;
    private Vector3 lastInput;
    private Vector3 currentInput;

    private Tweener tweener;
    private AudioSource audioSource;
    private Animator animator;

    private Tilemap map;


    IEnumerator WalkInCircle() {
        while (true) {
            for (int i = 0; i < 5; i++) {
                currentInput = Vector3.right;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
            for (int i = 0; i < 4; i++) {
                currentInput = Vector3.up;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
            for (int i = 0; i < 5; i++) {
                currentInput = Vector3.left;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
            for (int i = 0; i < 4; i++) {
                currentInput = Vector3.down;
                yield return new WaitWhile(() => tweener.TweenExists(transform));
            }
        }
    }


    private void readInputs() {
        if (Input.GetKeyDown("w")) {
            lastInput = Vector3.up;
        }
        else if (Input.GetKeyDown("a")) {
            lastInput = Vector3.left;
        }
        else if (Input.GetKeyDown("s")) {
            lastInput = Vector3.down;
        }
        else if (Input.GetKeyDown("d")) {
            lastInput = Vector3.right;
        }
    }

    private void checkCollisions() {

        Vector3Int nextPos = Vector3Int.FloorToInt(transform.position + currentInput);
        // this method is only ever run at the start of a tween, so there shouldn't ever be any issues with the rounding of this

        // if tile is a wall tile, do not let pac-student move there
        // if tile is a pellet tile, remove it
        // otherwise, do nothing

        if (LevelLayout.level[nextPos.x + 14, nextPos.y + 14] == 1) {
            lastInput = Vector3.zero;
            currentInput = Vector3.zero;
        } else if (LevelLayout.level[nextPos.x + 14, nextPos.y + 14] == 2) {
            LevelLayout.level[nextPos.x + 14, nextPos.y + 14] = 0;
            map.SetTile(nextPos, null);
            audioSource.clip = audioClips[1];
        } else {
            audioSource.clip = audioClips[0];
        }



    }

    private void move() {

            if (currentInput == Vector3.zero) {
                animator.speed = 0;
            } else {
                animator.speed = 1;
                tweener.AddTween(transform, transform.position, transform.position + currentInput, 0.25f);
                // 0.25s to move 1 tile to line up with animation (one tile per 'waka')

                if (currentInput == Vector3.right) animator.SetInteger("Direction", 0);
                else if (currentInput == Vector3.up) animator.SetInteger("Direction", 1);
                else if (currentInput == Vector3.left) animator.SetInteger("Direction", 2);
                else animator.SetInteger("Direction", 3);

                audioSource.Play();

            }
    }


    // Start is called before the first frame update
    void Start()
    {
        currentInput = Vector3.zero;
        tweener = GetComponent<Tweener>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        animator.speed = 0;

        map = GameObject.FindWithTag("Level").GetComponent<Tilemap>();

        //StartCoroutine(WalkInCircle());
    }

    // Update is called once per frame
    void Update()
    {

        readInputs();

        if (!tweener.TweenExists(transform)) {

            // store the direction we're currently moving so it doesn't change during the tween
            currentInput = new Vector3(lastInput.x, lastInput.y);

            checkCollisions();
            move();
        }
        
    }
}
