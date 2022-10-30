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

    IEnumerator SpawnCherries() {

        while (true) {

            yield return new WaitForSeconds(10f);

            direction = Random.Range(0, 4);

            switch (direction) {
                case 0: // left-right
                    start = new Vector3(-h_edge - 32f, Random.Range(-v_edge, v_edge));
                    end = new Vector3(h_edge + 32f, Random.Range(-v_edge, v_edge));
                    break;
                case 1: // top-bottom
                    start = new Vector3(Random.Range(-h_edge, h_edge), v_edge + 32f);
                    end = new Vector3(Random.Range(-h_edge, h_edge), -v_edge - 32f);
                    break;
                case 2: // right-left
                    start = new Vector3(h_edge + 32f, Random.Range(-v_edge, v_edge));
                    end = new Vector3(-h_edge - 32f, Random.Range(-v_edge, v_edge));
                    break;
                case 3: //bottom-top
                    start = new Vector3(Random.Range(-h_edge, h_edge), -v_edge - 32f);
                    end = new Vector3(Random.Range(-h_edge, h_edge), v_edge + 32f);
                    break;
            }

            enabled = true;
            tweener.AddTween(transform, start, end, 8f);

        }

    }


    // Start is called before the first frame update
    void Start()
    {

        h_edge = Screen.width / 128f;
        v_edge = Screen.height / 128f;

        StartCoroutine(SpawnCherries());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
