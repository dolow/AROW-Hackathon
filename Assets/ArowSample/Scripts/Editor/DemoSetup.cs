using System.IO;
using UnityEditor;
using UnityEngine;

namespace ArowSample.Scripts.Editor
{

public class DemoSetup
{
    [MenuItem("ArowSample/Setup for Demo", false, MenuItemProperty.ArowSampleDemoSetupGroup)]
    private static void CreateAsset_Normal()
    {
        // フォルダがあるかチェック、なければ打ち切り
        var path = "Assets/ArowSample/Resources/Demo/";

        if (!Directory.Exists(path))
        {
            Debug.LogWarning(path + " が存在しません。");
            return;
        }

        var config = BuildingConfigCreator.CreateNormalCreateConfig();
        AssetDatabase.CreateAsset(config, Path.Combine(path, "BuildingConfig.asset"));
        var poi_config = CreateSampleCategoryPoiConfig.CreateAssetSampleData();
        AssetDatabase.CreateAsset(poi_config, Path.Combine(path, "CategoryPoiConfig_DemoSample.asset"));
        var landmark_config = CreateSampleLandmarkPoiConfig.CreateAssetSampleData();
        AssetDatabase.CreateAsset(landmark_config, Path.Combine(path, "LandmarkPoiConfig_DemoSample.asset"));
        var prefab_config = CreateSamplePrefabConfigList.CreateAssetSampleData();
        AssetDatabase.CreateAsset(prefab_config, Path.Combine(path, "PrefabConfigList_demo.asset"));
        AssetDatabase.Refresh();
    }
}

}
