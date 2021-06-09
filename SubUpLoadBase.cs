using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ASK_SDK
{
    public abstract class SubUpLoadBase : IUpLoad
    {
        protected int num_id = int.MinValue;
        private const string img_path_name = "Img";
        protected string outputDir = string.Empty;
        protected string puffix = string.Empty;
        protected DirectoryInfo img_dirinfo;

        public abstract string Build();
        public abstract void Init();
        public abstract void Release();
        public abstract string UpLoad();

        public virtual void MakeInfo()
        {
            string _img_path_dir = Path.Combine(outputDir, img_path_name);

            if (System.IO.Directory.Exists(_img_path_dir))
            {
                try
                {
                    img_dirinfo = new System.IO.DirectoryInfo(_img_path_dir);
                    img_dirinfo.Attributes = img_dirinfo.Attributes & ~FileAttributes.ReadOnly;
                    img_dirinfo.Delete(true);
                }
                catch (System.Exception ex)
                {
                    Debug.Log($"message is :{ex.Message.ToString()}");
                }
            }

            img_dirinfo = Directory.CreateDirectory(_img_path_dir);


        }

        protected void MakePicture(Texture2D texture2d,string name)
        {
            if (texture2d != null)
            {
                string copy_path = string.Format("{0}/{1}.png", img_dirinfo.FullName, name);

                string srcp_ath = string.Format("{0}/{1}", Application.dataPath.Replace("Assets", ""), AssetDatabase.GetAssetPath(texture2d));

                File.Copy(srcp_ath, copy_path, true);
            }
        }

        protected void MakeRendererPicture(Texture2D texture2d, string name)
        {
            if (texture2d != null)
            {
                File.WriteAllBytes(string.Format("{0}\\{1}.png", img_dirinfo.FullName, name), texture2d.EncodeToPNG());
            }
        }

        protected void Zip(string name,int num,params string[] path)
        {
            string[] zipFilePaths = path;
            string zipOutputPath = Path.Combine(Application.persistentDataPath, string.Format("{0}_{1}.zip", num.ToString(), name));
            if (File.Exists(zipOutputPath))
            {
                FileUtil.DeleteFileOrDirectory(zipOutputPath);
            }
            AssetDatabase.Refresh();
            ZipWrapper.Zip(zipFilePaths, zipOutputPath);

        }
    }


}

