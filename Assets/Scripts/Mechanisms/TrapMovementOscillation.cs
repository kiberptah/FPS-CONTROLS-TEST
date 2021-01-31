using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMovementOscillation : MonoBehaviour
{
    public float speed;
    public float distance;
    public directions direction;
    public enum directions
    {
        forward,
        back,
        left,
        right,
        up,
        down
    }

    private Vector3 dir;

    private Dictionary<directions, Vector3> directionsDict = new Dictionary<directions, Vector3>
    {
        {directions.forward, Vector3.forward },
        {directions.back, Vector3.back },
        {directions.left, Vector3.left },
        {directions.right, Vector3.right },
        {directions.up, Vector3.up },
        {directions.down, Vector3.down }
    };

    [SerializeField] private Vector3 nextPos;
    [SerializeField] private Vector3 prevPos;

    Rigidbody rb;
    

    // Start is called before the first frame update
    void Awake()
    {
        TryGetComponent<Rigidbody>(out rb);

        prevPos = transform.position;
        dir = directionsDict[direction];
        nextPos = prevPos + dir * distance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Oscillate();
    }

    void Oscillate()
    {
        if (rb == null)
        {
            transform.Translate(dir * speed * 0.01f); // 0.01f cause otherwise numbers need to be too small!
            //transform.Translate(dir * speed * Time.deltaTime); 
        }
        else
        {
            rb.MovePosition(dir * speed * 0.01f);
        }

        //if (Vector3.Distance(nextPos, transform.position) < 0.1f)
        if (Vector3.Distance(prevPos, transform.position) >= distance)
        {
            dir = -dir;

            Vector3 swapPos = prevPos;
            prevPos = nextPos;
            nextPos = swapPos;

        }
    }
}
