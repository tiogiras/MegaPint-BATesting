﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace MegaPint.RepairScene.NonValidatable
{

internal class SceneManager : MonoBehaviour
{
    public static Action onWin;
    
    [SerializeField] private EndMessage _message;

    private Item[] _items;
    private GroundObject[] _groundObjects;
    private DebugObject[] _debugObjects;
    private DefaultTransformObject[] _defaultTransformObjects;
    private Npc[] _npcs;
    private Enemy[] _enemies;
    private StrongEnemy[] _strongEnemies;

    private void Awake()
    {
        _items = Resources.FindObjectsOfTypeAll <Item>();
        _groundObjects = Resources.FindObjectsOfTypeAll <GroundObject>();
        _debugObjects = Resources.FindObjectsOfTypeAll <DebugObject>();
        _defaultTransformObjects = Resources.FindObjectsOfTypeAll <DefaultTransformObject>();
        _npcs = Resources.FindObjectsOfTypeAll <Npc>();
        _enemies = Resources.FindObjectsOfTypeAll <Enemy>();
        _strongEnemies = Resources.FindObjectsOfTypeAll <StrongEnemy>();
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
}

}
