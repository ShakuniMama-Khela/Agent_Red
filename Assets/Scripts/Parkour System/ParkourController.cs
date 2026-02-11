using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour 
{
    [SerializeField] List<ParkourActions> parkouractions;
    bool inAction;  // creating varaible to tarck the animation is played or not 
    EnvironmentScanner environmentScanner;
    Animator animator; // to play animations 
    PlayerMovement playerMovement;

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
        
      private void Update()
      {
        if(Input.GetButton("Jump") && !inAction)
        {
            ObstacleHitData2D hitData = environmentScanner.ObstacleCheck();
            if (hitData.forwardHitFound)
            {
                foreach (var action in parkouractions)
                {
                    if (action.CheckIfPossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;

                    }
                }
            }
        }
      }
    IEnumerator DoParkourAction(ParkourActions action)
    {
        inAction = true;
        playerMovement.SetControl(false);
        animator.CrossFade(action.AnimName, 0.2f);
        yield return null;
        var animstate = animator.GetNextAnimatorStateInfo(0);
        yield return new WaitForSeconds(animstate.length);
        playerMovement.SetControl(true);
        inAction = false;
    }                

}

