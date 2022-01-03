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

    public bool CheckForGround4()
    {
        int collisionsDetected = Physics.OverlapBoxNonAlloc(c_playerCollider.transform.position, 
            c_playerCollider.size / 2, a_colliders, 
            c_playerCollider.transform.rotation,
            i_groundCollisionMask);
        if (collisionsDetected > 0)
        {
            Debug.Log(collisionsDetected);
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
                return true;
            }
            c_collisionData.v_attachPoint = Vector3.zero;
        }

        return false;
    }

    public bool CheckForGround5()
    {
        float offsetDist = c_collisionAttrs.CenterOffset.magnitude; // CollisionData.CenterOffset.magnitude;

        c_collisionData.b_collisionDetected = false;

        float boxDist = (c_playerData.f_gravity + (c_aerialMoveData.f_verticalVelocity * - 1)) * Time.deltaTime;
        Debug.Log(boxDist);
        bool hitDetected = Physics.BoxCast(c_playerData.v_currentPosition + c_playerData.q_currentRotation * c_collisionAttrs.CenterOffset,
                            c_collisionAttrs.BodyHalfExtents,
                            Vector3.down,
                            out h_groundCheck,
                            c_positionData.q_currentModelRotation,
                            boxDist,
                            CollisionLayers.ENVIRONMENT);

        if (hitDetected)
        {
            c_collisionData.b_collisionDetected = true;
            c_collisionData.f_contactOffset = h_groundCheck.distance;

            // rotation being weird on land, possibly influencing the offset push
            c_playerData.q_currentRotation = Quaternion.FromToRotation(c_playerData.q_currentRotation * Vector3.up, c_collisionData.v_surfaceNormal) * c_playerData.q_currentRotation;

            // ValidateRotatedAttachPoint();
            // CollectCenteredNormalIfPresent();
        }
        else
        {
            c_collisionData.f_contactOffset = Constants.ZERO_F;
            c_collisionData.v_attachPoint = Vector3.zero;
        }

        return hitDetected;
    }
}
