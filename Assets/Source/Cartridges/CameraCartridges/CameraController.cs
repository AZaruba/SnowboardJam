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
	void FixedUpdate ()
    {
        EnginePull();

        c_stateMachine.Act(ref c_cameraData);

        EngineUpdate();

        UpdateStateMachine();
	}

    public void EngineUpdate()
    {
        transform.position = c_cameraData.GetCurrentPosition();
        transform.forward = c_cameraData.GetCurrentDirection();
    }

    public void EnginePull()
    {
        c_cameraData.SetTargetPosition(c_cameraData.GetTarget().transform.position);
    }

    public void UpdateStateMachine()
    {
        Vector3 cameraPosition = c_cameraData.GetCurrentPosition();
        Vector3 targetPosition = c_cameraData.GetTargetPosition();
        float followDistance = c_cameraData.GetFollowDistance();

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
        Vector3 targetPosition = c_cameraData.GetTarget().transform.position;

        c_cameraData.SetCurrentPosition(transform.position);
        c_cameraData.SetTargetPosition(targetPosition);
        c_cameraData.SetCurrentDirection(transform.forward);
    }
    #endregion
}
