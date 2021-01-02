﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseStateBehaviour : StateMachineBehaviour
{
    private EnemyController2 controller;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = animator.gameObject.GetComponent<EnemyController2>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.transform.LookAt(controller.target);
        
        var direction = controller.target.position - controller.transform.position;
        var movementVector = direction.normalized * Time.deltaTime * controller.Speed;

        controller.transform.position += movementVector;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
