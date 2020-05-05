using System;
using OculusSampleFramework;
using UnityEngine;


public class HandsDiabloMoveInput : IMoveInput
{
    const float PinchStrength = 1.0f;
    private Vector3 _pinchStart;
    private LineRenderer _lineRenderer;

    public HandsDiabloMoveInput(LineRenderer lineRenderer)
    {
        _lineRenderer = lineRenderer;
    }

    public Vector3 SampleMoveDirection()
    {
        var pinchPosition = IsPinching() ? GetPinchPosition() : default;
        if (pinchPosition == default)
        {
            _pinchStart = default;
            _lineRenderer.enabled = false;
            return default;
        }

        if (_pinchStart == default)
        {
            _pinchStart = pinchPosition;
            _lineRenderer.enabled = true;
        }

        var sum = _pinchStart + pinchPosition;
        _lineRenderer.SetPositions(new[]
        {
            _pinchStart,
            _pinchStart * 0.75f + pinchPosition * 0.25f,
            _pinchStart * 0.5f + pinchPosition * 0.5f,
            _pinchStart * 0.25f + pinchPosition * 0.75f,
            pinchPosition
        });
        return pinchPosition - _pinchStart;
    }

    private Vector3 GetPinchPosition()
    {
        return HandsManager.Instance.RightHand.PointerPose.position;
    }

    private bool IsPinching()
    {
        return HandsManager.Instance.RightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
    }
}

public class OculusInputProvider : APlayerClient
{
    public override Vector3 MoveDir => _moveDir;
    public override int[] ButtonCounts => _buttonCounts;


    public override void UpdateTarget(Vector3 targetPosition, Vector3 targetForward)
    {
    }

    private Vector3 _moveDir;
    private readonly int[] _buttonCounts = new int[DesktopInputProvider.MobaKeys.Length];
    private IMoveInput _moveInput;

    private void Awake()
    {
        _moveInput = new HandsDiabloMoveInput(GetComponent<LineRenderer>());
    }

    void Update()
    {
        _moveDir = _moveInput.SampleMoveDirection();
        _moveDir += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _moveDir.y = 0;
        _moveDir = Vector3.Normalize(_moveDir.normalized);
    }
}