using Unity.Mathematics;
using UnityEngine;

public class ApproachState : EnviromentInteractionState
{
    float _elapsedTime = 0.0f;
    float _approachWeight = .5f;
    float _approachRotationWeight = .75f;
    float _lerpDuration = 5.0f;
    float _rotationSpeed = 500f;
    float _riseDistanceTreshold = .5f;
    float _approachDuration = 2.0f;
    public ApproachState(EnviromentInteractionContext context, EnviromentInteractionStateMachine.EEnviromentInteractionState estate) : base(context, estate)
    {
        EnviromentInteractionContext Context = context;
    }


    public override void EnterState()
    {
        //Debug.Log("Entering APPROACH state");
        _elapsedTime = 0.0f;

    }
    public override void ExitState()
    {

    }
    public override void UpdateState()
    {
        Quaternion expectedGroudLocation = Quaternion.LookRotation(-Vector3.up, Context.RootTransform.forward);

        _elapsedTime += Time.deltaTime;

        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation,
            expectedGroudLocation, _rotationSpeed * Time.deltaTime);

        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.LeftIkConstraint.weight, _approachWeight, _elapsedTime / _lerpDuration);

        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight,
            _approachRotationWeight, _elapsedTime / _lerpDuration);


    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        bool isOverStateLifeDuration = _elapsedTime >= _approachDuration;
        if (isOverStateLifeDuration || CheckShouldReset()) {
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Reset;
        }

        bool isWithinArmsReach = Vector3.Distance(Context.ClosestPointOnColliderFromShoulder,
            Context.CurrentShoulderTransform.position) < _riseDistanceTreshold;
        bool isClosestPointOnColliderReal = Context.ClosestPointOnColliderFromShoulder != Vector3.positiveInfinity;

        if (isClosestPointOnColliderReal && isWithinArmsReach) { 
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Rise;
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
