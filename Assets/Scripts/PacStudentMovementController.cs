using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentMovementController : MonoBehaviour
{

    public List<AudioClip> audioClips;
    private Vector3 lastInput;
    private Vector3 currentInput;

    public Tweener tweener;
    private AudioSource audioSource;
    private Animator animator;

    private Tilemap map;


    IEnumerator WalkInCircle() { // old
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


    private void faceDirection() {
        if (currentInput == Vector3.right) transform.eulerAngles = Vector3.zero;
        else if (currentInput == Vector3.up) transform.eulerAngles = new Vector3(0f, 0f, 90f);
        else if (currentInput == Vector3.left) transform.eulerAngles = new Vector3(0f, 0f, 180f);
        else if (currentInput == Vector3.down) transform.eulerAngles = new Vector3(0f, 0f, 270f);
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
            GameManager.score += 10;
        } else if (LevelLayout.level[nextPos.x + 14, nextPos.y + 14] == 3) {
            LevelLayout.level[nextPos.x + 14, nextPos.y + 14] = 0;
            map.SetTile(nextPos, null);
            audioSource.clip = audioClips[1];
            GameManager.score += 10;
            // collected a power pellet, call some function on the ghosts
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


                audioSource.Play();

            }
    }


    // Start is called before the first frame update
    void Start()
    {
        currentInput = Vector3.zero;
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

            // if we exited through the left / right tunnel, teleport to the other side
            if (transform.position.x < -13) {
                transform.position = new Vector3(12, transform.position.y);
            } else if (transform.position.x > 12) {
                transform.position = new Vector3(-13, transform.position.y);
            }

            // store the direction we're currently moving so it doesn't change during the tween
            currentInput = new Vector3(lastInput.x, lastInput.y);


            faceDirection();
            checkCollisions();
            move();
        }
        
    }
}
