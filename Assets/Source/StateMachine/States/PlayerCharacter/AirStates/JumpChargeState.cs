using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChargeState : iState
{
    private PlayerData c_playerData;
    private PlayerPositionData c_positionData;
    private CollisionData c_collisionData;
    private AerialMoveData c_aerialMoveData;
    private IncrementCartridge cart_increment;
    
    public JumpChargeState(ref PlayerData playerData, ref PlayerPositionData positionData, ref CollisionData collisionData, ref AerialMoveData aerialMoveData, ref IncrementCartridge incr)
    {
        this.c_playerData = playerData;
        this.c_positionData = positionData;
        this.c_aerialMoveData = aerialMoveData;
        this.c_collisionData = collisionData;
        this.cart_increment = incr;
    }

    public void Act()
    {
        float chargeCap = c_playerData.f_jumpPower;
        float chargeValue = c_playerData.f_currentJumpCharge;
        float chargeDelta = c_playerData.f_jumpChargeRate;

        cart_increment.Increment(ref chargeValue, chargeDelta * Time.deltaTime, chargeCap);

        c_playerData.f_currentJumpCharge = chargeValue;

        Vector3 currentNormal = c_playerData.v_currentNormal;
        Quaternion currentRotation = c_playerData.q_currentRotation;
        Quaternion currentModelRotation = c_positionData.q_currentModelRotation;

        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentRotation,
                                            ref currentNormal,
                                            1);
        AngleCalculationCartridge.AlignToSurfaceByTail(c_collisionData.v_surfaceNormal,
                                            ref currentModelRotation,
                                            ref currentNormal,
                                            1);

        c_playerData.v_currentNormal = currentNormal;
        c_playerData.q_currentRotation = currentRotation;
        c_positionData.q_currentModelRotation = currentModelRotation;
    }

    public void TransitionAct()
    {

    }

    public StateRef GetNextState(Command cmd)
    {
        if (cmd == Command.FALL)
        {
            c_playerData.f_currentJumpCharge = Constants.ZERO_F;
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.START_BOOST)
        {
            c_playerData.f_currentJumpCharge = Constants.ZERO_F;
            return StateRef.GROUNDED_BOOSTING;
        }
        if (cmd == Command.JUMP)
        {
            c_playerData.v_currentPosition += Vector3.up * 0.2f;
            return StateRef.AIRBORNE;
        }
        if (cmd == Command.CRASH)
        {
            return StateRef.DISABLED;
        }
        return StateRef.CHARGING;
    }
}

