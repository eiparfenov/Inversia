using UnityEngine;
using UnityEditor;

namespace Editors
{
    [CustomEditor(typeof(ShadowCastingUnion))]
    public class ShadowCastingUnionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create"))
                ((ShadowCastingUnion)target).Create();
        }
    }
}