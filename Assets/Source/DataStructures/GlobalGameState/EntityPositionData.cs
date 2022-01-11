using UnityEngine;

/// <summary>
/// Generic positioning data for entities. Initially used by the camera,
/// this contains the very basic data for maintaining spatial positions
/// using the FixedUpdate Interpolation System of this game
/// </summary>
public class EntityPositionData
{
    public EntityPositionData(Vector3 position, Quaternion rotation)
    {
        this.Position = position;
        this.Rotation = rotation;

        this.Position_LastFrame = position;
        this.Rotation_LastFrame = rotation;
    }

    private Vector3 Position;
    private Vector3 Position_LastFrame;

    private Quaternion Rotation;
    private Quaternion Rotation_LastFrame;

    public Vector3 v_position
    {
        get { return Position; }
        set { Position = value; }
    }

    public Vector3 v_position_lastFrame
    {
        get { return Position_LastFrame; }
        set { Position_LastFrame = value; }
    }

    public Quaternion q_rotation
    {
        get { return Rotation; }
        set { Rotation = value; }
    }

    public Quaternion q_rotation_lastFrame
    {
        get { return Rotation_LastFrame; }
        set { Rotation_LastFrame = value; }
    }
}
