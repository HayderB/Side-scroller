using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float smoothTime = 0.2f;
    public Transform target;
    private Vector3 _velocity = Vector3.zero;

    //Camera contantly follows a target using lateupdate which is a couple of frames behind the update
    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}
