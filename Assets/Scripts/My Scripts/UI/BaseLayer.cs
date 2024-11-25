using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLayer : StateMachineBehaviour
{
    GameObject scissors;
    Attack2DContoroll attack2DContoroll;
    AttackContoroll attackContoroll;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
 
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // === ステートが終了したとき ===
        scissors = GameObject.Find("scissors1");

        //GetComponent
        attackContoroll = scissors.GetComponent<AttackContoroll>();
        attack2DContoroll = scissors.GetComponent<Attack2DContoroll>();

        // はさみの当たり判定オフ
        if(attack2DContoroll != null)
            attack2DContoroll.SethitFlg(false);
        if(attackContoroll != null)
            attackContoroll.SethitFlg(false);
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
