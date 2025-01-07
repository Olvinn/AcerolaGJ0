using Controllers;
using UnityEditor;
using UnityEngine;

namespace Stages.Editor
{
    [CustomEditor(typeof(StageController))]
    public class StageControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var myTarget = (StageController)target;
            GUILayout.Space(10);
            GUILayout.Label("Current Stage:");
            GUILayout.Label(myTarget.GetCurrent());
            GUILayout.Label("Stack:");
            GUILayout.Label(myTarget.GetStack());
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}
