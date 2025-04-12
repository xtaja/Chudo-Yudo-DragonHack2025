using UnityEngine;

public class TouchState : EnviromentInteractionState
{
    public float _elapsedTime = 0.0f;
    float _resetTreshold = 3f;

    public TouchState(EnviromentInteractionContext context, EnviromentInteractionStateMachine.EEnviromentInteractionState estate) : base(context, estate)
    {
        EnviromentInteractionContext Context = context;
    }

    public override void EnterState()
    {
        _elapsedTime = 0.0f;
    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        if (_elapsedTime > _resetTreshold || CheckShouldReset()) {
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Reset;
        }
        return StateKey;
    }
    public override void OnTriggerEnter(Collider other)
    {
        StartIkTargetPositionTracking(other);
    }
    public override void OnTriggerStay(Collider other)
    {
        UpdateIkTargetPosition(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        ResetIkTargetPositionTracking(other);
    }
}
