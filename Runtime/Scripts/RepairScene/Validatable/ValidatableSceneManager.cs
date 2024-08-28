using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace MegaPint.RepairScene.Validatable
{

/// <summary> Used in the repair a scene 2 and 3 task </summary>
[AddComponentMenu("")]
internal class ValidatableSceneManager : MonoBehaviour
{
    public static Action onWin;

    [SerializeField] private EndMessage _message;
    private DebugObject[] _debugObjects;
    private Enemy[] _enemies;
    private GroundObject[] _groundObjects;

    private Item[] _items;
    private Npc[] _npcs;

    private ValidatableMonoBehaviourStatus[] _states;
    private StrongEnemy[] _strongEnemies;

    #region Unity Event Functions

    private void Awake()
    {
        _states = FindObjectsOfType <ValidatableMonoBehaviourStatus>();

        _items = FindObjectsOfType <Item>();
        _groundObjects = FindObjectsOfType <GroundObject>();
        _debugObjects = FindObjectsOfType <DebugObject>();
        _npcs = FindObjectsOfType <Npc>();
        _enemies = FindObjectsOfType <Enemy>();
        _strongEnemies = FindObjectsOfType <StrongEnemy>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);

        var valid = ValidateScene();

        if (valid)
            onWin?.Invoke();

        _message.ShowMessage(valid);
    }

    #endregion

    #region Private Methods

    /// <summary> Validate the scene </summary>
    /// <returns> If the scene is valid </returns>
    private bool ValidateScene()
    {
        var valid = true;

        if (_debugObjects.Any(obj => !obj.ValidateManually()))
        {
            Debug.LogWarning("Some DebugObject does not conform to the specified rules.");
            valid = false;
        }

        if (_enemies.Any(obj => !obj.ValidateManually()))
        {
            Debug.LogWarning("Some Enemy does not conform to the specified rules.");
            valid = false;
        }

        if (_groundObjects.Any(obj => !obj.ValidateManually()))
        {
            Debug.LogWarning("Some GroundObject does not conform to the specified rules.");
            valid = false;
        }

        if (_items.Any(obj => !obj.ValidateManually()))
        {
            Debug.LogWarning("Some Item does not conform to the specified rules.");
            valid = false;
        }

        if (_npcs.Any(obj => !obj.ValidateManually()))
        {
            Debug.LogWarning("Some NPC does not conform to the specified rules.");
            valid = false;
        }

        if (_strongEnemies.Any(obj => !obj.ValidateManually()))
        {
            Debug.LogWarning("Some StrongEnemy does not conform to the specified rules.");
            valid = false;
        }

        foreach (ValidatableMonoBehaviourStatus status in _states)
        {
            status.ValidateStatus();

            if (status.State == ValidationState.Ok)
                continue;

            Debug.LogWarning("Some VMB is reporting invalid state.");

            return false;
        }

        return valid;
    }

    #endregion
}

}
