using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jeomseon.Unity.Coroutines.Editor
{
    internal static class CoroutineCacheSettingsProvider
    {
        private const string LimitLabel = "Limit Wait For Seconds Cache";
        private const string MaxCountLabel = "Maximum Cached Waits";
        private const string UnlimitedWarning =
            "WaitForSeconds caches every distinct delay value for the lifetime of the application. " +
            "Enable a limit when delay values can vary without a fixed upper bound.";

        [SettingsProvider]
        private static SettingsProvider CreateProvider()
        {
            return new SettingsProvider(
                "Project/Jeomseon/Coroutines",
                SettingsScope.Project)
            {
                label = "Coroutines",
                activateHandler = (_, rootElement) => BuildSettings(rootElement)
            };
        }

        private static void BuildSettings(VisualElement rootElement)
        {
            rootElement.Clear();

            // activateHandler는 guiHandler와 달리 제목을 그리지 않으므로 Settings 창 IMGUI 제목과 같은 형태로 직접 추가합니다.
            var titleLabel = new Label("Coroutines")
            {
                style =
                {
                    fontSize = 19,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginLeft = 10,
                    marginTop = 1,
                    marginBottom = 10
                }
            };
            titleLabel.AddToClassList("title");
            rootElement.Add(titleLabel);

            var content = new VisualElement
            {
                style =
                {
                    marginLeft = 10,
                    marginRight = 10
                }
            };
            rootElement.Add(content);

            CoroutineCacheSettings settings = AssetDatabase.LoadAssetAtPath<CoroutineCacheSettings>(CoroutineCacheSettings.AssetPath);
            var limitToggle = new Toggle(LimitLabel);
            var maxCountField = new IntegerField(MaxCountLabel);
            var warningBox = new HelpBox(UnlimitedWarning, HelpBoxMessageType.Warning);
            content.Add(limitToggle);
            content.Add(maxCountField);
            content.Add(warningBox);

            if (settings)
            {
                var serializedSettings = new SerializedObject(settings);
                SerializedProperty limitProperty = serializedSettings.FindProperty("isWaitForSecondsCacheLimitEnabled");
                SerializedProperty maxCountProperty = serializedSettings.FindProperty("maxCachedWaitForSecondsCount");

                // BindProperty로 SerializedObject를 거쳐 기록하므로 Undo가 동작합니다.
                limitToggle.BindProperty(limitProperty);
                maxCountField.BindProperty(maxCountProperty);
                maxCountField.RegisterValueChangedCallback(evt =>
                {
                    if (evt.newValue < 1)
                    {
                        maxCountField.value = 1;
                    }
                });

                content.TrackSerializedObjectValue(serializedSettings, so =>
                {
                    RefreshState(so.FindProperty("isWaitForSecondsCacheLimitEnabled").boolValue, maxCountField, warningBox);
                    CoroutineHelper.ResetWaitForSecondsCache();
                });

                RefreshState(limitProperty.boolValue, maxCountField, warningBox);
            }
            else
            {
                // 설정 에셋이 없으면 기본값을 표시하고, 처음 값을 바꿀 때 에셋을 만든 뒤 바인딩된 UI로 다시 구성합니다.
                limitToggle.SetValueWithoutNotify(false);
                maxCountField.SetValueWithoutNotify(CoroutineCacheSettings.DefaultMaxCachedWaitForSecondsCount);
                RefreshState(false, maxCountField, warningBox);

                limitToggle.RegisterValueChangedCallback(_ => CreateAndApply(rootElement, limitToggle.value, maxCountField.value));
                maxCountField.RegisterValueChangedCallback(_ => CreateAndApply(rootElement, limitToggle.value, maxCountField.value));
            }

            // guiHandler는 매 프레임 에셋을 다시 읽었으므로, 창이 열린 동안 에셋이 외부에서 생성·삭제되면 다시 구성합니다.
            bool hasSettings = settings;
            rootElement.schedule.Execute(() =>
            {
                bool exists = AssetDatabase.LoadAssetAtPath<CoroutineCacheSettings>(CoroutineCacheSettings.AssetPath);
                if (exists != hasSettings)
                {
                    BuildSettings(rootElement);
                }
            }).Every(500).Until(() => content.panel is null);
        }

        private static void RefreshState(bool isLimitEnabled, VisualElement maxCountField, VisualElement warningBox)
        {
            maxCountField.SetEnabled(isLimitEnabled);
            warningBox.style.display = isLimitEnabled ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private static void CreateAndApply(VisualElement rootElement, bool isLimitEnabled, int maxCount)
        {
            CoroutineCacheSettings settings = AssetDatabase.LoadAssetAtPath<CoroutineCacheSettings>(CoroutineCacheSettings.AssetPath);
            settings ??= CreateSettingsAsset();
            var serializedSettings = new SerializedObject(settings);
            serializedSettings.FindProperty("isWaitForSecondsCacheLimitEnabled").boolValue = isLimitEnabled;
            serializedSettings.FindProperty("maxCachedWaitForSecondsCount").intValue = Mathf.Max(1, maxCount);
            serializedSettings.ApplyModifiedProperties();
            CoroutineHelper.ResetWaitForSecondsCache();

            // 값 변경 콜백 도중 자신을 제거하지 않도록 다음 틱에 다시 구성합니다.
            rootElement.schedule.Execute(() => BuildSettings(rootElement));
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
