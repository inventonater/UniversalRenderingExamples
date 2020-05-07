using System;
using System.Linq;
using UnityEngine;

public class HandSphereCast : MonoBehaviour
{
    public GameObject _sphereCast;
    public float _radius;
    public Rigidbody _target;
    public float _pullForce = 75f;
    private OVRHand _hand;
    private float _lastJump;
    private Vector3 JumpForce = Vector3.up*500f;

    private void Awake()
    {
        _hand = FindRightHand();
        
        if (!_target)
        {
            _target = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<Rigidbody>();
            _target.transform.localScale = Vector3.one * 2;
        }

        if (!_sphereCast)
        {
            _sphereCast = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _sphereCast.GetComponent<SphereCollider>().enabled = false;
            _sphereCast.transform.localScale = Vector3.one * 2;
        }
    }

    public static OVRHand FindRightHand()
    {
        return FindObjectsOfType<OVRHand>().First(h => h.transform.parent.name.Contains("Right"));
    }
    
    private void FixedUpdate()
    {
        if (!_hand.IsTracked || !_hand.IsDataValid)
        {
            return;
        };

        var pose = _hand.PointerPose;
        transform.SetPositionAndRotation(pose.position, pose.rotation);

        if (Time.time-_lastJump > .5f && _hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            _target.AddForce(JumpForce);
            _lastJump = Time.time;
        }
        
        if(Physics.SphereCast(transform.position, _radius, pose.forward, out var hitInfo))
        {
            _sphereCast.transform.position = hitInfo.point;
            if (_target)
            {
                var toHitPoint = Vector3.ClampMagnitude(hitInfo.point - _target.position, 10f);
                _target.AddForce(toHitPoint * _pullForce * Time.fixedDeltaTime);
            }
        }

    }

}