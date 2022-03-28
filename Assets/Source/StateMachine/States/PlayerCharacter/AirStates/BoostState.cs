using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostState : iState
{

    private PlayerData c_playerData;
    private AerialMoveData c_aerialMoveData;
    private PlayerPositionData c_positionData;
    private CollisionData c_collisionData;
    private PlayerHandlingData c_turnData;

    public BoostState(ref PlayerData playerData,
                         ref AerialMoveData aerialMoveData,
                         ref CollisionData collisionData,
                         ref PlayerPositionData positionData,
                         ref PlayerHandlingData turnData)
    {
        this.c_playerData = playerData;
        this.c_aerialMoveData = aerialMoveData;
        this.c_positionData = positionData;
        this.c_collisionData = collisionData;
        this.c_turnData = turnData;
    }
    public void Act()
    {
        Vector3 currentNormal = c_collisionData.v_surfaceNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;
        float currentVelocity = c_playerData.f_currentSpeed;

        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentRotation,
                                            ref currentNormal,
                                            1);
        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentModelRotation,
                                            ref currentNormal,
                                            1);

        // boosting adds an extra acceleration force, scale boost rate by vertical direction
        float scaledBoost = Mathf.Max((c_playerData.q_currentRotation * Vector3.forward).y * c_playerData.f_boostAcceleration * Constants.NEGATIVE_ONE, Constants.ZERO_F);
        AccelerationCartridge.AccelerateAbs(ref currentVelocity, scaledBoost * Time.fixedDeltaTime, c_playerData.f_topSpeed);

        c_playerData.f_currentSpeed = currentVelocity;
        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.q_currentModelRotation = currentModelRotation;

        c_aerialMoveData.f_verticalVelocity = Vector3.Dot(c_playerData.q_currentRotation * Vector3.forward, Vector3.up) * c_playerData.f_currentSpeed;
    }

    public void TransitionAct()
    {
        c_playerData.f_currentJumpCharge = Constants.ZERO_F;
        c_playerData.f_currentAirVelocity = Constants.ZERO_F;

        c_playerData.f_currentRaycastDistance = c_playerData.f_raycastDistance;
        c_playerData.v_currentDown = c_collisionData.v_surfaceNormal * -1;
        c_turnData.f_turnTopSpeed *= c_turnData.f_boostTurnRatio;
        c_playerData.f_topSpeed += c_playerData.f_boostBonus;

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.STOP_BOOST)
        {
            c_playerData.f_currentJumpCharge = c_playerData.f_baseJumpPower;
            c_turnData.f_turnTopSpeed /= c_turnData.f_boostTurnRatio;
            c_playerData.f_topSpeed -= c_playerData.f_boostBonus;
            return StateRef.GROUNDED;
        }
        if (cmd == Command.FALL)
        {
            c_turnData.f_turnTopSpeed /= c_turnData.f_boostTurnRatio;
            c_playerData.f_topSpeed -= c_playerData.f_boostBonus;
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.CRASH)
        {
            c_turnData.f_turnTopSpeed /= c_turnData.f_boostTurnRatio;
            c_playerData.f_topSpeed -= c_playerData.f_boostBonus;
            return StateRef.DISABLED;
        }
        return StateRef.GROUNDED_BOOSTING;
    }
}
