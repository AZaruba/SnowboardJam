using UnityEngine;

public class CameraController3 : MonoBehaviour
{
    private StateData c_stateData;
    private EntityPositionData c_positionData;
    private CameraTrackingData c_trackingData;
    private iMessageClient c_cameraClient;

    // Start after awake so targets are initialized
    private void Start()
    {
        InitializeData();
        InitializeMessangingClient();
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
        if (!c_stateData.b_updateState)
        {
            return;
        }

        c_positionData.v_position_lastFrame = c_positionData.v_position;
        c_positionData.q_rotation_lastFrame = c_positionData.q_rotation;

        c_positionData.v_position = c_trackingData.v_position - c_trackingData.v_position_lastFrame;
    }

    private void InitializeData()
    {
        c_stateData = new StateData();
        c_stateData.b_updateState = true; // prestarted state

        c_positionData = new EntityPositionData(transform.position, transform.rotation);
        c_trackingData = new CameraTrackingData(); // position will be updated on first frame

    }

    private void InitializeMessangingClient()
    {
        c_cameraClient = new CameraMessageClient(ref c_stateData, ref c_trackingData);
        MessageServer.Subscribe(ref c_cameraClient, MessageID.PAUSE);
        MessageServer.Subscribe(ref c_cameraClient, MessageID.COUNTDOWN_START);
        MessageServer.Subscribe(ref c_cameraClient, MessageID.PLAYER_POSITION_UPDATED);
    }
}
