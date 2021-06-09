using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.Profiling;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.Profiling;
using UnityEngine.Video;
using System.IO;

namespace ASK_SDK
{
    public class BSceneAssetWindow : SubAssetWindowBase
    {
        private string icon = string.Empty;
        private const string icon_scene_avatar_str = "d_SceneAsset Icon";
        private SceneAsset sceneAsset;
        

        public override void OnGUI()
        {
            using (gs = new GUI.GroupScope(new Rect(0, 100, Screen.width, 600)))
            {
                DrawAvatarHeaderGUI();
                DrawContentGUI();
                ShowMsgGUI();
                GetAllChangeInfo();
                UpdateLanguage();
            }
            
        }
        private void UpdateLanguage()
        {
            if (typeOfList == null || typeOfList.Count < 1) return;
            typeOfList[0].str = FieldLNGConfig.astralTriggerCountStr;
            typeOfList[1].str = FieldLNGConfig.mirrorCountStr;
            typeOfList[2].str = FieldLNGConfig.changingRoomCountStr;
            typeOfList[3].str = FieldLNGConfig.materialCountStr;
            typeOfList[4].str = FieldLNGConfig.shanderCountStr;
            typeOfList[5].str = FieldLNGConfig.gameobjectCountStr;
            typeOfList[6].str = FieldLNGConfig.texture2dCountStr;
            typeOfList[7].str = FieldLNGConfig.texture2dSizeStr;
            typeOfList[8].str = FieldLNGConfig.particalSystemCountStr;
            typeOfList[9].str = FieldLNGConfig.paticalSystemSizeStr;
            typeOfList[10].str = FieldLNGConfig.audioComponentCountStr;
            typeOfList[11].str = FieldLNGConfig.audioComponentSizeStr;
            typeOfList[12].str = FieldLNGConfig.playVideoComponentCountStr;
            typeOfList[13].str = FieldLNGConfig.playVideoComponentSizeStr;

            typeOfList[0].p = FieldLNGConfig.p_num;
            typeOfList[1].p = FieldLNGConfig.p_num;
            typeOfList[2].p = FieldLNGConfig.p_num;
            typeOfList[3].p = FieldLNGConfig.p_num;
            typeOfList[4].p = FieldLNGConfig.p_num;
            typeOfList[5].p = FieldLNGConfig.p_num;
            typeOfList[6].p = FieldLNGConfig.p_num;
            typeOfList[7].p = FieldLNGConfig.p_size;
            typeOfList[8].p = FieldLNGConfig.p_num;
            typeOfList[9].p = FieldLNGConfig.p_num;
            typeOfList[10].p = FieldLNGConfig.p_num;
            typeOfList[11].p = FieldLNGConfig.p_size;
            typeOfList[12].p = FieldLNGConfig.p_num;
            typeOfList[13].p = FieldLNGConfig.p_size;
        }

        public override void OnInit() 
        {
            typeOfList = new Dictionary<int, BShowInfo>()
                {
                    {0, new BShowInfo(){ str = FieldLNGConfig.astralTriggerCountStr, num = 0, p =FieldLNGConfig.p_num , id = 0} },
                    {1, new BShowInfo(){ str = FieldLNGConfig.mirrorCountStr, num = 0, p =FieldLNGConfig.p_num , id = 1} },
                    {2, new BShowInfo(){ str = FieldLNGConfig.changingRoomCountStr, num = 0, p =FieldLNGConfig.p_num , id = 2} },
                    {3, new BShowInfo(){ str = FieldLNGConfig.materialCountStr, num = 0, p =FieldLNGConfig.p_num , id = 3} },
                    {4, new BShowInfo(){ str = FieldLNGConfig.shanderCountStr, num = 0, p =FieldLNGConfig.p_num , id = 4} },
                    {5, new BShowInfo(){ str = FieldLNGConfig.gameobjectCountStr, num = 0, p =FieldLNGConfig.p_num , id = 5} },
                    {6, new BShowInfo(){ str = FieldLNGConfig.texture2dSizeStr, num = 0, p =FieldLNGConfig.p_size, id = 7 } },
                    {7, new BShowInfo(){ str = FieldLNGConfig.texture2dCountStr, num = 0, p =FieldLNGConfig.p_num , id = 6} },
                    {8, new BShowInfo(){ str = FieldLNGConfig.particalSystemCountStr, num = 0, p =FieldLNGConfig.p_num , id = 8} },
                    {9, new BShowInfo(){ str = FieldLNGConfig.paticalSystemSizeStr, num = 0, p =FieldLNGConfig.p_num , id = 9} },
                    {10, new BShowInfo(){ str = FieldLNGConfig.audioComponentSizeStr, num = 0, p =FieldLNGConfig.p_size , id =11} },
                    {11, new BShowInfo(){ str = FieldLNGConfig.audioComponentCountStr, num = 0, p =FieldLNGConfig.p_num , id = 10} },
                    {12, new BShowInfo(){ str = FieldLNGConfig.playVideoComponentCountStr, num = 0, p =FieldLNGConfig.p_num , id = 12} },
                    {13, new BShowInfo(){ str = FieldLNGConfig.playVideoComponentSizeStr, num = 0, p =FieldLNGConfig.p_size , id = 13} }
                };

            EditorSceneManager.activeSceneChangedInEditMode += EditorSceneManager_activeSceneChangedInEditMode;

            currentScene = EditorSceneManager.GetActiveScene();

            GetAllChangeInfo();

            if (currentScene.path != null)
            {
                sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentScene.path);
            }
        }

        private void EditorSceneManager_activeSceneChangedInEditMode(Scene arg0, Scene arg1)
        {
            GetAllChangeInfo();
            currentScene = arg1;
            sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(arg1.path);
        }

        public override void OnRelease()
        {

            EditorSceneManager.activeSceneChangedInEditMode -= EditorSceneManager_activeSceneChangedInEditMode;
        }

        private void DrawAvatarHeaderGUI()
        {
            EditorGUI.LabelField(new Rect(350, 10, 100, 20), new GUIContent(EditorGUIUtility.IconContent(icon_scene_avatar_str)) { text = FieldLNGConfig.scene_avatar_str }, StyleUtils.GetLine());

        }

        private void DrawContentGUI()
        {
            GUI.Label(new Rect(294.23f, 45, 100f, 20f),
           currentScene == default(Scene) || string.IsNullOrWhiteSpace(currentScene.name) ? FieldLNGConfig.bAvatarAsset_maybe : currentScene.name, new GUIStyle("AC Button"));
        }

        private void ShowMsgGUI()
        {
            Rect rect = new Rect(200, 85, 300, 20);
            icon = greyIcon;

            GUI.Box(rect, "");

            foreach (var item in typeOfList)
            {
                DrawItemGUI(rect, item.Value);
                rect.y += 25;
            }
        }

        private void DrawItemGUI(Rect rect, BShowInfo msg)
        {
            EditorGUI.LabelField(rect, new GUIContent(EditorGUIUtility.IconContent(icon)) { text = msg.str }, new GUIStyle() { normal = new GUIStyleState() { textColor = white } });
            rect.x += 360;
            EditorGUI.LabelField(rect, new GUIContent() { text = string.Format("{0}{1}", msg.num.ToString(),msg.p)}, new GUIStyle() { normal = new GUIStyleState() { textColor = white } });
            rect.x -= 360;
        }

        private static void GetAllChangeInfo()
        {
            ///全部信息统计

            //materials count
            {
                if (typeOfList.TryGetValue(3, out BShowInfo value))
                {
                    MeshRenderer[] mrs = ObjectUtils.GetSceneType<MeshRenderer>();

                    ParticleSystem[] pars = ObjectUtils.GetSceneType<ParticleSystem>();

                    SkinnedMeshRenderer[] skrs = ObjectUtils.GetSceneType<SkinnedMeshRenderer>();

                    ObjectUtils.GetAllMaterials(mrs, pars, skrs);

                    value.num = ObjectUtils.Materials.Count;

                    typeOfList[3] = value;
                }
            }


            //shader count
            {
                if (typeOfList.TryGetValue(4, out BShowInfo value))
                {
                    value.num = ObjectUtils.Shaders.Count;

                    typeOfList[4] = value;
                }
            }

            //gameobject num
            {
                if(typeOfList.TryGetValue(5, out BShowInfo value))
                {
                    value.num = ObjectUtils.GetSceneType<GameObject>().Length;

                    typeOfList[5] = value;
                }
            }

            //texture2d
            {
                if(typeOfList.TryGetValue(6, out BShowInfo value))
                {
                    ObjectUtils.GetAlltextures(ObjectUtils.GetSceneType<GameObject>());
                    value.num = ObjectUtils.Textures.Count;
                    typeOfList[6] = value;
                }
            }

            {
                if((typeOfList.TryGetValue(7, out BShowInfo value)))
                {
                    value.num = (int)(Texture.currentTextureMemory / 1024);
                    typeOfList[7] = value;
                }
            }

            //particalSystem
            {
                if (typeOfList.TryGetValue(8, out BShowInfo value))
                {
                    var ps =  ObjectUtils.GetSceneType<ParticleSystem>();
                    value.num = ps.Length;
                    typeOfList[8] = value;
                }
            }

            //patical System s
            {
                if (typeOfList.TryGetValue(9, out BShowInfo value))
                {
                    var ps = ObjectUtils.GetSceneType<ParticleSystem>();
                    int len = 0;
                    foreach (var item in ps)
                    {
                        len += item.main.maxParticles;
                    }

                    value.num = len;
                    typeOfList[9] = value;
                }
            }


            //audio
            {
                int len = 0;
                if (typeOfList.TryGetValue(10, out BShowInfo value))
                {
                    var ps = ObjectUtils.GetSceneType<AudioSource>();

                    value.num = ps.Length;
                    typeOfList[10] = value;

                    List<AudioClip> audios = new List<AudioClip>();

                    

                    foreach (var item in ps)
                    {
                        if(item.clip != null && !audios.Contains(item.clip))
                        {
                            audios.Add(item.clip);

                           var bytes =  File.ReadAllBytes(Application.dataPath.Replace("Assets","") + AssetDatabase.GetAssetPath(item.clip));
                            len += bytes.Length;
                        }
                    }
                    
                }

                if (typeOfList.TryGetValue(11, out BShowInfo value1))
                {
                    value1.num = len / 1024;
                    typeOfList[11] = value1;
                }
            }

            //video
            {
                int len = 0;

                if (typeOfList.TryGetValue(12, out BShowInfo value))
                {
                    var ps = ObjectUtils.GetSceneType<VideoPlayer>();

                    value.num = ps.Length;
                    typeOfList[12] = value;

                    List<VideoClip> audios = new List<VideoClip>();

                    

                    foreach (var item in ps)
                    {
                        if (item.clip != null && !audios.Contains(item.clip))
                        {
                            audios.Add(item.clip);

                            var bytes = File.ReadAllBytes(Application.dataPath.Replace("Assets", "") + AssetDatabase.GetAssetPath(item.clip));
                            len += bytes.Length;
                        }
                    }                    
                }

                if (typeOfList.TryGetValue(13, out BShowInfo value1))
                {
                    value1.num = len / 1024;
                    typeOfList[13] = value1;
                }
            }
        }
    }
}

