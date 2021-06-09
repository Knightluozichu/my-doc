using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace ASK_SDK
{
    public class BAvatarAssetWindow : SubAssetWindowBase
    {
        private static int last_instanceid_obj = int.MaxValue;
        private static int current_instanceid_obj;

        private GameObject AvatarObj
        {
            get
            {
                return avatarObj;
            }

            set
            {
                avatarObj = value;

                if (avatarObj != null)
                {
                    current_instanceid_obj = avatarObj.GetInstanceID();
                    if (last_instanceid_obj != current_instanceid_obj)
                    {
                        last_instanceid_obj = current_instanceid_obj;
                        GetAllChangeInfo(avatarObj);
                    }
                }

            }
        }

        private const string icon_asset_avatar_str = "Avatar Icon";

        public override void OnGUI()
        {
            //Debug.Log(nameof(OnGUI));
            using (gs = new GUI.GroupScope(new Rect(0, 100, Screen.width, 600)))
            {
                DrawAvatarHeaderGUI();
                DrawContentGUI();
                DrawMessageInfoGUI();
                UpdateLanguage();
            }
        }

        private void UpdateLanguage()
        {
            if (typeOfList == null || typeOfList.Count < 1) return;
            typeOfList[0].str = FieldLNGConfig.bAvatarAsset_modeCountStr;
            typeOfList[1].str = FieldLNGConfig.bAvatarAsset_boneCountStr;
            typeOfList[2].str = FieldLNGConfig.bAvatarAsset_damBoneCountStr;
            typeOfList[3].str = FieldLNGConfig.bAvatarAsset_damBoneCollisionCountStr;
            typeOfList[4].str = FieldLNGConfig.materialCountStr;
            typeOfList[5].str = FieldLNGConfig.shanderCountStr;
            typeOfList[6].str = FieldLNGConfig.gameobjectCountStr;
            typeOfList[7].str = FieldLNGConfig.texture2dSizeStr;
            typeOfList[8].str = FieldLNGConfig.texture2dCountStr;
            typeOfList[9].str = FieldLNGConfig.particalSystemCountStr;
            typeOfList[10].str = FieldLNGConfig.paticalSystemSizeStr;
            typeOfList[11].str = FieldLNGConfig.audioComponentCountStr;
            typeOfList[12].str = FieldLNGConfig.audioComponentSizeStr;
            typeOfList[13].str = FieldLNGConfig.playVideoComponentCountStr;
            typeOfList[14].str = FieldLNGConfig.playVideoComponentSizeStr;

            typeOfList[0].p = FieldLNGConfig.p_plan;
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
            typeOfList[11].p = FieldLNGConfig.p_num;
            typeOfList[12].p = FieldLNGConfig.p_size;
            typeOfList[13].p = FieldLNGConfig.p_num;
            typeOfList[14].p = FieldLNGConfig.p_size;
        }

        public override void OnInit()
        {

            typeOfList = new Dictionary<int, BShowInfo>()
                {
                    {0, new BShowInfo(){ str = FieldLNGConfig.bAvatarAsset_modeCountStr, num = 0 , p =FieldLNGConfig.p_plan,id= 0} },
                    {1, new BShowInfo(){ str = FieldLNGConfig.bAvatarAsset_boneCountStr, num = 0 , p =FieldLNGConfig.p_num,id= 1} },
                    {2, new BShowInfo(){ str = FieldLNGConfig.bAvatarAsset_damBoneCountStr, num = 0 , p =FieldLNGConfig.p_num,id= 2} },
                    {3, new BShowInfo(){ str = FieldLNGConfig.bAvatarAsset_damBoneCollisionCountStr, num = 0 , p =FieldLNGConfig.p_num,id= 3} },
                    {4, new BShowInfo(){ str = FieldLNGConfig.materialCountStr, num = 0 , p =FieldLNGConfig.p_num,id= 4} },
                    {5, new BShowInfo(){ str = FieldLNGConfig.shanderCountStr, num = 0, p =FieldLNGConfig.p_num ,id= 5} },
                    {6, new BShowInfo(){ str = FieldLNGConfig.gameobjectCountStr, num = 0, p =FieldLNGConfig.p_num ,id= 6} },
                    {7, new BShowInfo(){ str = FieldLNGConfig.texture2dSizeStr, num = 0 , p =FieldLNGConfig.p_size,id= 7} },
                    {8, new BShowInfo(){ str = FieldLNGConfig.texture2dCountStr, num = 0 , p =FieldLNGConfig.p_num,id= 8} },
                    {9, new BShowInfo(){ str = FieldLNGConfig.particalSystemCountStr, num = 0 , p =FieldLNGConfig.p_num,id= 9} },
                    {10, new BShowInfo(){ str = FieldLNGConfig.paticalSystemSizeStr, num = 0 , p =FieldLNGConfig.p_num,id= 10} },
                    {11, new BShowInfo(){ str = FieldLNGConfig.audioComponentCountStr, num = 0 , p =FieldLNGConfig.p_num,id= 11} },
                    {12, new BShowInfo(){ str = FieldLNGConfig.audioComponentSizeStr, num = 0, p =FieldLNGConfig.p_size ,id= 12} },
                    {13, new BShowInfo(){ str = FieldLNGConfig.playVideoComponentCountStr, num = 0, p =FieldLNGConfig.p_num ,id= 13} },
                    {14, new BShowInfo(){ str = FieldLNGConfig.playVideoComponentSizeStr, num = 0 , p =FieldLNGConfig.p_size,id= 14} }
                };

            if(avatarObj != null) 
            {
                GetAllChangeInfo(avatarObj);
            }
            
        }

        public override void OnRelease()
        {
            //typeOfList = null;
            last_instanceid_obj = int.MaxValue;
        }

        private void DrawAvatarHeaderGUI()
        {
            EditorGUI.LabelField(new Rect(350,10, 100, 20), new GUIContent(EditorGUIUtility.IconContent(icon_asset_avatar_str)) { text = FieldLNGConfig.bAvatarAsset_asset_avatar_str }, StyleUtils.GetLine());
            
        }

        private void DrawContentGUI()
        {
            GUI.Label(new Rect(200, 45, 200f, 20f),
           AvatarObj == null ? FieldLNGConfig.bAvatarAsset_maybe : AvatarObj.name ,
           new GUIStyle() { normal = new GUIStyleState() { textColor = AvatarObj == null ? new Color(0.7380255f, 0.01f, 0.8f) : new Color(0.2182896f, 0.6509434f, 0) }, fontSize = 15 , clipping = TextClipping.Clip }
           );

            AvatarObj = (GameObject)EditorGUI.ObjectField(new Rect(400, 45f, 200, 20), AvatarObj, typeof(GameObject), true);

        }

        private void DrawMessageInfoGUI()
        {
            Rect rect = new Rect(200, 85, 300, 20);

            icon_current = greyIcon;

            foreach (var item in typeOfList)
            {
                ShowMsgGUI(rect, item.Value);
                rect.y += 25;
            }
        }

        private void ShowMsgGUI(Rect rect, BShowInfo msg)
        {

            EditorGUI.LabelField(rect, new GUIContent(EditorGUIUtility.IconContent(icon_current)) { text = msg.str }, new GUIStyle() { normal = new GUIStyleState() { textColor = white } });
            rect.x += 360;
            EditorGUI.LabelField(rect, new GUIContent() { text =string.Format("{0}{1}",msg.num.ToString(),msg.p.ToString()) }, new GUIStyle() { normal = new GUIStyleState() { textColor = white } });
            rect.x -= 360;
        }

        private static void GetAllChangeInfo(GameObject avatar)
        {
            //model triangles
            {
                if (typeOfList.TryGetValue(0, out BShowInfo value))
                {
                    MeshFilter[] mfs = ObjectUtils.GetPrefabsType<MeshFilter>(avatar);
                    SkinnedMeshRenderer[] smr = ObjectUtils.GetPrefabsType<SkinnedMeshRenderer>(avatar);
                    var verts = 0;
                    var triangles = 0;

                    if (mfs != null && mfs.Length > 0)
                    {
                        foreach (var item in mfs)
                        {
                            verts += item.sharedMesh.vertexCount;
                            triangles += item.sharedMesh.triangles.Length / 3;
                        }
                    }

                    if(smr != null && smr.Length > 0)
                    {
                        foreach (var item in smr)
                        {
                            verts += item.sharedMesh.vertexCount;
                            triangles += item.sharedMesh.triangles.Length / 3;
                        }
                    }
                    value.num = triangles;
                    typeOfList[0] = value;
                }
            }


            //bones
            {
                if (typeOfList.TryGetValue(1, out BShowInfo value))
                {
                    SkinnedMeshRenderer[] mfs = ObjectUtils.GetPrefabsType<SkinnedMeshRenderer>(avatar);

                    var len = 0;
 
                    if (mfs != null && mfs.Length > 0)
                    {
                        foreach (var item in mfs)
                        {
                            len += item.bones.Length;
                        }
                    }

                    value.num = len;
                    typeOfList[1] = value;
                }
            }

            //dynamic bones
            //dynamic collider


            //maiterials
            {
                if(typeOfList.TryGetValue(4, out BShowInfo value))
                {
                    MeshRenderer[] mrs = ObjectUtils.GetPrefabsType<MeshRenderer>(avatar);

                    ParticleSystem[] pars = ObjectUtils.GetPrefabsType<ParticleSystem>(avatar);

                    SkinnedMeshRenderer[] skrs = ObjectUtils.GetPrefabsType<SkinnedMeshRenderer>(avatar);

                    ObjectUtils.GetAllMaterials(mrs, pars, skrs);

                    value.num = ObjectUtils.Materials.Count;

                    typeOfList[4] = value;
                }
            }

            //shader
            {
                if (typeOfList.TryGetValue(5, out BShowInfo value))
                {
                    
                    value.num = ObjectUtils.Shaders.Count;
                    typeOfList[3] = value;
                }
            }

            //gameobject
            {
                if (typeOfList.TryGetValue(6, out BShowInfo value))
                {
                    var gobj = ObjectUtils.GetPrefabsType<Transform>(avatar);
                    value.num = gobj.Length;
                    typeOfList[6] = value;
                }
            }

            //texture
            {
                if (typeOfList.TryGetValue(8, out BShowInfo value))
                {
                    var gobjs = ObjectUtils.GetPrefabsGameObject(avatar);
                    ObjectUtils.GetAlltextures(gobjs);
                    value.num = ObjectUtils.Textures.Count;
                    typeOfList[8] = value;
                }
            }

            //texture size
            {
                long len = 0;

                if (typeOfList.TryGetValue(7, out BShowInfo value))
                {
                    foreach (var item in ObjectUtils.Textures)
                    {
                        string filePath = AssetDatabase.GetAssetPath(item);

                        string fullPath = string.Format("{0}{1}",Application.dataPath.Replace("Assets", ""), filePath);

                        if(!System.IO.File.Exists(fullPath))
                        {
                            continue;
                        }

                        System.IO.FileInfo finfo = new System.IO.FileInfo(fullPath);

                        len += finfo.Length / 1024;
                    }

                    value.num = (int)len;
                    typeOfList[7] = value;
                }
            }

            //par s
            {
                if (typeOfList.TryGetValue(9, out BShowInfo value))
                {
                    var ps = ObjectUtils.GetPrefabsType<ParticleSystem>(avatar);
                    value.num = ps.Length;
                    typeOfList[9] = value;
                }
            }

            //par count
            {
                if (typeOfList.TryGetValue(10, out BShowInfo value))
                {
                    var ps = ObjectUtils.GetPrefabsType<ParticleSystem>(avatar);
                    int len = 0;
                    foreach (var item in ps)
                    {
                        len += item.main.maxParticles;
                    }

                    value.num = len;
                    typeOfList[10] = value;
                }
            }

            //video source
            //video clip
            {
                int len = 0;
                if (typeOfList.TryGetValue(11, out BShowInfo value))
                {
                    var ps = ObjectUtils.GetPrefabsType<AudioSource>(avatar);

                    value.num = ps.Length;
                    typeOfList[11] = value;

                    List<AudioClip> audios = new List<AudioClip>();

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

                if (typeOfList.TryGetValue(12, out BShowInfo value1))
                {
                    value1.num = len / 1024;
                    typeOfList[12] = value1;
                }
            }

            //audio source
            //audio clip
            {
                int len = 0;

                if (typeOfList.TryGetValue(13, out BShowInfo value))
                {
                    var ps = ObjectUtils.GetPrefabsType<VideoPlayer>(avatar);

                    value.num = ps.Length;
                    typeOfList[13] = value;

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

                if (typeOfList.TryGetValue(14, out BShowInfo value1))
                {
                    value1.num = len / 1024;
                    typeOfList[14] = value1;
                }
            }
        }
    }

}

