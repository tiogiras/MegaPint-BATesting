using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MegaPint
{

/// <summary> Handles winning and losing in the task validator view </summary>
[AddComponentMenu("")]
[ExecuteInEditMode]
internal class SceneManagerValidatorView : MonoBehaviour
{
    public static Action onWin;

    private List <ValidatableMonoBehaviourStatus> _states;

    #region Unity Event Functions

    private void OnValidate()
    {
        GetStates();
        SubscribeToStates();
    }

    #endregion

    #region Private Methods

    /// <summary> Get all VMBStates </summary>
    private void GetStates()
    {
        _states = new List <ValidatableMonoBehaviourStatus>();

        foreach (ValidatableMonoBehaviourStatus status in Resources.
                     FindObjectsOfTypeAll <ValidatableMonoBehaviourStatus>())
            _states.Add(status);
    }

    /// <summary> Callback when a VMBStatus was validated </summary>
    /// <param name="_"></param>
    private void OnStatusUpdate(ValidationState _)
    {
        if (_states.Any(status => status.State != ValidationState.Ok))
            return;

        onWin?.Invoke();
    }

    /// <summary> Subscribe to the events of all found states </summary>
    private void SubscribeToStates()
    {
        if (_states.Count == 0)
            return;

        foreach (ValidatableMonoBehaviourStatus status in _states)
            status.onStatusUpdate += OnStatusUpdate;
    }

    #endregion
}

}
