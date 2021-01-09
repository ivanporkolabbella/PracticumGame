using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{
    public Dictionary<Type, StateMachineState> states = new Dictionary<Type, StateMachineState>();
    public StateMachineState currentState;
    public StateMachineState anyState;

    public void InitializeMachine(Type initialState)
    {
        currentState = states[initialState];
        currentState.OnStateEnter();
    }

    public virtual void Update()
    {
        ProcessTrasitionsForState(currentState);

        if (anyState != null)
        {
            ProcessTrasitionsForState(anyState);
        }

        currentState.Update();
    }

    private void ProcessTrasitionsForState(StateMachineState state)
    {
        foreach (var transtion in state.transitions)
        {
            if (transtion.Evaluate())
            {
                ChangeState(states[transtion.TargetState]);
                return;
            }
        }
    }

    private void ChangeState(StateMachineState state)
    {
        if (state == currentState) return;

        currentState.OnStateExit();
        state.OnStateEnter();
        currentState = state;
    }

    public void AddState(StateMachineState state)
    {
        states[state.GetType()] = state;
        state.parentMachine = this;
        state.SetupTransitions();
    }

    public void AddAnyState(StateMachineState state)
    {
        anyState = state;
        anyState.parentMachine = this;
        anyState.SetupTransitions();
    } 
}

public abstract class StateMachineState
{
    public StateMachine parentMachine;

    public List<StateMachineTransition> transitions = new List<StateMachineTransition>();

    public void AddTransition(StateMachineTransition transition)
    {
        transitions.Add(transition);
    }

    public virtual void Update()
    {

    }
    public virtual void OnStateEnter()
    {

    }

    public virtual void OnStateExit()
    {

    }

    public abstract void SetupTransitions();
}

public class StateMachineTransition
{
    private Type targetState;
    public Type TargetState => targetState;

    private Func<bool> condition;

    public StateMachineTransition(Type target, Func<bool> condition)
    {
        this.targetState = target;
        this.condition = condition;
    }

    public bool Evaluate()
    {
        return condition();
    }
}