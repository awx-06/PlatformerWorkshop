using UnityEngine;

public class FollowCameraClamped : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Collider2D bounds;
    [SerializeField] float smoothTime = 0.15f;

    Camera cam;
    Vector3 velocity;

    void Awake() => cam = GetComponent<Camera>();

    void LateUpdate()
    {
        Vector3 goal = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector3 pos = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Bounds b = bounds.bounds;

        pos.x = Mathf.Clamp(pos.x, b.min.x + halfW, b.max.x - halfW);
        pos.y = Mathf.Clamp(pos.y, b.min.y + halfH, b.max.y - halfH);

        transform.position = pos;
    }
}