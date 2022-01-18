using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputData
{
    #region PlayerOneControls
    [SerializeField] public KeyCode JumpButton;
    [SerializeField] public KeyCode JumpKey;

    [SerializeField] public KeyCode CrouchButton;
    [SerializeField] public KeyCode CrouchKey;

    [SerializeField] public KeyCode DTrickButton;
    [SerializeField] public KeyCode DTrickKey;

    [SerializeField] public KeyCode LTrickButton;
    [SerializeField] public KeyCode LTrickKey;

    [SerializeField] public KeyCode RTrickButton;
    [SerializeField] public KeyCode RTrickKey;

    [SerializeField] public KeyCode UTrickButton;
    [SerializeField] public KeyCode UTrickKey;

    [SerializeField] public KeyCode PauseButton;
    [SerializeField] public KeyCode PauseKey;

    [SerializeField] public KeyCode ConfirmButton;
    [SerializeField] public KeyCode BackButton;

    [SerializeField] public string LeftHorizontalAxis;
    [SerializeField] public string LeftVerticalAxis;
    #endregion
}
