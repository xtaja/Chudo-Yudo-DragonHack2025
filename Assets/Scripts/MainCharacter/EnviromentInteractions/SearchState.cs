using UnityEngine;

public class SearchState : EnviromentInteractionState
{
    public float _approachDistanceTreshold = 2.0f;

    public SearchState(EnviromentInteractionContext context, EnviromentInteractionStateMachine.EEnviromentInteractionState estate) : base(context, estate)
    {
        EnviromentInteractionContext Context = context;
    }


    public override void EnterState()
    {
        Debug.Log("Entering SEARCH state");

    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        if (CheckShouldReset())
        {
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Reset;
        }

        bool isCloseToTarget = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder, Context.RootTransform.position) < _approachDistanceTreshold ;
        
        bool isClosestPointOnColliderValid = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

        if (isClosestPointOnColliderValid && isCloseToTarget) {
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Approach;
        }

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) {
        StartIkTargetPositionTracking(other);
    }
    public override void OnTriggerStay(Collider other) { 
        UpdateIkTargetPosition(other);
    }
    public override void OnTriggerExit(Collider other) {
        ResetIkTargetPositionTracking(other);
    }
}
