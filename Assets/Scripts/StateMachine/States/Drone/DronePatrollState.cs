using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DronePatrollState : BaseState
{

    private Drone drone;

    public DronePatrollState(Drone drone) : base(drone.gameObject)
    {
        this.drone = drone;
        //this.drone.stateMachine.CurrentStateName = "PatrollState";
    }

    public override Type Tick()
    {
        //transform.position += new Vector3(0, 0, -5 * Time.deltaTime);
        if (drone.EnemyPatrol.DestinationReached())
            drone.EnemyPatrol.GoToNextCheckpoint();

        return null;
    }


    public override void OnStateEnter(StateMachine manager)
    {
        // Debug.Log("Entering Patrol state");
    }

    public override void OnStateExit()
    {

    }
}
