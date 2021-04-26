using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float radius = 0.5f;
    public Vector3 finishPos;
    public float speed = 0.5f;

    private float initalspeed;
    private Vector3 _startPos;
    Vector3 currentEndGoal;

    //Set start position, end position, and speed of platforms
    void Start()
    {
        _startPos = transform.position;
        currentEndGoal = finishPos;
        speed *= Time.deltaTime;
        initalspeed = speed;
    }

    //Shows path of each platform
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, finishPos);
    }

    //Check if platform has reached its destination then wait before moving back
    void Update()
    {
        if (Vector2.Distance(transform.position, currentEndGoal) <= radius)
        {
            if (currentEndGoal == _startPos)
            {
                currentEndGoal = finishPos;
            } else
            {
                currentEndGoal = _startPos;
            }
            speed = 0;
            StartCoroutine(StopTime());
        }
        transform.Translate((finishPos - _startPos) * speed);
    }

    //Platforms wait 2 seconds before going back
    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(2);
        initalspeed = -initalspeed;
        speed = initalspeed;
    }
}
