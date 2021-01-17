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
        right
    }

    private Vector3 dir;

    private Dictionary<directions, Vector3> directionsDict = new Dictionary<directions, Vector3>
    {
        {directions.forward, Vector3.forward },
        {directions.back, Vector3.back },
        {directions.left, Vector3.left },
        {directions.right, Vector3.right }
    };

    [SerializeField] private Vector3 nextPos;
    [SerializeField] private Vector3 prevPos;

    // Start is called before the first frame update
    void Awake()
    {
        prevPos = transform.position;
        dir = directionsDict[direction];
        nextPos = prevPos + dir * distance;
    }

    // Update is called once per frame
    void Update()
    {
        Oscillate();
    }

    void Oscillate()
    {
        transform.Translate(dir * speed * Time.deltaTime);

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
