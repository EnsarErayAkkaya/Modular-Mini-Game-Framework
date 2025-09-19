using UnityEngine;

namespace EEA.Menu
{
    public interface ISnappyScrollElement
    {
        RectTransform RectTransform { get; }
        void OnFocus();
        void OnUnfocus();
    }
}