using UnityEngine;

public class RiseState : EnviromentInteractionState
{
    float _elapsedTime = 0.0f;
    float _lerpDuration = 5.0f;
    float _riseWeight = 1.0f;
    Quaternion _expectedRotation;
    float _maxDistance = .5f;
    protected LayerMask _interactableLayerMask = LayerMask.GetMask("Interactable");
    float _rotationSpeed = 1000f;
    float _touchDistanceTreshold = .05f;
    float _touchTimeTreashold = 2.0f;

    public RiseState(EnviromentInteractionContext context, EnviromentInteractionStateMachine.EEnviromentInteractionState estate) : base(context, estate)
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
        CalculateExpectedHandRotation();

        Context.InteractionPointYOffset = Mathf.Lerp(Context.InteractionPointYOffset,
            Context.ClosestPointOnColliderFromShoulder.y, _elapsedTime / _lerpDuration);
        
        Context.CurrentIkConstraint.weight = Mathf.Lerp(Context.CurrentIkConstraint.weight, _riseWeight,
            _elapsedTime / _lerpDuration);
        Context.CurrentMultiRotationConstraint.weight = Mathf.Lerp(Context.CurrentMultiRotationConstraint.weight,
            _riseWeight, _elapsedTime/_lerpDuration);

        Context.CurrentIkTargetTransform.rotation = Quaternion.RotateTowards(Context.CurrentIkTargetTransform.rotation,
            _expectedRotation, _rotationSpeed * Time.deltaTime);

        _elapsedTime += Time.deltaTime;
    }
    private void CalculateExpectedHandRotation() { 
        //raycast for normal of the surface
        Vector3 startPos = Context.CurrentShoulderTransform.position;
        Vector3 endPos = Context.ClosestPointOnColliderFromShoulder;
        Vector3 direction = (endPos - startPos).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, _maxDistance, _interactableLayerMask)) {
            Vector3 surfaceNormal = hit.normal;
            Vector3 targetForward = -surfaceNormal;
            _expectedRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        }
    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        if (CheckShouldReset()) { 
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Reset;
        }
        if (Vector3.Distance(Context.CurrentIkTargetTransform.position,Context.ClosestPointOnColliderFromShoulder)
            < _touchDistanceTreshold && _elapsedTime >= _touchTimeTreashold) { 
        
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Touch;
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
