using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Micosmo.SensorToolkit {

    public interface ISteeringSensor {
        GameObject gameObject { get; }
        SteerSeek Seek { get; }
        SteerAvoid Avoid { get; }
        LocomotionSystem Locomotion { get; }
        bool IsDestinationReached { get; }
        Vector3 GetSteeringVector();
    }

}