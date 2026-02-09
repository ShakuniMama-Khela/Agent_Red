using UnityEngine;


[CreateAssetMenu(menuName = "Parkour System/ New Parkour Action")]
public class ParkourActions : ScriptableObject
{
    [SerializeField] string animName;
    [SerializeField]  float minHeight;
    [SerializeField] float maxHeight;

    public bool CheckIfPossible(ObstacleHitData2D hitData, Transform player)
    {
        // to get the height of obstacle  we need to  subracting the y position of  heighthit point
        //  by the yposition of player  
        float height = hitData.heightHit.point.y - player.position.y;
        if (height < minHeight || height > maxHeight)
            return false;

        return true;
    }   
    public string AnimName => animName; 
}
