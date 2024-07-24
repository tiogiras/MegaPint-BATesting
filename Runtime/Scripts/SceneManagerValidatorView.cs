using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MegaPint
{

// TODO remove from add component menu

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

    private void GetStates()
    {
        _states = new List <ValidatableMonoBehaviourStatus>();

        foreach (ValidatableMonoBehaviourStatus status in Resources.
                     FindObjectsOfTypeAll <ValidatableMonoBehaviourStatus>())
            _states.Add(status);
    }

    private void OnStatusUpdate(ValidationState _)
    {
        if (_states.Any(status => status.State != ValidationState.Ok))
            return;

        onWin?.Invoke();
    }

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
