using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OvyKode
{
    [CustomEditor(typeof(SetItemID))]
    public class GenerateItemID : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SetItemID setItemID = (SetItemID)target;

            if (GUILayout.Button("Change Object ID"))
            {
                setItemID.ChangeName();
            }
        }
    }
}