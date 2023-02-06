#if PLAYMAKER

using System.Collections;
using HutongGames.PlayMaker;

namespace Micosmo.SensorToolkit.PlayMaker {

    [ActionCategory("SensorToolkit")]
    [Tooltip("Exposes some configuration settings on the Steering Sensor that control its behaviour.")]
    public class SteeringSensorConfigure : SensorToolkitAction3DOr2D<SteeringSensor, SteeringSensor2D> {

        [ActionSection("Seek")]

        [Tooltip("The target distance from the destination.")]
        public FsmFloat targetDistance;

        [Tooltip("The distance from target when it is reached.")]
        public FsmFloat acceptedDistanceRange;

        [ActionSection("Avoid")]

        [Tooltip("The desired distance to avoid obstacles by.")]
        public FsmFloat avoidanceDistance;

        [ActionSection("Other")]

        public FsmFloat interpolationSpeed;

        [Tooltip("Set steering configurations each frame.")]
        public bool everyFrame;

        public override void Reset() {
            base.Reset();
            targetDistance = 0f;
            acceptedDistanceRange = 1f;
            avoidanceDistance = 4f;
            interpolationSpeed = 8f;
            everyFrame = false;
        }

        public override void OnEnter3D(SteeringSensor sensor) {
            OnUpdate3D(sensor);
            if (!everyFrame) {
                Finish();
            }
        }

        public override void OnEnter2D(SteeringSensor2D sensor) {
            OnUpdate2D(sensor);
            if (!everyFrame) {
                Finish();
            }
        }

        public override void OnExit3D(SteeringSensor sensor) { }

        public override void OnExit2D(SteeringSensor2D sensor) { }

        public override void OnUpdate3D(SteeringSensor sensor) {
            sensor.Seek.TargetDistance = targetDistance.Value;
            sensor.Seek.AcceptedDistanceRange = acceptedDistanceRange.Value;
            sensor.Avoid.DesiredDistance = avoidanceDistance.Value;
            sensor.InterpolationSpeed = interpolationSpeed.Value;
        }

        public override void OnUpdate2D(SteeringSensor2D sensor) {
            sensor.Seek.TargetDistance = targetDistance.Value;
            sensor.Seek.AcceptedDistanceRange = acceptedDistanceRange.Value;
            sensor.Avoid.DesiredDistance = avoidanceDistance.Value;
            sensor.InterpolationSpeed = interpolationSpeed.Value;
        }
    }

}

#endif