using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace ASK_SDK
{
    public abstract class SubAssetWindowBase :BaseWindow, IBaseWindow
    {
        protected Color red = Color.red;
        protected Color white = Color.white;
        protected Color yellow = Color.yellow;
        protected Color green = Color.green;

        protected const string redIcon = "sv_icon_dot6_pix16_gizmo";
        protected const string yellowIcon = "sv_icon_dot4_pix16_gizmo";
        protected const string greenIcon = "sv_icon_dot3_pix16_gizmo";
        protected const string greyIcon = "sv_icon_dot0_pix16_gizmo";
        protected string icon_current = string.Empty;

        //protected GUI.GroupScope gs;
        protected static Dictionary<int, BShowInfo> typeOfList;

        protected Scene currentScene;
        public Scene GetCurrentScene() => currentScene;

        protected GameObject avatarObj;
        public GameObject GetAvatarObj()=> avatarObj;

        public abstract void OnGUI();
        public abstract void OnInit();
        public abstract void OnRelease();

        public static void GetInfo(Dictionary<int, BShowInfo> keyValues, int id, System.Func<int> func)
        {
            keyValues.TryGetValue(id, out BShowInfo value);
            value.num = func();
            keyValues[id] = value;
        }
    }
}

