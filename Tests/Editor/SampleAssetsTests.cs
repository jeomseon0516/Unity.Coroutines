using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jeomseon.Tests
{
    public sealed class SampleAssetsTests
    {
        private const string ScenePath =
            "Packages/com.jeomseon.unity.coroutines/Samples~/BasicUsage/CoroutineLifetimeSample.unity";

        [Test]
        public void CoroutineLifetimeSample_ImportsWithoutMissingScripts()
        {
            AssetDatabase.ImportAsset(ScenePath, ImportAssetOptions.ForceSynchronousImport);
            Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);

            try
            {
                MonoBehaviour[] components = scene
                    .GetRootGameObjects()
                    .SelectMany(gameObject => gameObject.GetComponentsInChildren<MonoBehaviour>(true))
                    .ToArray();

                Assert.That(components, Has.None.Null);
            }
            finally
            {
                EditorSceneManager.CloseScene(scene, removeScene: true);
            }
        }
    }
}
