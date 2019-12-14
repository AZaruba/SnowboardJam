using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, iEntityController {

    #region Members
    [SerializeField] private CameraData c_cameraData;

    private CameraStateMachine c_stateMachine;

    //cartridge list
    private FocusCartridge cart_focus;
    #endregion

    /// <summary>
    /// Start this instance. Initializes all valid states for this object
    /// then adds them to the state machine
    /// </summary>
	void Start ()
    {
        SetDefaultCameraData();
        InitializeCartridges();
        InitializeStateMachine();
	}
	
    /// <summary>
    /// Update this instance. States perform actions on data, the data is then
    /// used for object-level functions (such as translations) and then the
    /// state is updated.
    /// </summary>
	void Update ()
    {
        EnginePull();

        UpdateStateMachine();

        c_stateMachine.Act(ref c_cameraData);

        EngineUpdate();
	}

    public void EngineUpdate()
    {
        transform.position = c_cameraData.v_currentPosition;
        transform.forward = c_cameraData.v_currentDirection;
    }

    public void EnginePull()
    {
        c_cameraData.v_targetPosition = c_cameraData.t_targetTransform.position;
    }

    public void UpdateStateMachine()
    {
        Vector3 cameraPosition = c_cameraData.v_currentPosition;
        Vector3 targetPosition = c_cameraData.v_targetPosition;
        float followDistance = c_cameraData.f_followDistance;

        float trueDistance = Vector3.Distance(cameraPosition, targetPosition);

        if (trueDistance > followDistance)
        {
            // camera is too far, start moving closer
        } 
        else if (trueDistance < followDistance)
        {
            // camera is too close, start moving further away
        } 
        else
        {
            // we have achieved balance!
        }
    }

    #region StartupFunctions

    /// <summary>
    /// Initializes the state machine.
    /// </summary>
    void InitializeStateMachine()
    {
        StationaryLookAtState s_stationary = new StationaryLookAtState (ref cart_focus);

        c_stateMachine = new CameraStateMachine (s_stationary, StateRef.STATIONARY);
    }

    /// <summary>
    /// Initializes the cartridges.
    /// </summary>
    void InitializeCartridges()
    {
        cart_focus = new FocusCartridge ();
    }

    void SetDefaultCameraData()
    {
        Vector3 targetPosition = c_cameraData.t_targetTransform.position;
        Vector3 targetDirection = c_cameraData.t_targetTransform.forward;

        c_cameraData.v_currentPosition = transform.position;
        c_cameraData.v_currentDirection = transform.forward;
        c_cameraData.v_targetPosition = targetPosition;
        c_cameraData.v_targetDirection = targetDirection;
    }
    #endregion
}
