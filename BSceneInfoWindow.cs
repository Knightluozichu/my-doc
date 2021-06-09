using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ASK_SDK
{
    public class BSceneInfoWindow : SubInfoWindowBase
    {
        private string map_preview_path = "Img/scene_preview.png";
        private string thumbnail_path = "Img/scene_thumbnail.png";

        private string tags = string.Empty;
        private  Texture2D thumbnail = default(Texture2D);
        public Texture2D ThumbnailT2d => thumbnail;



        private Texture2D preview2d;
        public Texture2D Preview2d=> preview2d;

        public override void OnGUI()
        {
            if (aEvent == null)
            {
                aEvent = Event.current;
            }

            DarwUIGUI();

        }

        private void DarwUIGUI()
        {
           
            //缩略图
            GetThumbnailPicture_UI(new Rect(9, 10, 180, 180), IconUtils.squid, FieldLNGConfig.thumbnailStr, "LightmapEditorSelectedHighlight", FieldLNGConfig.picture_drag_this_str+ "\n180*180", true,ref thumbnail);

            //预览图
            GetThumbnailPicture_UI(new Rect(207, 10, 180, 180), IconUtils.skewers, FieldLNGConfig.preview_picture_str, "LightmapEditorSelectedHighlight", FieldLNGConfig.scene_preview_str + "\n180*180", false, ref preview2d);

            //地图简介
            Map_introduction_UI(new Rect(5, 265, 180, 20), FieldLNGConfig.map_introduction_str, IconUtils.roasted_leeks, ref ((SceneModel)model).sceneData.introduction);

            //更新日志
            Map_introduction_UI(new Rect(207, 265, 180, 20), FieldLNGConfig.log_update_str, IconUtils.roasted_gluten, ref ((SceneModel)model).sceneData.update_log);

            //视频介绍网址
            Label_Text_UI(new Rect(5, 460, 130, 20), FieldLNGConfig.video_str, IconUtils.roast_lamb, ref ((SceneModel)model).sceneData.url);

            //房间最大人数
            ((SceneModel)model).sceneData.map_capacity = Map_max_capacity_UI(new Rect(5, 490, 130, 20), FieldLNGConfig.max_room_str, IconUtils.roasted_shiitake_mushrooms, ((SceneModel)model).sceneData.map_capacity);

            //name
            Label_Text_UI(new Rect(5, 520, 130, 20), FieldLNGConfig.map_name_str, IconUtils.squid_roll, ref ((SceneModel)model).sceneData.map_name);

            //地图标签
            Label_Text_UI(new Rect(5, 550, 130, 20), FieldLNGConfig.map_tags_str, IconUtils.roasted_lettuce, ref tags);
            ((SceneModel)model).sceneData.tag_keys =  GetTagKeys(tags);

            
            //渲染
            Label_Pop_UI(new Rect(5, 580, 130, 20), FieldLNGConfig.render_pip_str, IconUtils.roasted_leeks,ref ((SceneModel)model).sceneData.rendering_Pipeline);
        }

        private void GetThumbnailPicture_UI(Rect rect, Texture2D imagestr, string text, string guistyle, string textImagestr,bool isChooise, ref Texture2D picture)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, 40), new GUIContent() { image = imagestr, text = text }, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });
            GUI.Box(new Rect(rect.x, rect.y+40, rect.width + 2, rect.height + 2), new GUIContent() { text = textImagestr }, new GUIStyle(guistyle) { alignment = TextAnchor.MiddleCenter });
            if(isChooise)
            {
                GetDragTexture(new Rect(rect.x + 1, 1 + 40 + rect.y , rect.width, rect.height), ref picture);
            }
            if (picture)
            {
                EditorGUI.DrawPreviewTexture(new Rect(rect.x +1, 1 + 40 + rect.y , rect.width, rect.height), picture);
            }
        }

        private void GetTexture2dPewview_UI()
        {
            Camera camera = null;

            if(Camera.main == null)
            {
                camera = new GameObject("Main Camera").AddComponent<Camera>();
                camera.tag = "MainCamera";
            }
            else
            {
                camera = Camera.main;
            }

            RenderTexture rt = new RenderTexture(180, 180, 24);
            camera.targetTexture = rt;

            camera.Render();
            UnityEngine.RenderTexture.active = camera.targetTexture;
            preview2d = new Texture2D(UnityEngine.RenderTexture.active.width, UnityEngine.RenderTexture.active.height);
            preview2d.ReadPixels(new Rect(0, 0, preview2d.width, preview2d.height), 0, 0);
            preview2d.Apply();

        }

        private void Map_introduction_UI(Rect rect, string str,Texture2D img,ref string area) 
        {
            EditorGUI.LabelField(rect, new GUIContent() {text = str, image = img }, StyleUtils.GetStyle(EditorStyles.label.name));
            area =  EditorGUI.TextArea(new Rect(rect.x, rect.y + rect.height, rect.width, rect.height + 120), area, EditorStyles.textArea);
        }

        private void Label_Text_UI(Rect rect,string str, Texture2D img,ref string field )
        {
            EditorGUI.LabelField(rect, new GUIContent() { text = str, image = img }, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft });
            field = EditorGUI.TextField(new Rect(rect.x + rect.width + 10, rect.y , rect.width + 120, rect.height), field, EditorStyles.textField);
        }

        private void Label_Pop_UI(Rect rect, string str, Texture2D img, ref Rendering_Pipeline field)
        {
            EditorGUI.LabelField(rect, new GUIContent() { text = str, image = img }, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft });
            field = (Rendering_Pipeline)EditorGUI.EnumPopup(new Rect(rect.x + rect.width + 10, rect.y, rect.width + 120, rect.height), field, EditorStyles.popup);
        }

        private int Map_max_capacity_UI(Rect rect, string str, Texture2D img, int num)
        {
            EditorGUI.LabelField(rect, new GUIContent() { text = str, image = img }, new GUIStyle(EditorStyles.label) {  alignment = TextAnchor.MiddleLeft});
            num = EditorGUI.IntSlider(new Rect(rect.x + rect.width + 10, rect.y , rect.width+120, rect.height), num, 1, 40);
            return num;
        }

        public override void OnInit()
        {
            if(model == null)
            {
                model = new SceneModel(new SceneData()
                {
                    introduction = string.Empty,
                    map_capacity = 1,
                    map_name = string.Empty,
                    map_preview_path = map_preview_path,
                    thumbnail_path = thumbnail_path,
                    tag_keys = new List<string>(),
                    update_log = string.Empty,
                    url = string.Empty,
                    rendering_Pipeline = Rendering_Pipeline.Bulit_in
                });
            }

            GetTexture2dPewview_UI();

        }

        public override void OnRelease()
        {
            
        }
    }
}

