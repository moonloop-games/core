using System;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Moonloop.Core {

    public class VelocityMeasure : MonoBehaviour 
    {
        [Range(1, 10)]
        [Tooltip("Number of consecutive frames to sample positions from when calculating velocity. Increasing this can smooth out velocity readings.")]
        public int velocitySamples = 5;
        
        [ReadOnly, ShowInInspector]
        Vector3 _velocity;
        public Vector3 Velocity => _velocity;

        List<Vector3> _velocities = new List<Vector3>();
        Vector3 _compoundVelocity;
        Vector3 _previousPos;

        // the minimum timescale at which we'll calculate velocity
        const float minTimeScale = 0.01f;

        public void OnEnable()
        {
            _velocity = Vector3.zero;
            _compoundVelocity = Vector3.zero;
            _previousPos = transform.position;
        }

        public void Update()
        {
            if (Time.timeScale < minTimeScale) 
                return;

            _velocities.Add(PVelocity());

            // if we have more than the max number of samples, remove the oldest one
            if (_velocities.Count > velocitySamples) 
                _velocities.RemoveAt(0);

            _compoundVelocity = Vector3.zero;
            for (int i = 0; i < _velocities.Count; i++)
                _compoundVelocity += _velocities[i];

            _velocity = _compoundVelocity / _velocities.Count;
        }
        
        Vector3 PVelocity()
        {
            // to avoid any weirdness from divide by zero, 
            // just return zero if the timescale is too low
            if (Time.timeScale < minTimeScale) 
            {
                _previousPos = transform.position;
                return Vector3.zero;
            }

            Vector3 returnVel = (transform.position - _previousPos) / Time.deltaTime;

            // clamp the velocity to a reasonable value
            returnVel = Vector3.ClampMagnitude(returnVel, 99999);

            // store the current position for the next frame
            _previousPos = transform.position;
            return returnVel;
        }
    }
}