using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MegaPint.RepairScene.Validatable;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace MegaPint.RepairScene
{

/// <summary> Handles the repair of all missing prefab instances in the scene </summary>
[ExecuteInEditMode]
internal class PrefabRepair : MonoBehaviour
{
    [ContextMenu("Validate")]
    private void OnValidate()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("B_"))
            return;
        
        StartCoroutine(ValidateIE());
    }

    private IEnumerator ValidateIE()
    {
        yield return null;
        
        GameObject[] objects = FindObjectsByType <GameObject>(FindObjectsSortMode.None);

        for (var i = objects.Length - 1; i >= 0; i--)
        {
            GameObject o = objects[i];
            
            if (!o.name.Contains("Missing Prefab"))
                continue;

            RepairPrefab(o);
        }
    }
    
    private void RepairPrefab(GameObject o)
    {
        Debug.Log($"REPAIRING GAMEOBJECT: {o}");
        
        var prefabName = o.name.Split(" (")[0];
        
        var directoryPath = Path.Join(Application.dataPath, "MegaPint Test Scenes");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        
        var prefabDirectory = Path.Join(directoryPath, "Prefabs");
        
        if (!Directory.Exists(prefabDirectory))
            Directory.CreateDirectory(prefabDirectory);

        var prefabPath = Path.Join(prefabDirectory, $"{prefabName}.prefab");
        prefabPath = prefabPath.Replace(Application.dataPath, "Assets");

        var loadedPrefab = AssetDatabase.LoadAssetAtPath <GameObject>(prefabPath);
        
        if (loadedPrefab == null)
            loadedPrefab = CreatePrefab();

        if (loadedPrefab == null)
        {
            Debug.LogWarning($"No Prefab found for {prefabName}");
            return;
        }

        var newObject = new GameObject(o.name.Split(" (Missing Prefab")[0]);
        
        Transform newTransform = newObject.transform;
        Transform oldTransform = o.transform;

        PrefabUtility.ConvertToPrefabInstance(
            newObject,
            loadedPrefab,
            new ConvertToPrefabInstanceSettings(),
            InteractionMode.AutomatedAction);
        
        newTransform.parent = oldTransform.parent;

        Debug.Log($"OLD POSITION: {oldTransform.localPosition}");
        
        newTransform.localPosition = oldTransform.localPosition;
        newTransform.localRotation = oldTransform.localRotation;
        newTransform.localScale = oldTransform.localScale;
        
        for (var i = 0; i < oldTransform.childCount; i++)
        {
            oldTransform.GetChild(i).parent = newTransform;
        }
        
        Component[] components = o.GetComponents(typeof(Component)).Where(c => c.GetType() != typeof(Transform)).ToArray();

        if (components.Length > 0)
        {
            foreach (Component component in components)
            {
                Component newComponent = newObject.AddComponent(component.GetType());
                EditorUtility.CopySerialized(component, newComponent);
            }   
        }
        
        DestroyImmediate(o);
    }

    public static void ResetPrefabs()
    {
        var directoryPath = Path.Join(Application.dataPath, "MegaPint Test Scenes");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        
        var prefabDirectory = Path.Join(directoryPath, "Prefabs");
        
        if (!Directory.Exists(prefabDirectory))
            Directory.CreateDirectory(prefabDirectory);

        var prefabs = AssetDatabase.LoadAllAssetsAtPath(prefabDirectory.Replace(Application.dataPath, "Assets"));

        if (prefabs.Length > 0)
        {
            foreach (UnityEngine.Object prefab in prefabs)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(prefab));
            }
        }
        
        GenerateGoblin(prefabDirectory);
        GenerateLargeRock(prefabDirectory);
        GenerateTile(prefabDirectory);
        GenerateCoins01(prefabDirectory);
        GenerateCoins03(prefabDirectory);
        GeneratePotion(prefabDirectory);
        GenerateStatue(prefabDirectory);
    }

    private static string s_prefabsPath = Path.Combine("MegaPint", "BATesting", "Assets", "Prefabs");
    
    private GameObject CreatePrefab()
    {
        return null;
    }

    private static void GenerateGoblin(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[E] SK_Character_Goblin_Male"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Enemy>();
        
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[E] SK_Character_Goblin_Male.prefab"));
        
        DestroyImmediate(instance);
    }
    
    private static void GenerateTile(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[GR] SM_Env_Tiles_04"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <GroundObject>();
        
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[GR] SM_Env_Tiles_04.prefab"));
        
        DestroyImmediate(instance);
    }
    
    private static void GenerateLargeRock(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[GR] SM_Env_Rock_Flat_Large_03"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <GroundObject>();
        
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[GR] SM_Env_Rock_Flat_Large_03.prefab"));
        
        DestroyImmediate(instance);
    }
    
    private static void GenerateCoins01(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[ITEM] SM_Item_Coins_01"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();
        
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[ITEM] SM_Item_Coins_01.prefab"));
        
        DestroyImmediate(instance);
    }
    
    private static void GenerateCoins03(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[ITEM] SM_Item_Coins_03"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();
        
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[ITEM] SM_Item_Coins_03.prefab"));
        
        DestroyImmediate(instance);
    }
    
    private static void GeneratePotion(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[ITEM] SM_Item_Potion_07"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();
        
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[ITEM] SM_Item_Potion_07.prefab"));
        
        DestroyImmediate(instance);
    }

    private static void GenerateStatue(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[NPC] SM_Env_Statue_04"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();
        
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[NPC] SM_Env_Statue_04.prefab"));
        
        DestroyImmediate(instance);
    }
}

}
