using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController
{

    private CollisionData c_collisionData;
    private CharacterCollisionData c_collisionAttrs;

    private PlayerData c_playerData;
    private PlayerPositionData c_positionData;
    private AerialMoveData c_aerialMoveData;

    private BoxCollider c_playerCollider;

    private LayerMask i_groundCollisionMask;
    private LayerMask i_zoneCollisionMask;

    private RaycastHit h_groundCheck;
    private Collider[] a_colliders;

    public CollisionController(ref PlayerData playerData,
        ref PlayerPositionData positionData,
        ref CollisionData collisionData,
        ref CharacterCollisionData collisionAttrs,
        ref AerialMoveData aerialMoveData,
        ref BoxCollider playerCollider,
        LayerMask groundMask,
        LayerMask zoneMask)
    {
        this.c_collisionData = collisionData;
        this.c_collisionAttrs = collisionAttrs;
        this.c_playerData = playerData;
        this.c_positionData = positionData;
        this.c_aerialMoveData = aerialMoveData;
        this.i_groundCollisionMask = groundMask;
        this.i_zoneCollisionMask = zoneMask;
        this.c_playerCollider = playerCollider;
        this.a_colliders = new Collider[5];
    }

    /// <summary>
    /// Casts a ray downward directly from the player to verify the rotation the
    /// model should have
    /// </summary>
    /// <returns>Whether the raycast finds a normal</returns>
    public bool CheckGroundRotation()
    {
        // draw a box here, check to see what this guy looks like
        DrawCubePoints(CubePoints(c_playerData.v_currentPosition + c_playerData.q_currentRotation * c_collisionAttrs.CenterOffset, new Vector3(0.25f, 0.25f, 0.25f), c_playerData.q_currentRotation), Color.cyan);

        // TODO: assess distance and under what circumstances we would realign
        if (Physics.Raycast(c_playerData.v_currentPosition + c_playerData.q_currentRotation * c_collisionAttrs.CenterOffset,
                                           Vector3.down,
                                           out h_groundCheck,
                                           (c_playerData.f_gravity + (c_aerialMoveData.f_verticalVelocity * -1)) * Time.deltaTime + c_collisionAttrs.CenterOffset.y * 2,
                                           i_groundCollisionMask))
        {
            DrawCubePoints(CubePoints(c_playerData.v_currentPosition + c_playerData.q_currentRotation * c_collisionAttrs.CenterOffset
                + c_playerData.q_currentRotation * Vector3.down *
                ((c_playerData.f_gravity + (c_aerialMoveData.f_verticalVelocity * -1)) * Time.deltaTime + c_collisionAttrs.CenterOffset.y * 2),
                new Vector3(0.25f, 0.25f, 0.25f), c_playerData.q_currentRotation), Color.magenta);

            c_collisionData.v_surfaceNormal = Utils.GetBaryCentricNormal(h_groundCheck);

            return true;
        }
        DrawCubePoints(CubePoints(c_playerData.v_currentPosition + c_playerData.q_currentRotation * c_collisionAttrs.CenterOffset
            + c_playerData.q_currentRotation * Vector3.down *
            ((c_playerData.f_gravity + (c_aerialMoveData.f_verticalVelocity * -1)) * Time.deltaTime + c_collisionAttrs.CenterOffset.y * 2),
            new Vector3(0.25f, 0.25f, 0.25f), c_playerData.q_currentRotation), Color.cyan);
        return false;
    }

    public bool CheckForGround4()
    {
        int collisionsDetected = Physics.OverlapBoxNonAlloc(c_playerCollider.transform.position, 
            c_playerCollider.size / 2, a_colliders, 
            c_playerCollider.transform.rotation,
            i_groundCollisionMask);
        if (collisionsDetected > 0)
        {
            if (Physics.ComputePenetration(c_playerCollider,
                c_playerCollider.transform.position,
                c_playerCollider.transform.rotation,
                a_colliders[0],
                a_colliders[0].transform.position,
                a_colliders[0].transform.rotation,
                out Vector3 adjustmentDirection,
                out float adjustmentDistance))
            {
                // add a little less to keep the adjustment from pushing all the way out
                c_collisionData.v_attachPoint = adjustmentDirection * adjustmentDistance;
            }
            else
            {
                c_collisionData.v_attachPoint = Vector3.zero;
            }
            return true;
        }

        return false;
    }

    // TODO: strange behavior on land with a bounce, attach point is decidedly wrong but how to rectify it?
    public bool CheckForAir()
    {
        c_collisionData.v_attachPoint = Vector3.zero;
        CalculateRaycastDistance(out float upwardDist, out float downwardDist);

        Vector3 origin = c_playerData.v_currentPosition + Vector3.up * (upwardDist + 0.1f) + c_playerData.q_currentRotation * c_collisionAttrs.CenterOffset;
        bool boxCasted = Physics.BoxCast(origin,
                               c_collisionAttrs.HalfExtents,
                               Vector3.down,
                               out h_groundCheck,
                               c_positionData.q_currentModelRotation,
                               downwardDist + 0.1f,
                               CollisionLayers.ENVIRONMENT);

        DrawBoxcast(upwardDist, downwardDist, boxCasted);
        if (boxCasted)
        {
            Vector3 projected = Vector3.ProjectOnPlane(c_playerData.v_currentPosition - h_groundCheck.point, c_playerData.q_currentRotation * Vector3.forward);
            projected = Vector3.ProjectOnPlane(projected, c_playerData.q_currentRotation * Vector3.right);
            c_collisionData.v_attachPoint = projected * -1;
        }
        return boxCasted;
    }

    /// <summary>
    /// Calculates the distance to check downward for triggering the "fall"
    /// state based on the current velocity
    /// </summary>
    /// <returns>The total distance of the raycast check</returns>
    private void CalculateRaycastDistance(out float upwardDistance, out float downwardDistance)
    {
        // upward dist raises the scan start point, so any upward vertical velocity should increase it
        upwardDistance = c_aerialMoveData.f_verticalVelocity > Constants.ZERO_F ? c_aerialMoveData.f_verticalVelocity * Constants.NEGATIVE_ONE * Time.deltaTime : Constants.ZERO_F;

        // downward scan lowers the bottom, so it should be increased with downward velocity and decreased by speed
        downwardDistance = c_aerialMoveData.f_verticalVelocity > Constants.ZERO_F ? Constants.ZERO_F : c_aerialMoveData.f_verticalVelocity * Constants.NEGATIVE_ONE * Time.deltaTime;
        downwardDistance += c_playerData.f_gravity * Time.deltaTime;
    }

    private void DrawBoxcast(float upwardDist, float downwardDist, bool collision)
    {
        DrawCubePoints(CubePoints(c_playerData.v_currentPosition + Vector3.up * upwardDist + c_positionData.q_currentModelRotation * c_collisionAttrs.CenterOffset,
                                  c_collisionAttrs.HalfExtents,
                                  c_positionData.q_currentModelRotation),
                       Color.blue);

        // draw dest
        DrawCubePoints(CubePoints(c_playerData.v_currentPosition + Vector3.up * upwardDist + c_positionData.q_currentModelRotation * c_collisionAttrs.CenterOffset + Vector3.down * downwardDist,
                                  c_collisionAttrs.HalfExtents,
                                  c_positionData.q_currentModelRotation),
                       collision ? Color.green : Color.red);
    }

    // debug functions
    Vector3[] CubePoints(Vector3 center, Vector3 extents, Quaternion rotation)
    {
        Vector3[] points = new Vector3[8];
        points[0] = rotation * Vector3.Scale(extents, new Vector3(1, 1, 1)) + center;
        points[1] = rotation * Vector3.Scale(extents, new Vector3(1, 1, -1)) + center;
        points[2] = rotation * Vector3.Scale(extents, new Vector3(1, -1, 1)) + center;
        points[3] = rotation * Vector3.Scale(extents, new Vector3(1, -1, -1)) + center;
        points[4] = rotation * Vector3.Scale(extents, new Vector3(-1, 1, 1)) + center;
        points[5] = rotation * Vector3.Scale(extents, new Vector3(-1, 1, -1)) + center;
        points[6] = rotation * Vector3.Scale(extents, new Vector3(-1, -1, 1)) + center;
        points[7] = rotation * Vector3.Scale(extents, new Vector3(-1, -1, -1)) + center;

        return points;
    }

    void DrawCubePoints(Vector3[] points, Color color)
    {
        Debug.DrawLine(points[0], points[1], color);
        Debug.DrawLine(points[0], points[2], color);
        Debug.DrawLine(points[0], points[4], color);

        Debug.DrawLine(points[7], points[6], color);
        Debug.DrawLine(points[7], points[5], color);
        Debug.DrawLine(points[7], points[3], color);

        Debug.DrawLine(points[1], points[3], color);
        Debug.DrawLine(points[1], points[5], color);

        Debug.DrawLine(points[2], points[3], color);
        Debug.DrawLine(points[2], points[6], color);

        Debug.DrawLine(points[4], points[5], color);
        Debug.DrawLine(points[4], points[6], color);
    }
}