using UnityEngine;

public class ResetState : EnviromentInteractionState
{

    float _elapsedTime = 0.0f;
    float _resetDuration = 1.0f;
    float _lerpDuration = 10.0f;
    float _rotationSpeed = 500.0f;
    private Vector3 _previousPosition;
    private Vector3 _velocity;
    public ResetState(EnviromentInteractionContext context, EnviromentInteractionStateMachine.EEnviromentInteractionState estate) : base(context, estate)
    {
        EnviromentInteractionContext Context = context;
    }


    public override void EnterState() {
        _elapsedTime = 0.0f;
        Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
        Context.CurrentIntersectingCollider = null;
    }
    public override void ExitState() { 
    
    }
    public override void UpdateState() {
        _elapsedTime += Time.deltaTime;

        _velocity = (Context.RootTransform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = Context.RootTransform.position;

        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset, Context.ColliderCenterY, _elapsedTime / _lerpDuration);

        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, 0, _elapsedTime / _lerpDuration);
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight, 0, _elapsedTime / _lerpDuration);

        Context.CurrentIkTargetTransform.localPosition = Vector3.Lerp(Context.CurrentIkTargetTransform.localPosition,
            Context.CurrentOriginalTargetPosition, _elapsedTime / _lerpDuration);
        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation,
            Context.OriginalTargetRotation, -_rotationSpeed * Time.deltaTime);
    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState() {
        bool isMoving = _velocity.magnitude > 0.01f;
        //bool isMoving = Context.CharacterController.velocity != Vector3.zero;
        if (_elapsedTime >= _resetDuration && isMoving) { 
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Search;
        }

        return StateKey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }

}
