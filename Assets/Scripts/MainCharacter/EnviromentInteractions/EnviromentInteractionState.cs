using UnityEngine;

public abstract class EnviromentInteractionState : BaseState<EnviromentInteractionStateMachine.EEnviromentInteractionState>
{
    protected EnviromentInteractionContext Context;
    private float _movingAwayOffset = .5f;
    bool _shouldReset;
    public EnviromentInteractionState(EnviromentInteractionContext context, EnviromentInteractionStateMachine.EEnviromentInteractionState
        steteKey) : base(steteKey)
    {
        Context = context;
    }

    private Vector3 GetClosestPointOnCollider(Collider intersectingCollider, Vector3 positionToCheck) {
        return intersectingCollider.ClosestPoint(positionToCheck);
    }

    protected void StartIkTargetPositionTracking(Collider intersectingCollider) { // pass to on trigger methoods

        if (intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") && Context.CurrentIntersectingCollider == null) {
            Context.CurrentIntersectingCollider = intersectingCollider;
            Vector3 closestPointFromRoot = GetClosestPointOnCollider(intersectingCollider, Context.RootTransform.position);
            Context.SetCurrentSide(closestPointFromRoot);

            SetIkTargetPosition();
        }

    }
    protected void UpdateIkTargetPosition(Collider intersectingCollider) {

        if (intersectingCollider == Context.CurrentIntersectingCollider) {
            SetIkTargetPosition();
        }

    }
    protected void ResetIkTargetPositionTracking(Collider intersectingCollider) {
        if (intersectingCollider == Context.CurrentIntersectingCollider) {
            Context.CurrentIntersectingCollider = null;
            Context.ClosestPointOnColliderFromShoulder = Vector3.positiveInfinity;
            _shouldReset = true;
        }
    }

    private void SetIkTargetPosition() {
        Context.ClosestPointOnColliderFromShoulder = GetClosestPointOnCollider(Context.CurrentIntersectingCollider,
            new Vector3(Context.CurrentShoulderTransform.position.x, Context.CharacterShoulderHeight, Context.CurrentShoulderTransform.position.z));

        Vector3 rayDirection = Context.CurrentShoulderTransform.position - Context.ClosestPointOnColliderFromShoulder;
        Vector3 normalizedRayDirection = rayDirection.normalized;
        float offsetDistace = .05f;
        Vector3 offset = normalizedRayDirection * offsetDistace;

        Vector3 offsetPosition = Context.ClosestPointOnColliderFromShoulder + offset;
        Context.CurrentIkTargetTransform.position = new Vector3(offsetPosition.x,
            Context.InteractionPointYOffset, offsetPosition.z);
    }

    protected bool CheckShouldReset() {
        // reset if
        //Debug.Log("here2");

        //_shouldReset colliders no longer intersecting
        if (_shouldReset) {
            Context.LowestDistance = Mathf.Infinity;
            _shouldReset = false;
            //Debug.Log("here1");
            return true;
        }
        //bool isPlayerSopped = Context.CharacterController.velocity.magnitude < 0.1f; // stops moving
        //Debug.Log(Context.CharacterController.velocity.magnitude);
        bool isMovingAway = ChechIsMovingAvay();  // moving away
        bool isBadAngle = CheckIsBadAngle(); // rotating away
        bool isPlayerJumping = !Context.CharacterController.isGrounded; // jumping

        if ( isMovingAway || isBadAngle ) { 
            Context.LowestDistance = Mathf.Infinity;
            //Debug.Log("reset");
            return true;
        }

        return false;
    }

    protected bool ChechIsMovingAvay() {
        float currentDistanceFromTarget = Vector3.Distance(Context.RootTransform.position,
            Context.ClosestPointOnColliderFromShoulder);
        //compare lowest distance to current distance
        bool isSearchingForNewIntersection = Context.CurrentIntersectingCollider == null;
        if (isSearchingForNewIntersection) { 
            return false;
        }
        bool isGettingCloser = currentDistanceFromTarget <= Context.LowestDistance;
        if (isGettingCloser) { 
            Context.LowestDistance = currentDistanceFromTarget;
            return false;
        }

        bool isMovingAway = currentDistanceFromTarget > Context.LowestDistance + _movingAwayOffset;
        if (isMovingAway)
        {
            Context.LowestDistance = Mathf.Infinity;
            return true;
        }

        return false;
    }
    protected bool CheckIsBadAngle() {
        // if player rotates past certain angle
        if (Context.CurrentIntersectingCollider == null) {
            return false;
        }

        Vector3 targetDirection = Context.ClosestPointOnColliderFromShoulder - Context.CurrentShoulderTransform.position;
        Vector3 shoulderDirection = Context.CurrentBodySide == EnviromentInteractionContext.EBodySide.RIGHT ?
            Context.RootTransform.right : -Context.RootTransform.right;

        float dotProduct = Vector3.Dot(shoulderDirection, targetDirection.normalized);

        bool isBadAngle = dotProduct < 0; // grater than 90'
        return isBadAngle;
    }
}
