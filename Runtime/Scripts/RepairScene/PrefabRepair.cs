using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MegaPint.RepairScene.Validatable;
using MegaPint.ValidationRequirement;
using MegaPint.ValidationRequirement.Requirements.GameObjectValidation;
using MegaPint.ValidationRequirement.Requirements.TransformValidation;
using UnityEngine;
using Object = UnityEngine.Object;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MegaPint.RepairScene
{

/// <summary> Handles the repair of all missing prefab instances in the scene </summary>
[ExecuteInEditMode]
internal class PrefabRepair : MonoBehaviour
{
    private static readonly string s_prefabsPath = Path.Combine("MegaPint", "BATesting", "Assets", "Prefabs");

    #region Unity Event Functions

    private void OnValidate()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("B_"))
            return;

        StartCoroutine(ValidateIE());
    }

    #endregion

    #region Public Methods

    /// <summary> Clear the unity console </summary>
    private static void ClearConsole()
    {
#if UNITY_EDITOR
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method?.Invoke(new object(), null);
#endif
    }

    /// <summary> Reset all prefabs for Repair a scene III </summary>
    public static void ResetPrefabs()
    {
        var directoryPath = Path.Join(Application.dataPath, "MegaPint Test Scenes");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var prefabDirectory = Path.Join(directoryPath, "Prefabs");

        if (!Directory.Exists(prefabDirectory))
            Directory.CreateDirectory(prefabDirectory);

#if UNITY_EDITOR
        Object[] prefabs = AssetDatabase.LoadAllAssetsAtPath(prefabDirectory.Replace(Application.dataPath, "Assets"));

        if (prefabs.Length > 0)
        {
            foreach (Object prefab in prefabs)
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(prefab));
        }

        GenerateGoblin(prefabDirectory);
        GenerateLargeRock(prefabDirectory);
        GenerateTile(prefabDirectory);
        GenerateCoins01(prefabDirectory);
        GenerateCoins03(prefabDirectory);
        GeneratePotion(prefabDirectory);
        GenerateStatue(prefabDirectory);
#endif
    }

    /// <summary> Reset the prefab for Validator View </summary>
    public static void ResetSinglePrefab()
    {
        var directoryPath = Path.Join(Application.dataPath, "MegaPint Test Scenes");

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var prefabDirectory = Path.Join(directoryPath, "Prefabs");

        if (!Directory.Exists(prefabDirectory))
            Directory.CreateDirectory(prefabDirectory);

        var singleDirectory = Path.Join(prefabDirectory, "Single");

        if (!Directory.Exists(singleDirectory))
            Directory.CreateDirectory(singleDirectory);

#if UNITY_EDITOR
        Object[] prefabs = AssetDatabase.LoadAllAssetsAtPath(prefabDirectory.Replace(Application.dataPath, "Assets"));

        if (prefabs.Length > 0)
        {
            foreach (Object prefab in prefabs)
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(prefab));
        }
#endif

        var instance = new GameObject("Test Validation Prefab");
        var vmb = (ValidatableMonoBehaviour)instance.AddComponent <SomeVMB>();

        List <ScriptableValidationRequirement> requirements = new()
        {
            new RequireDefaultTransform(), new RequireGameObjectActive(), new RequireLayer("Default")
        };

        vmb.SetRequirements(requirements);

        instance.SetActive(false);
        instance.layer = LayerMask.NameToLayer("UI");

        Transform myTransform = instance.transform;
        myTransform.localPosition = new Vector3(6, -2, 7);
        myTransform.localRotation = Quaternion.Euler(new Vector3(153, -4, 141));

#if UNITY_EDITOR
        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(singleDirectory, "Test Validation Prefab.prefab"));
#endif

        DestroyImmediate(instance);
    }

    #endregion

    #region Private Methods

    /// <summary> Start to validate the scene </summary>
    /// <returns> Coroutine </returns>
    private static IEnumerator ValidateIE()
    {
        yield return null;

        ClearConsole();

        GameObject[] objects = FindObjectsByType <GameObject>(FindObjectsSortMode.None);

        for (var i = objects.Length - 1; i >= 0; i--)
        {
            GameObject o = objects[i];

            if (!o.name.Contains("Missing Prefab"))
                continue;

#if UNITY_EDITOR
            RepairPrefab(o);
#endif
        }
    }

    #endregion

#if UNITY_EDITOR
    /// <summary> Generate the coins01 prefab </summary>
    /// <param name="directory"> Prefab directory </param>
    private static void GenerateCoins01(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[ITEM] SM_Item_Coins_01"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();

        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[ITEM] SM_Item_Coins_01.prefab"));

        DestroyImmediate(instance);
    }

    /// <summary> Generate the coins03 prefab </summary>
    /// <param name="directory"> Prefab directory </param>
    private static void GenerateCoins03(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[ITEM] SM_Item_Coins_03"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();

        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[ITEM] SM_Item_Coins_03.prefab"));

        DestroyImmediate(instance);
    }

    /// <summary> Generate the goblin prefab </summary>
    /// <param name="directory"> Prefab directory </param>
    private static void GenerateGoblin(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[E] SK_Character_Goblin_Male"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Enemy>();

        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[E] SK_Character_Goblin_Male.prefab"));

        DestroyImmediate(instance);
    }

    /// <summary> Generate the large rock prefab </summary>
    /// <param name="directory"> Prefab directory </param>
    private static void GenerateLargeRock(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[GR] SM_Env_Rock_Flat_Large_03"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <GroundObject>();

        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[GR] SM_Env_Rock_Flat_Large_03.prefab"));

        DestroyImmediate(instance);
    }

    /// <summary> Generate the potion prefab </summary>
    /// <param name="directory"> Prefab directory </param>
    private static void GeneratePotion(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[ITEM] SM_Item_Potion_07"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();

        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[ITEM] SM_Item_Potion_07.prefab"));

        DestroyImmediate(instance);
    }

    /// <summary> Generate the statue prefab </summary>
    /// <param name="directory"> Prefab directory </param>
    private static void GenerateStatue(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[NPC] SM_Env_Statue_04"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <Item>();

        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[NPC] SM_Env_Statue_04.prefab"));

        DestroyImmediate(instance);
    }

    /// <summary> Generate the tile prefab </summary>
    /// <param name="directory"> Prefab directory </param>
    private static void GenerateTile(string directory)
    {
        var o = Resources.Load <GameObject>(Path.Join(s_prefabsPath, "[GR] SM_Env_Tiles_04"));

        GameObject instance = Instantiate(o);
        instance.AddComponent <GroundObject>();

        PrefabUtility.SaveAsPrefabAsset(instance, Path.Join(directory, "[GR] SM_Env_Tiles_04.prefab"));

        DestroyImmediate(instance);
    }

    /// <summary> Repair any invalid object in the scene that is missing it's prefab instance </summary>
    /// <param name="o"> Invalid object </param>
    private static void RepairPrefab(GameObject o)
    {
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
        {
            Debug.LogWarning($"No Prefab found for {prefabName}! Try to reset the current task.");

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
        newTransform.localPosition = oldTransform.localPosition;
        newTransform.localRotation = oldTransform.localRotation;

        for (var i = 0; i < oldTransform.childCount; i++)
        {
            Transform child = oldTransform.GetChild(i);
            child.SetParent(newTransform);
        }

        Component[] components =
            o.GetComponents(typeof(Component)).Where(c => c.GetType() != typeof(Transform)).ToArray();

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
#endif
}

}
