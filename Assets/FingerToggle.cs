using System;
using System.Linq;
using UnityEngine;

public class FingerToggle : MonoBehaviour
{
    public GameObject PowerPrefab;
    private GameObject _power;

    private OVRHand _hand;
    private OVRSkeleton _skeleton;
    private Vector3 _defaultScale = Vector3.one * 0.01f;
    private void Awake()
    {
        _hand = FindObjectsOfType<OVRHand>().First(h => h.transform.parent.name.Contains("Right"));
        _skeleton = _hand.GetComponent<OVRSkeleton>();
        _power = Instantiate(PowerPrefab);
        _power.transform.localScale = Vector3.zero;
        _power.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (!_hand.IsTracked || !_hand.IsDataValid || !_skeleton.IsInitialized || !_skeleton.IsDataValid)
        {
            return;
        };

        var pose = _hand.PointerPose;
        Velocity = (pose.position - transform.position) / Time.deltaTime;
        transform.SetPositionAndRotation(pose.position, pose.rotation);

        _power.transform.localScale = _defaultScale * (_hand.GetFingerPinchStrength(OVRHand.HandFinger.Index)*3+1);
        var indexTip = _skeleton.Bones.FirstOrDefault(b => b.Id == OVRSkeleton.BoneId.Hand_IndexTip);
        if (indexTip != default)
        {
            _power.transform.SetPositionAndRotation(indexTip.Transform.position, indexTip.Transform.rotation);
        }
    }

    public Vector3 Velocity { get; set; }
}