using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTypesBehaviour : StateMachineBehaviour
{
  
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!SoundManager.instance.sfx2DSource.isPlaying && !GameManager.instance.isPaused){
            if(animator.gameObject.GetComponent<Player>().onWhatGround == GroundType.WOOD){
                SoundManager.instance.PlaySound2D("WoodWalk");
            }
            else if(animator.gameObject.GetComponent<Player>().onWhatGround == GroundType.GRASS){
                SoundManager.instance.PlaySound2D("GrassWalk");
            }
            else if(animator.gameObject.GetComponent<Player>().onWhatGround == GroundType.GRAVEL){
                SoundManager.instance.PlaySound2D("GravelWalk");
            }
        }
        
    }
    
}
