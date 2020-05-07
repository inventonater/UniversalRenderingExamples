using OculusSampleFramework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class MagePickable : MonoBehaviour
{
    [SerializeField] private GameObject _startStopButton = null;

    private InteractableTool _toolInteractingWithMe = null;

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

    private void Update()
    {
        if (!_toolInteractingWithMe)
        {
            UnGrab();
            return;
        }
        if (_toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputDown)
        {
            Debug.Log("Down");
            Grab();
        }

        if (_toolInteractingWithMe.ToolInputState == ToolInputState.PrimaryInputUp)
        {
            Debug.Log("UP");
            UnGrab();
        }
    }

    private void Grab()
    {
        transform.parent = _toolInteractingWithMe.ToolTransform;
        GetComponent<NavMeshAgent>().enabled = false;
    }

    private void UnGrab()
    {
        transform.parent = null;
        GetComponent<NavMeshAgent>().enabled = true;
    }
}