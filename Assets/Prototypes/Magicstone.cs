using OculusSampleFramework;
using UnityEngine;
using UnityEngine.Assertions;

public class Magicstone : MonoBehaviour
{
    [SerializeField] private GameObject _startStopButton = null;

    private InteractableTool _toolInteractingWithMe = null;

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void OnEnable()
    {
        _startStopButton.GetComponent<OculusSampleFramework.Interactable>().InteractableStateChanged
            .AddListener(StartStopStateChanged);
    }

    private void OnDisable()
    {
        if (_startStopButton != null)
        {
            _startStopButton.GetComponent<OculusSampleFramework.Interactable>().InteractableStateChanged
                .RemoveListener(StartStopStateChanged);
        }
    }

    private void StartStopStateChanged(InteractableStateArgs obj)
    {
        bool inActionState = obj.NewInteractableState == InteractableState.ActionState;
        if (inActionState)
        {
        }

        _toolInteractingWithMe = obj.NewInteractableState > InteractableState.Default ? obj.Tool : null;
    }

    [SerializeField] private float _lerpSpeed = 0.2f;
    [SerializeField] private float _pinchScale = 0.1f;
    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    private float _targetScale=1;
    
    private void Update()
    {
        _targetPosition = _originalPosition;
        _targetScale = 1;
        if (_toolInteractingWithMe)
        {
            if (_toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown ||
                _toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDownStay)
            {
                _targetPosition = _toolInteractingWithMe.InteractionPosition;
                _targetScale = _pinchScale;
            }
        }
        transform.position = Vector3.Slerp(transform.position, _targetPosition, Time.deltaTime * _lerpSpeed);
        transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one*_targetScale,Time.deltaTime * _lerpSpeed);
    }
}