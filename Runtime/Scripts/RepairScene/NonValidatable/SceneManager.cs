using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

/// <summary> Used in the repair a scene 1 task </summary>
[AddComponentMenu("")]
internal class SceneManager : MonoBehaviour
{
    public static Action onWin;

    [SerializeField] private EndMessage _message;
    private DebugObject[] _debugObjects;
    private DefaultTransformObject[] _defaultTransformObjects;
    private Enemy[] _enemies;
    private GroundObject[] _groundObjects;

    private Item[] _items;
    private Npc[] _npcs;
    private StrongEnemy[] _strongEnemies;

    #region Unity Event Functions

    private void Awake()
    {
        _items = FindObjectsOfType <Item>();
        _groundObjects = FindObjectsOfType <GroundObject>();
        _debugObjects = FindObjectsOfType <DebugObject>();
        _defaultTransformObjects = FindObjectsOfType <DefaultTransformObject>();
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
        if (_items.Any(item => !item.HasCollided))
            return false;

        if (_groundObjects.Any(obj => !obj.IsValid))
            return false;

        if (_debugObjects.Any(obj => obj.gameObject.activeSelf))
            return false;

        if (_defaultTransformObjects.Any(obj => !obj.HasDefaultTransform))
            return false;

        if (_npcs.Any(obj => !obj.IsValid))
            return false;

        if (_enemies.Any(obj => !obj.IsValid))
            return false;

        if (_strongEnemies.Any(obj => !obj.IsValid))
            return false;

        return true;
    }

    #endregion
}

}
