using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Micosmo.SensorToolkit {

    public interface ISignalProcessor {
        bool ProcessOutput(ref Signal signal);
    }

    [Serializable]
    public class MapToRigidBodyFilter : ISignalProcessor {
        public Sensor Sensor;
        public bool IsRigidBodyMode;
        
        public bool Is2D { get; set; }

        public bool ProcessOutput(ref Signal signal) {
            if (!IsRigidBodyMode) {
                return true;
            }

            GameObject rbGo = null;
            if (Is2D) {
                var col = signal.Object.GetComponent<Collider2D>();
                if (col != null) {
                    rbGo = col.attachedRigidbody?.gameObject;
                }
            } else {
                var col = signal.Object.GetComponent<Collider>();
                if (col != null) {
                    rbGo = col.attachedRigidbody?.gameObject;
                }
            }

            if (rbGo != null) {
                signal.Shape = new Bounds(signal.Shape.center - (rbGo.transform.position - signal.Object.transform.position), signal.Shape.size);
                signal.Object = rbGo;
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class MapToSignalProxyFilter : ISignalProcessor {
        public bool ProcessOutput(ref Signal signal) {
            var origObject = signal.Object;
            var targetObject = SignalProxy.GetProxyTarget(signal.Object);
            var deltaPos = (targetObject.transform.position - origObject.transform.position);
            signal.Object = targetObject;
            signal.Shape = new Bounds(signal.Shape.center - deltaPos, signal.Shape.size);
            return true;
        }
    }
}