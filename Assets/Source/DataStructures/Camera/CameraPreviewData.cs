using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraPreviewData", menuName = "Camera Preview Data")]
public class CameraPreviewData : ScriptableObject
{
    #region PUBLIC_VARIABLES
    public List<PreviewShot> PreviewShots;
    #endregion
}

[System.Serializable]
public struct PreviewShot
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public Vector3 CameraAngle;
    public float Time;
}
