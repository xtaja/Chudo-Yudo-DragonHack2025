using UnityEngine;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public abstract class StateManager<EState>: MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected bool IsTransitioningState = false;
    public BaseState<EState> CurrentState { get; protected set; }

    void Start() { 
        CurrentState.EnterState();
    }
    void Update() { 
        EState nextStateKey = CurrentState.GetNextState();
        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey)) {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState) {
            TransitionToState(nextStateKey);
        }
        
    }
    public void TransitionToState(EState stateKey) { 
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }
    void OnTriggerEnter(Collider other) {
        //Debug.Log("StateManager Trigger Enter: " + other.name);
        CurrentState.OnTriggerEnter(other);
    }
    void OnTriggerStay(Collider other) {
        CurrentState.OnTriggerStay(other);
    }
    void OnTriggerExit(Collider other) {
        CurrentState.OnTriggerExit(other);
    }
}
