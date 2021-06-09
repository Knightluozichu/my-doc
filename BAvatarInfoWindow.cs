using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ASK_SDK
{
    public class BAvatarInfoWindow : SubInfoWindowBase
    {
        //private AvatarModel model;

        private const string avatar_bg_path = "Img/avatar_bg.png";
        private const string avatar_show_path = "Img/avatar_show.png";
        private const string avatar_preview_path = "Img/avatar_preview.png";

        private Camera camera = null;
        private Texture2D preview_picture;
        private Texture2D bg_picture;
        private Texture2D show_picture;

        public Texture2D Preview_picture => preview_picture;
        public Texture2D Bg_picture => bg_picture;
        public Texture2D Show_picture => show_picture;

        private string tags = string.Empty;
        

        public override void OnGUI()
        {
            if(aEvent == null)
            {
                aEvent = Event.current;
            }
            if (AssetWindow.CurrentSubWindow == null || AssetWindow.CurrentSubWindow.GetAvatarObj() == null)
            {
                EditorGUI.LabelField(new Rect(0, 10, 397.4f, 600f), FieldLNGConfig.please_chooise_asset_str, new GUIStyle() { alignment = TextAnchor.MiddleCenter });
                return;
            }


            //封面图
            EditorGUI.LabelField(new Rect(5, 10, 180, 18), FieldLNGConfig.preview_picture_str, StyleUtils.GetStyle("Tab first"));

            if(camera == null)
            {
                GetTexture2dPewview_UI();
            }

            if(preview_picture != null)
            {
                EditorGUI.DrawPreviewTexture(new Rect(5, 25 + 10, 180f, 100f), preview_picture);
            }

            //背景图
            EditorGUI.LabelField(new Rect(215f, 10, 180, 18), new GUIContent(EditorGUIUtility.IconContent("PreTextureArrayLastSlice")) { text = FieldLNGConfig.bg_picture_str }, StyleUtils.GetStyle("Tab first"));
            GUI.Box(new Rect(215f, 25+10, 180f, 100f), FieldLNGConfig.picture_drag_this_str + "\n180*100");
            GetDragTexture(new Rect(215f, 25 + 10, 180f, 100f), ref bg_picture);
            if (bg_picture)
            {
                EditorGUI.DrawPreviewTexture(new Rect(215f, 25 + 10, 180f, 100f), bg_picture);
            }

            //name
            ((AvatarModel)model).avatarData.avatar_name = EditorGUI.TextField(new Rect(5, 140f + 10, 390, 30), new GUIContent(EditorGUIUtility.IconContent("d_CollabEdit Icon")) { text = FieldLNGConfig.avatar_name_str }, ((AvatarModel)model).avatarData.avatar_name, StyleUtils.GetStyle("AC Button"));

            //文本介绍
            EditorGUI.LabelField(new Rect(5, 175f + 10, 280, 20), new GUIContent(EditorGUIUtility.IconContent("d_editicon.sml")) { text = FieldLNGConfig.avatar_info_str });
            ((AvatarModel)model).avatarData.avatar_introduction = EditorGUI.TextArea(new Rect(5, 195f + 10, 280, 160), ((AvatarModel)model).avatarData.avatar_introduction, EditorStyles.textArea);

            //展示图
            EditorGUI.LabelField(new Rect(300, 175f + 10, 90, 20), new GUIContent(EditorGUIUtility.IconContent("PreTextureArrayFirstSlice")) { text = FieldLNGConfig.plan_picture_str }, EditorStyles.label);
            GUI.Box(new Rect(300, 195f + 10, 90, 270), FieldLNGConfig.picture_drag_this_str +"\n90*270");
            GetDragTexture(new Rect(300, 195f + 10, 90, 270), ref show_picture);
            if (show_picture)
            {
                EditorGUI.DrawPreviewTexture(new Rect(300, 195f + 10, 90, 270), show_picture);
            }

            //更新日志
            EditorGUI.LabelField(new Rect(5, 365 + 10, 280, 20), new GUIContent(EditorGUIUtility.IconContent("Text Icon")) { text = FieldLNGConfig.log_update_str }, EditorStyles.label);
            ((AvatarModel)model).avatarData.update_log = EditorGUI.TextArea(new Rect(5, 387 + 10, 280, 80), ((AvatarModel)model).avatarData.update_log, EditorStyles.textArea);

            //Tag
            EditorGUI.LabelField(new Rect(5, 478f + 10, 40, 20), new GUIContent(EditorGUIUtility.IconContent("Animation.AddKeyframe")) { text = "Tag" }, EditorStyles.boldLabel);
            EditorGUI.LabelField(new Rect(45, 478f + 10, 350, 20), new GUIContent() { text = FieldLNGConfig.tags_tip_str }, new GUIStyle(EditorStyles.miniLabel) { fontSize = 10, alignment = TextAnchor.LowerLeft, fontStyle = FontStyle.Italic});
            tags = EditorGUI.TextField(new Rect(5, 500 + 10, 390, 30), tags, new GUIStyle(EditorStyles.textField)
            {
                stretchHeight = true,
                stretchWidth = true,
                border = new RectOffset(11, 11, 11, 15),
                margin = new RectOffset(2, 2, 0, 0),
                padding = new RectOffset(2, 2, 0, 0),
                overflow = new RectOffset(7, 7, 6, 9),

                // 进行文本方面的设置
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                richText = true,
            });
            ((AvatarModel)model).avatarData.tag_keys = GetTagKeys(tags);

            //状态
            EditorGUI.LabelField(new Rect(5, 550 + 10, 40, 20), FieldLNGConfig.status_str, EditorStyles.boldLabel);

            using (gs = new GUI.GroupScope(new Rect(55, 540 + 10, 340, 40)))
            {
                GUI.Box(new Rect(0, 0 + 10, 340, 35), string.Empty, EditorStyles.helpBox);
                ((AvatarModel)model).avatarData.avatar_property = GUI.Toolbar(new Rect(0, 0 + 10, 340, 35), ((AvatarModel)model).avatarData.avatar_property, new string[] { FieldLNGConfig.public_str, FieldLNGConfig.private_str });
            }
        }

        public override void OnInit()
        {
            if (model == null)
            {
                model = new AvatarModel(new AvatarData()
                {
                    avatar_bg_path = avatar_bg_path,
                    avatar_show_path = avatar_show_path,
                    avatar_preview_path = avatar_preview_path,
                    avatar_introduction = string.Empty,
                    avatar_name = string.Empty,
                    avatar_property = 1,
                    tag_keys = new List<string>(),
                    update_log = string.Empty,
                });
            }
        }

        public override void OnRelease()
        {
            
        }
        private void GetTexture2dPewview_UI()
        {
            if (Camera.main == null)
            {
                camera = new GameObject("Main Camera").AddComponent<Camera>();
                camera.tag = "MainCamera";
            }
            else
            {
                camera = Camera.main;
            }
            RenderTexture rt = new RenderTexture(180, 100, 24);
            camera.targetTexture = rt;

            camera.Render();
            UnityEngine.RenderTexture.active = camera.targetTexture;
            preview_picture = new Texture2D(UnityEngine.RenderTexture.active.width, UnityEngine.RenderTexture.active.height);
            preview_picture.ReadPixels(new Rect(0, 0, preview_picture.width, preview_picture.height), 0, 0);
            preview_picture.Apply();

        }

    }

}
