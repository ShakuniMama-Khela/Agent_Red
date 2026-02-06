using JetBrains.Annotations;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
    [SerializeField] Vector2 forwardRayOffset= new Vector2(0, 0.47f);
    [SerializeField] float forwardRayLength;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float heightRayLength = 0.5f;
    public ObstacleHitData2D ObstacleCheck()
    {
        ObstacleHitData2D hitData = new ObstacleHitData2D();
        //ray start position 
        Vector2 origin = (Vector2)transform.position + forwardRayOffset;


        Vector2 direction = transform.localScale.x > 0
            ? Vector2.right
            : Vector2.left;

        hitData.forwardHit= Physics2D.Raycast(
            origin,
            direction,
            forwardRayLength,
            obstacleLayer
        );
        hitData.forwardHitFound = hitData.forwardHit.collider != null;

        // Debug ray (Scene view)
        Debug.DrawRay(
            origin,
            direction * forwardRayLength,
            hitData.forwardHitFound ? Color.red : Color.green,
            0.1f
        );
        if(hitData.forwardHitFound )
        {
            var heightOrigin = hitData.forwardHit.point + Vector2.up * 0.02f;
            hitData.heightHit = Physics2D.Raycast(
                  heightOrigin,
                  Vector2.up,
                  heightRayLength,
                  obstacleLayer
                  );

            hitData.heightHitFound = hitData.heightHit.collider != null;

            Debug.DrawRay(
            heightOrigin,
            Vector2.up * heightRayLength,
            hitData.heightHitFound ? Color.red : Color.green,
            //Color.yellow,
            0.1f
        );
        }
        return hitData;
    }

}
        public struct ObstacleHitData2D
        {
        public bool heightHitFound;
        public bool forwardHitFound;
        public RaycastHit2D forwardHit;
        public RaycastHit2D heightHit;

        }