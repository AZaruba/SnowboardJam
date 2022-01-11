using UnityEngine;

public class CameraController3 : MonoBehaviour
{
    [SerializeField] private CameraMotionData CameraMotionData;
    [SerializeField] private DebugAccessor debugAccessor;

    private StateData c_stateData;
    private EntityPositionData c_positionData;
    private CameraTrackingData c_trackingData;

    private StateMachine sm_camera;
    private iMessageClient c_cameraClient;

    // Start after awake so targets are initialized
    private void Start()
    {
        InitializeData();
        InitializeMessangingClient();
        InitializeStateMachine();
    }

    // TODO:
    /* - Get camera states together, have them accurately reflect updates instead of slapping them in fixedUpdate
     *     - weight
     *     - acceleration
     *     - multiple states of motion ?
     *     
     * - figure out how to get the player's transform position when this thing wakes first    
     * 
     */ 
    private void Update()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }

        // transform position updates
        transform.position = Utils.InterpolateFixedVector(c_positionData.v_position_lastFrame, c_positionData.v_position);
        transform.rotation = Utils.InterpolateFixedQuaternion(c_positionData.q_rotation_lastFrame, c_positionData.q_rotation);

    }

    private void FixedUpdate()
    {
        debugAccessor.DisplayState("Camera State", sm_camera.GetCurrentState());

        if (!c_stateData.b_updateState)
        {
            return;
        }

        c_positionData.v_position_lastFrame = c_positionData.v_position;
        c_positionData.q_rotation_lastFrame = c_positionData.q_rotation;
    }

    private void InitializeData()
    {
        c_stateData = new StateData();
        c_stateData.b_updateState = true; // prestarted state

        c_positionData = new EntityPositionData(transform.position, transform.rotation);
        c_trackingData = new CameraTrackingData(); // position will be updated on first frame
    }

    private void InitializeStateMachine()
    {
        CameraSnapState s_snap = new CameraSnapState(ref c_trackingData, ref c_positionData);
        CameraFollowState2 s_follow = new CameraFollowState2(ref c_trackingData, ref CameraMotionData, ref c_positionData);

        sm_camera = new StateMachine(s_snap, StateRef.SNAPPING);
        sm_camera.AddState(s_follow, StateRef.FOLLOWING);
    }

    private void InitializeMessangingClient()
    {
        c_cameraClient = new CameraMessageClient(ref c_stateData, ref c_trackingData);
        MessageServer.Subscribe(ref c_cameraClient, MessageID.PAUSE);
        MessageServer.Subscribe(ref c_cameraClient, MessageID.COUNTDOWN_START);
        MessageServer.Subscribe(ref c_cameraClient, MessageID.PLAYER_POSITION_UPDATED);
    }
}
