using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    
    private float yOffset;
    private float zOffset;

    void Start()
    {
        if (target != null)
        {
            yOffset = transform.position.y - target.position.y;
            zOffset = transform.position.z - target.position.z;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = transform.position;
        desiredPosition.y = target.position.y + yOffset;
        desiredPosition.z = target.position.z + zOffset; 
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}