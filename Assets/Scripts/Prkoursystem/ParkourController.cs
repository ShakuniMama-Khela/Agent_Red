using UnityEngine;

public class ParkourController : MonoBehaviour 
{
    EnvironmentScanner environmentScanner;
    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
    }
        
      private void Update()
    {
        
        ObstacleHitData2D hitData = environmentScanner.ObstacleCheck();

        if (hitData.forwardHitFound)
        {

           
            Debug.Log("obstacle " + hitData.forwardHit.collider.name);
        }

    }
}

