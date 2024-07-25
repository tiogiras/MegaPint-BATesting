using System;
using System.Collections;
using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

internal class SceneManager : MonoBehaviour
{
    public static Action onWin;
    
    [SerializeField] private EndMessage _message;
    
    private ValidatableMonoBehaviourStatus[] _states;

    private void Awake()
    {
        _states = Resources.FindObjectsOfTypeAll <ValidatableMonoBehaviourStatus>();
    }
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);

        var valid = ValidateScene();

        if (valid)
            onWin?.Invoke();

        _message.ShowMessage(valid);
    }

    private bool ValidateScene()
    {
        foreach (ValidatableMonoBehaviourStatus status in _states)
        {
            status.ValidateStatus();
            
            if (status.State != ValidationState.Ok)
                return false;
        }

        return true;
    }
}

}
