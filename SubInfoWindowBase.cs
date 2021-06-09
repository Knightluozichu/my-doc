using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace ASK_SDK
{
    public abstract class SubInfoWindowBase : BaseWindow, IBaseWindow
    {
        protected Event aEvent;

        public abstract void OnGUI();
        public abstract void OnInit();
        public abstract void OnRelease();

        protected void GetDragTexture(Rect rect, ref Texture2D texture2D)
        {
            switch (aEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!rect.Contains(aEvent.mousePosition))
                    {
                        break;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (aEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        for (int i = 0; i < DragAndDrop.objectReferences.Length; ++i)
                        {
                            texture2D = DragAndDrop.objectReferences[i] as Texture2D;

                            if (texture2D == null)
                            {
                                break;
                            }
                        }
                    }

                    Event.current.Use();
                    break;
                default:
                    break;
            }
        }

        protected List<string> GetTagKeys(string tags)
        {
            return tags.Split(',', '，').ToList();
        }
    }
}

