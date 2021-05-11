using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraController : MonoBehaviour, iEntityController
{
    [SerializeField] private NewCameraData c_cameraData;
    [SerializeField] private CameraPreviewData c_previewData;
    [SerializeField] private DebugAccessor debugAccessor;

    private StateData c_stateData;
    private NewCameraLastFrameData c_lastFrameData;
    private NewCameraPositionData c_positionData;
    private NewCameraTargetData c_targetData;
    private CameraPreviewActiveData c_previewActiveData;

    iMessageClient cl_camera;
    StateMachine sm_cameraBehavior;

    // rendering constants
    private static int renderPixelHeight;
    private static float renderPixelRatio;
    private static int renderPixelWidth;

    // Start is called before the first frame update
    void Start()
    {
        SetDefaultData();
        InitializeStateMachine();

        c_stateData = new StateData();
        c_stateData.b_updateState = true;

        cl_camera = new CameraMessageClient(ref c_stateData, ref c_targetData);
        MessageServer.Subscribe(ref cl_camera, MessageID.PAUSE);
        MessageServer.Subscribe(ref cl_camera, MessageID.COUNTDOWN_START);
        MessageServer.Subscribe(ref cl_camera, MessageID.PLAYER_POSITION_UPDATED);
    }

    // Update is called once per frame
    void Update()
    {
        if (!c_stateData.b_updateState)
        {
            return;
        }
        EnginePull();

        EngineUpdate();
    }

    void FixedUpdate()
    {

        if (!c_stateData.b_updateState)
        {
            return;
        }
        FixedEnginePull();

        sm_cameraBehavior.Act();

        debugAccessor.DisplayFloat("Vert Velocity", c_positionData.f_currentTranslationVelocity);
        c_lastFrameData.v_lastTargetPosition = c_targetData.v_currentTargetPosition;
        c_lastFrameData.q_lastTargetRotation = c_targetData.q_currentTargetRotation;
    }

    public void EnginePull()
    {

    }

    public void FixedEnginePull()
    {
        c_lastFrameData.v_lastPosition = c_positionData.v_currentPosition;
        c_lastFrameData.q_lastRotation = c_positionData.q_currentRotation;

        DoCollisionChecks();
    }

    public void EngineUpdate()
    {
        transform.position = Utils.InterpolateFixedVector(c_lastFrameData.v_lastPosition, c_positionData.v_currentPosition);
        transform.rotation = Utils.InterpolateFixedQuaternion(c_lastFrameData.q_lastRotation, c_positionData.q_currentRotation);
    }

    public void UpdateStateMachine()
    {

    }

    public void DoCollisionChecks()
    {

    }

    void InitializeStateMachine()
    {
        CameraFollowState s_followTarget = new CameraFollowState(ref c_cameraData, ref c_positionData, ref c_targetData, ref c_lastFrameData);

        sm_cameraBehavior = new StateMachine(s_followTarget, StateRef.TRACKING);
    }

    void SetDefaultData()
    {
        renderPixelHeight = 1080;
        renderPixelRatio = ((float)Camera.main.pixelHeight / (float)Camera.main.pixelWidth);
        renderPixelWidth = Mathf.RoundToInt(renderPixelRatio * renderPixelHeight);

        c_positionData = new NewCameraPositionData(transform.position, transform.rotation, 0);
        c_targetData = new NewCameraTargetData(Vector3.zero, Quaternion.identity);

        c_lastFrameData = new NewCameraLastFrameData(c_positionData.v_currentPosition,
                                                  c_targetData.v_currentTargetPosition,
                                                  c_positionData.q_currentRotation,
                                                  c_targetData.q_currentTargetRotation);
    }

    #region RenderingFuncs
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Camera.main.orthographicSize = renderPixelHeight;
        source.filterMode = FilterMode.Point;
        RenderTexture buffer = RenderTexture.GetTemporary(renderPixelWidth, renderPixelHeight, -1);
        buffer.filterMode = FilterMode.Point;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
    #endregion
}
