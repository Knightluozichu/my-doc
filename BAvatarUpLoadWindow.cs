using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ASK_SDK
{
    public class BAvatarUpLoadWindow : SubUpLoadBase
    {
 
        private GameObject obj;
        private string txt_path;
        private string obj_ath;
        private string obj_name;

        public override string Build()
        {
            MakeInfo();
            return AssetUtils.AssetPrefab(obj, outputDir, puffix);
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

            var window = InfoWindow.CurrentWindow as BAvatarInfoWindow;
            //背景图
            MakePicture(window.Bg_picture, "avatar_bg");
            //展示图
            MakePicture(window.Show_picture, "avatar_show");
            //预览图
            MakeRendererPicture(window.Preview_picture, "avatar_preview");

            AssetDatabase.Refresh();
        }

        public override void Init()
        {
            outputDir = Application.persistentDataPath;
            num_id = 1;
            if (AssetWindow.CurrentSubWindow != null)
            {
                obj = AssetWindow.CurrentSubWindow.GetAvatarObj();
                obj_name = obj.name;
                puffix = "avatar";
                txt_path = Path.Combine(outputDir, $"{InfoWindow.CurrentWindow.model.GetData().Data_ID}.txt");
                obj_ath = Path.Combine(outputDir, $"{obj_name}.{puffix}");
                
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
            //制作上传的压缩包
            Zip(obj_name, num_id, obj_ath, txt_path, img_dirinfo.FullName);

            //todo.. 上传
            return string.Empty;
        }


    }
}

