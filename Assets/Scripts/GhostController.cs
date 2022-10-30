using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public int ghostNo;
    private bool isDefeated = false;

    private List<System.Action> movementBehaviours = new List<System.Action>();

    public Tweener tweener;

    private Vector3 targetTile;
    private Vector3 nextTile;

    private Transform pacStudentTransform;
    private StateManager stateManager;
    private Animator animator;
    private AudioSource audioSource;


    private List<Vector3> startingPositions = new List<Vector3>() {
        new Vector3(-5, 3),
        new Vector3(4, 3),
        new Vector3(-5, -3),
        new Vector3(4, -3)
    };


    private List<Vector3> directions = new List<Vector3>() {
        Vector3.right,
        Vector3.up,
        Vector3.left,
        Vector3.down
    }; // for selecting a tile to move to

    private int direction = 0;


    IEnumerator DefeatedBehaviour() {

        GameManager.score += 300;

        isDefeated = true;
        animator.SetTrigger("GhostEaten");
        audioSource.Play();
        GameManager.ghostsDefeated++;

        yield return new WaitWhile(() => tweener.TweenExists(transform));

        tweener.AddTween(transform, transform.position, Vector3.zero, 5f); // move back to ghost house
        yield return new WaitWhile(() => tweener.TweenExists(transform));

        direction = 1;
        animator.SetInteger("Direction", 1);
        animator.SetTrigger("GhostRespawned");
        GameManager.ghostsDefeated--;
        tweener.AddTween(transform, Vector3.zero, new Vector3(0, 2), 0.6f); // move out of ghost house
        yield return new WaitWhile(() => tweener.TweenExists(transform));

        isDefeated = false;

    }


    
    public void Reset() {
        transform.position = startingPositions[ghostNo - 1];
        direction = 0;
    }



    void ghost1Behaviour() {
        // Ghost 1: Move in a random valid direction AWAY from PacStudent

        List<Vector3> possibleDirections = new List<Vector3>() {
            transform.position + Vector3.right,
            transform.position + Vector3.up,
            transform.position + Vector3.left,
            transform.position + Vector3.down
        };

        Vector3 differenceFromPac = pacStudentTransform.position - transform.position;

        for (int i = 0; i < possibleDirections.Count; i++) {
            Vector3 nextDifferenceFromPac = pacStudentTransform.position - possibleDirections[i];
            if (nextDifferenceFromPac.sqrMagnitude < differenceFromPac.sqrMagnitude) { // next direction is closer to pacman
                possibleDirections.RemoveAt(i);
                i--;
            }

        }

        if (possibleDirections.Count == 0) return; // failsafe

        int choice = Random.Range(0, possibleDirections.Count);
        targetTile = new Vector3(possibleDirections[choice].x, possibleDirections[choice].y);

    }

    void ghost2Behaviour() {
        // Ghost 2: Move in a random valid direction TOWARDS PacStudent

        List<Vector3> possibleDirections = new List<Vector3>() {
            transform.position + Vector3.right,
            transform.position + Vector3.up,
            transform.position + Vector3.left,
            transform.position + Vector3.down
        };

        Vector3 differenceFromPac = pacStudentTransform.position - transform.position;

        for (int i = 0; i < possibleDirections.Count; i++) {
            Vector3 nextDifferenceFromPac = pacStudentTransform.position - possibleDirections[i];
            if (nextDifferenceFromPac.sqrMagnitude > differenceFromPac.sqrMagnitude) { // next direction is closer to pacman
                possibleDirections.RemoveAt(i);
                i--;
            }

        }

        if (possibleDirections.Count == 0) return; // failsafe

        int choice = Random.Range(0, possibleDirections.Count);
        targetTile = new Vector3(possibleDirections[choice].x, possibleDirections[choice].y);
    }

    void ghost3Behaviour() {
        // Ghost 3: Move in a random direction with no restrictions

        int choice = Random.Range(0, 4);
        targetTile = transform.position + directions[choice];
    }

    void ghost4Behaviour() {
        // Ghost 4: Move clockwise along the edge of the map

        if (targetTile == Vector3.zero) {
            targetTile = new Vector3(12, 13);
        }

        if (transform.position == targetTile) {
            if (targetTile.x == 12 && targetTile.y == 13) targetTile = new Vector3(12, -13);
            else if (targetTile.x == 12 && targetTile.y == -13) targetTile = new Vector3(-13, -13);
            else if (targetTile.x == -13 && targetTile.y == -13) targetTile = new Vector3(-13, 13);
            else targetTile = new Vector3(12, 13);
        }

    }


    void checkCollision() {
        // check if colliding with PacStudent, handle according to ghost state
        if (Mathf.Abs(pacStudentTransform.position.x - transform.position.x) < 1f && Mathf.Abs(pacStudentTransform.position.y - transform.position.y) < 1f) {
            if (GameManager.ghostScareTime > 0) {
                StartCoroutine(DefeatedBehaviour());
            } else {
                stateManager.PacStudentDefeated();
            }
        }
    }

    void setTargetTile() {
        if (GameManager.ghostScareTime > 0) {
            movementBehaviours[0]();
        } else {
            movementBehaviours[ghostNo - 1]();
        }
    }

    void move() {
        // move towards the next target tile

        int bestDirection = -1;

        Vector3 candidateTile;

        for (int i = 0; i < 4; i++) {
            if (i == (direction + 2) % 4) { // cannot move backwards, don't check this direction
                continue;
            }

            candidateTile = transform.position + directions[i];

            if (LevelLayout.level[(int)candidateTile.x + 14, (int)candidateTile.y + 14] == 1 // direction leads to a wall
                || (candidateTile.x == -9 && candidateTile.y == 0) || (candidateTile.x == 8 && candidateTile.y == 0) // direction leads towards warps on level sides
                || (candidateTile.x == -1 || candidateTile.x == 0) && (candidateTile.y == -2 || candidateTile.y == 2)) { // direction leads into ghost house
                continue;
            }

            if (bestDirection == -1) {
                bestDirection = i;
                nextTile = candidateTile;
            } else {
                if ((targetTile - nextTile).sqrMagnitude > (targetTile - candidateTile).sqrMagnitude) {
                    bestDirection = i;
                    nextTile = candidateTile;
                }
            }

        }

        direction = bestDirection;

        animator.SetInteger("Direction", direction);

        tweener.AddTween(transform, transform.position, nextTile, 0.3f);

    }


    // Start is called before the first frame update
    void Start()
    {
        targetTile = Vector3.zero;

        pacStudentTransform = GameObject.FindWithTag("Player").transform;
        stateManager = GameObject.FindWithTag("Managers").GetComponent<StateManager>();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        movementBehaviours.Add(ghost1Behaviour);
        movementBehaviours.Add(ghost2Behaviour);
        movementBehaviours.Add(ghost3Behaviour);
        movementBehaviours.Add(ghost4Behaviour);

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.gamePlaying) {

            if (!isDefeated) {
                checkCollision();
                if (!tweener.TweenExists(transform)) {
                    setTargetTile();
                    move();
                }
            }

            animator.SetFloat("TimeScared", GameManager.ghostScareTime);

        }
        
    }
}
