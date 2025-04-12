using UnityEngine;
using UnityEditor.Animations.Rigging;
using UnityEngine.Animations.Rigging;
using NUnit.Framework;

public class EnviromentInteractionStateMachine : StateManager<EnviromentInteractionStateMachine.EEnviromentInteractionState>
{
    public enum EEnviromentInteractionState { 
        Search,
        Approach,
        Rise,
        Touch,
        Reset,
    }

    private EnviromentInteractionContext _context;

    [SerializeField] private TwoBoneIKConstraint _leftIkConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIkConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiRotationConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiRotationConstraint;
    //[SerializeField] private Rigidbody _rigidbody;
    //[SerializeField] private CapsuleCollider _rootCollider;
    [SerializeField] private CharacterController _characterController;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_context != null && _context.ClosestPointOnColliderFromShoulder != null) {
            Gizmos.DrawSphere(_context.ClosestPointOnColliderFromShoulder, .03f);
        }
    }

    void Awake()
    {
        ValidateConstraints();

        _context = new EnviromentInteractionContext(_leftIkConstraint, _rightIkConstraint, _leftMultiRotationConstraint,
            _rightMultiRotationConstraint, _characterController, transform.root);

        InitializeStates();
        ConstructEnviromentDirectionCollider();
    }

    private void ValidateConstraints() {
        Assert.IsNotNull(_leftIkConstraint, "Left IK constraint is not assigned");
        Assert.IsNotNull(_rightIkConstraint, "Right IK constraint is not assigned");
        Assert.IsNotNull(_leftMultiRotationConstraint, "Left MultiRotationConstraint is not assigned");
        Assert.IsNotNull(_rightMultiRotationConstraint, "Right MultiRotationConstraint is not assigned");
        Assert.IsNotNull(_characterController, "CharacterController is not assigned");
    }

    private void InitializeStates() {
        // add states to inherit state manager "states" dictionary and set initial state
        States.Add(EEnviromentInteractionState.Reset, new ResetState(_context, EEnviromentInteractionState.Reset));
        States.Add(EEnviromentInteractionState.Search, new SearchState(_context, EEnviromentInteractionState.Search));
        States.Add(EEnviromentInteractionState.Approach, new ApproachState(_context, EEnviromentInteractionState.Approach));
        States.Add(EEnviromentInteractionState.Rise, new RiseState(_context, EEnviromentInteractionState.Rise));
        States.Add(EEnviromentInteractionState.Touch, new TouchState(_context, EEnviromentInteractionState.Touch));

        CurrentState = States[EEnviromentInteractionState.Reset];
    }

    private void ConstructEnviromentDirectionCollider() {
        float wingspan = _characterController.height;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(wingspan, wingspan, wingspan);
        boxCollider.center = new Vector3(_characterController.center.x,
            _characterController.center.y + (.25f * wingspan), _characterController.center.z + (.5f * wingspan));
        boxCollider.isTrigger = true;

        _context.ColliderCenterY = _characterController.center.y + 0.25f;
    
    }

}
