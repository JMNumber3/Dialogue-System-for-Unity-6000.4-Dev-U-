
using UnityEditor;
using UnityEngine;
using DialogueSystem;
using UnityEditor.Rendering;

namespace DialogueSystem
{
    [CustomEditor(typeof(DialogueTemplateInk))]
    public class DialogueTemplateInkEditor : Editor
    {
        private bool showSimpleDialogue = true;
        private bool showChoiceDialogue = true;
        private bool showTypewriter = true;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            showSimpleDialogue = EditorGUILayout.BeginFoldoutHeaderGroup(showSimpleDialogue, "Simple Dialogue");
            if (showSimpleDialogue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useSimpleDialogue"));
                bool useSimple = serializedObject.FindProperty("useSimpleDialogue").boolValue;
                EditorGUI.BeginDisabledGroup(!useSimple);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("simpleTextBox"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("simpleText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("continueButton"));
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            showChoiceDialogue = EditorGUILayout.BeginFoldoutHeaderGroup(showChoiceDialogue, "Choice Dialogue");
            if (showChoiceDialogue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useChoiceDialogue"));
                bool useChoice = serializedObject.FindProperty("useChoiceDialogue").boolValue;
                EditorGUI.BeginDisabledGroup(!useChoice);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceTextBox"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("buttonPrefab"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("choiceText"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("buttonContainer"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("choicesContinueButton"));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            showTypewriter = EditorGUILayout.BeginFoldoutHeaderGroup(showTypewriter, "Typewriter");
            if (showTypewriter)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("characteresPerSecond"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("interpunctuationDelay"));
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}