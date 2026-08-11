using UnityEditor;
using UnityEngine;

namespace Jeomseon.Coroutine.Editor
{
    internal static class CoroutineCacheSettingsProvider
    {
        [SettingsProvider]
        private static SettingsProvider CreateProvider()
        {
            return new SettingsProvider(
                "Project/Jeomseon/Coroutines",
                SettingsScope.Project)
            {
                label = "Coroutines",
                guiHandler = _ => DrawSettings()
            };
        }

        private static void DrawSettings()
        {
            CoroutineCacheSettings settings = AssetDatabase.LoadAssetAtPath<CoroutineCacheSettings>(CoroutineCacheSettings.AssetPath);
            bool isLimitEnabled = settings && settings.IsWaitForSecondsCacheLimitEnabled;
            int maxCount = settings ? settings.MaxCachedWaitForSecondsCount : CoroutineCacheSettings.DefaultMaxCachedWaitForSecondsCount;

            EditorGUI.BeginChangeCheck();
            isLimitEnabled = EditorGUILayout.Toggle("Limit Wait For Seconds Cache", isLimitEnabled);

            using (new EditorGUI.DisabledScope(!isLimitEnabled))
            {
                maxCount = EditorGUILayout.IntField("Maximum Cached Waits", maxCount);
            }

            if (EditorGUI.EndChangeCheck())
            {
                settings ??= CreateSettingsAsset();
                SerializedObject serializedSettings = new SerializedObject(settings);
                serializedSettings.FindProperty("isWaitForSecondsCacheLimitEnabled").boolValue = isLimitEnabled;
                serializedSettings.FindProperty("maxCachedWaitForSecondsCount").intValue = Mathf.Max(1, maxCount);
                serializedSettings.ApplyModifiedProperties();
                CoroutineHelper.ResetWaitForSecondsCache();
            }

            if (!isLimitEnabled)
            {
                EditorGUILayout.HelpBox(
                    "WaitForSeconds caches every distinct delay value for the lifetime of the application. " +
                    "Enable a limit when delay values can vary without a fixed upper bound.",
                    MessageType.Warning);
            }
        }

        private static CoroutineCacheSettings CreateSettingsAsset()
        {
            const string ResourcesPath = "Assets/Resources";
            const string JeomseonPath = ResourcesPath + "/Jeomseon";
            const string CoroutinesPath = JeomseonPath + "/Coroutines";

            if (!AssetDatabase.IsValidFolder(ResourcesPath))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (!AssetDatabase.IsValidFolder(JeomseonPath))
                AssetDatabase.CreateFolder(ResourcesPath, "Jeomseon");
            if (!AssetDatabase.IsValidFolder(CoroutinesPath))
                AssetDatabase.CreateFolder(JeomseonPath, "Coroutines");

            var settings = ScriptableObject.CreateInstance<CoroutineCacheSettings>();
            AssetDatabase.CreateAsset(settings, CoroutineCacheSettings.AssetPath);
            return settings;
        }
    }
}
