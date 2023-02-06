using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Micosmo.SensorToolkit.Example {

    public class FollowWaypoints : MonoBehaviour {
        public List<Transform> Waypoints;

        ISteeringSensor steering;

        void OnEnable() {
            steering = GetComponent<ISteeringSensor>();
            StartCoroutine(FollowWaypointsRoutine());
        }

        IEnumerator FollowWaypointsRoutine() {
            var currWaypointIndex = 0;

            while (true) {
                yield return null;
                if (currWaypointIndex >= Waypoints.Count) {
                    currWaypointIndex = 0;
                    continue;
                }
                var currWaypoint = Waypoints[currWaypointIndex];
                yield return SeekRoutine(currWaypoint);
                currWaypointIndex += 1;
            }
        }

        IEnumerator SeekRoutine(Transform destination) {
            steering.Seek.DestinationTransform = destination;
            while (!steering.IsDestinationReached) {
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }
    }

}