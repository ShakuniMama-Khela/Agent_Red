
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform target;
   public float smoothSpeed = 5f; 
    public Vector3 offset;

 void LateUpdate()
    {
        float lookAhead = target.GetComponent<Rigidbody2D>().linearVelocity.x*0.2f;
        Vector3 desiredPosition = new Vector3 (
            target.position.x +offset.x+lookAhead,
            target.position.y +offset.y,
            transform.position.z);

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                smoothSpeed * Time.deltaTime
            );
        }
    }

