using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASK_SDK
{
    public class BSceneUpLoadWindow : SubUpLoadBase
    {
        private Scene scene;
        private string scene_name;
        private string txt_path;
        private string scene_path;
        private SceneAsset sceneAsset;

        public override string Build()
        {
            MakeInfo();
            return AssetUtils.AssetScene(sceneAsset, outputDir, puffix, UnityEditor.BuildTarget.StandaloneWindows64, UnityEditor.BuildOptions.UncompressedAssetBundle | UnityEditor.BuildOptions.BuildAdditionalStreamedScenes);
        }

        public override void MakeInfo()
        {
            //创建IMG目录
            base.MakeInfo();

            //收集数据
            //json
            string json = JsonUtility.ToJson(InfoWindow.CurrentWindow.model.GetData());

            using (StreamWriter sw = File.CreateText(txt_path))
            {
                sw.Write(json);
            }

            var window = InfoWindow.CurrentWindow as BSceneInfoWindow;

            //展示图
            MakePicture(window.ThumbnailT2d, "scene_thumbnail");
            //预览图
            MakeRendererPicture(window.Preview2d, "scene_preview");

            AssetDatabase.Refresh();

        }

        public override void Init()
        {
            outputDir = Application.persistentDataPath;
            num_id = 0;
            if (AssetWindow.CurrentSubWindow != null)
            {
                scene = AssetWindow.CurrentSubWindow.GetCurrentScene();
                scene_name = scene.name;
                puffix = "scene";
                txt_path = Path.Combine(outputDir, $"{InfoWindow.CurrentWindow.model.GetData().Data_ID}.txt");
                scene_path = Path.Combine(outputDir, $"{scene_name}.{puffix}");
                string asset_path = string.Format("{0}/{1}.unity", Application.dataPath, scene_name);
                bool successed = EditorSceneManager.SaveScene(scene, asset_path);
                AssetDatabase.Refresh();
                if(!successed)
                {
                    Debug.Log($"{scene_name} {FieldLNGConfig.scene_save_fail_str}");
                }
                else
                {
                    //Debug.Log(scene.path);
                    sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(asset_path.Replace(Application.dataPath,"Assets"));
                    Debug.Log(sceneAsset.name);
                }
            }
            else
            {
                //todo.. 异常处理
            }
        }

        public override void Release()
        {
           
        }

        public override string UpLoad()
        {
            Zip(scene_name, num_id, scene_path, txt_path, img_dirinfo.FullName);
            return string.Empty;
        }
    }
}

