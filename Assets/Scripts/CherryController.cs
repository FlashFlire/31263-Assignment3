using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{

    Vector3 start = new Vector3();
    Vector3 end = new Vector3();
    int direction;

    float h_edge;
    float v_edge;

    public Tweener tweener;

    private Transform pacStudentTransform;


    IEnumerator SpawnCherries() {

        while (true) {

            yield return new WaitForSeconds(10f);

            direction = Random.Range(0, 4);

            switch (direction) {
                case 0: // left-right
                    start = new Vector3(-h_edge - 32f, Random.Range(-v_edge, v_edge));
                    break;
                case 1: // top-bottom
                    start = new Vector3(Random.Range(-h_edge, h_edge), v_edge + 32f);
                    break;
                case 2: // right-left
                    start = new Vector3(h_edge + 32f, Random.Range(-v_edge, v_edge));
                    break;
                case 3: //bottom-top
                    start = new Vector3(Random.Range(-h_edge, h_edge), -v_edge - 32f);
                    break;
            }

            end = new Vector3(-start.x, -start.y);

            enabled = true;
            tweener.AddTween(transform, start, end, 8f);

        }

    }


    public void Reset() {
        enabled = false;
        transform.position = new Vector3(-200, 0);
    }


    // Start is called before the first frame update
    void Start()
    {

        enabled = false;

        h_edge = Screen.width / 128f;
        v_edge = Screen.height / 128f;
        pacStudentTransform = GameObject.FindWithTag("Player").transform;

        StartCoroutine(SpawnCherries());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled && Mathf.Abs(pacStudentTransform.position.x - transform.position.x) < 1f && Mathf.Abs(pacStudentTransform.position.y - transform.position.y) < 1f) {
            enabled = false;
            GameManager.score += 100;
        }
    }
}
