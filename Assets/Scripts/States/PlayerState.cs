using UnityEngine;
using UnityHFSM;


public enum PlayerStateType
{
    Idle,
    Running,
    Jumping,
    Falling,
    CarryingObject,
    RotatingCarryObject,
    InMenu
}

public class PlayerStateMachine : StateMachine<PlayerStateType>
{
    public PlayerStateMachine()
    {
        SetStartState(PlayerStateType.Idle);
    }
}

public class PlayerTransition : Transition<PlayerStateType>
{
    public PlayerTransition(PlayerStateType from, PlayerStateType to) : base(from, to)
    {
    }
}

public class PlayerState : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    public PlayerStateType currentState;

    public static PlayerState instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize the player state machine
        stateMachine = new PlayerStateMachine();
        currentState = PlayerStateType.Idle;
        stateMachine.AddState(PlayerStateType.InMenu, onEnter: State => UpdateState(PlayerStateType.InMenu));
        stateMachine.AddState(PlayerStateType.Idle, onEnter: state => UpdateState(PlayerStateType.Idle));
        stateMachine.AddState(PlayerStateType.CarryingObject, onEnter: state => UpdateState(PlayerStateType.CarryingObject));
        stateMachine.AddState(PlayerStateType.RotatingCarryObject, onEnter: state => UpdateState(PlayerStateType.RotatingCarryObject));

        stateMachine.AddTriggerTransitionFromAny("OnInMenu", new PlayerTransition(PlayerStateType.InMenu, PlayerStateType.InMenu));
        stateMachine.AddTriggerTransitionFromAny("OnIdle", new PlayerTransition(PlayerStateType.Idle, PlayerStateType.Idle));
        stateMachine.AddTriggerTransitionFromAny("OnCarryingObject", new PlayerTransition(PlayerStateType.CarryingObject, PlayerStateType.CarryingObject));
        stateMachine.AddTriggerTransition("OnRotatingCarryObject", new PlayerTransition(PlayerStateType.CarryingObject, PlayerStateType.RotatingCarryObject));

        stateMachine.Init();
    }

    public void Start()
    {
        
    }

    private void UpdateState(PlayerStateType state)
    {
        currentState = state;

        switch (state)
        {
            case PlayerStateType.Idle:
                // Handle idle state logic
                break;
            case PlayerStateType.CarryingObject:
                // Handle carrying object logic
                break;
            case PlayerStateType.RotatingCarryObject:
                // Handle rotating carry object logic
                break;
            case PlayerStateType.InMenu:
                //Handle Menu Changing Logic
                break;
            default:
                Debug.LogWarning("Unhandled player state: " + state);
                break;
        }
    }

    public void TriggerTransition(PlayerStateType newState)
    {
        // Translate newState to a string with On in the beginning
        string triggerName = "On" + newState.ToString();
        stateMachine.Trigger(triggerName);
    }

}
