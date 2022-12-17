using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(EnemyController))]
public class EnemyScriptEditor : Editor
{
    AnimBool smallEnemyChecklist;
    AnimBool midEnemyChecklist;
    AnimBool largeEnemyChecklist;

   // EnemyController enemyController;
     
    private void OnEnable()
    {
        smallEnemyChecklist = new AnimBool(false);
        smallEnemyChecklist.valueChanged.AddListener(Repaint);

        midEnemyChecklist = new AnimBool(false);
        midEnemyChecklist.valueChanged.AddListener(Repaint);

        largeEnemyChecklist = new AnimBool(false);
        largeEnemyChecklist.valueChanged.AddListener(Repaint);
    }

    public override void OnInspectorGUI()
    {
        var enemyController = target as EnemyController;
        //float ecAS = enemyController.midEnemyAttackSpeed;       

        //GUILayout.Label("Medium Enemy Stats", EditorStyles.boldLabel);

        //EditorGUILayout.Space();

        smallEnemyChecklist.target = EditorGUILayout.ToggleLeft("Small Enemy Stats", smallEnemyChecklist.target);

        if (EditorGUILayout.BeginFadeGroup(smallEnemyChecklist.faded))
        {
            EditorGUI.indentLevel++;

            enemyController.smallEnemyAttackSpeed = EditorGUILayout.FloatField("Attack Speed", enemyController.smallEnemyAttackSpeed);
            enemyController.smallEnemyDamage = EditorGUILayout.FloatField("Damage", enemyController.smallEnemyDamage);
            enemyController.smallEnemyHealth = EditorGUILayout.FloatField("Health", enemyController.smallEnemyHealth);
            enemyController.smallEnemyHealthMax = EditorGUILayout.FloatField("Max Health", enemyController.smallEnemyHealthMax);
            enemyController.smallEnemySpeed = EditorGUILayout.FloatField("Speed", enemyController.smallEnemySpeed);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        midEnemyChecklist.target = EditorGUILayout.ToggleLeft("Medium Enemy Stats", midEnemyChecklist.target);

        if (EditorGUILayout.BeginFadeGroup(midEnemyChecklist.faded))
        {
            EditorGUI.indentLevel++;

            enemyController.midEnemyAttackSpeed = EditorGUILayout.FloatField("Attack Speed", enemyController.midEnemyAttackSpeed);
            enemyController.midEnemyDamage = EditorGUILayout.FloatField("Damage", enemyController.midEnemyDamage);
            enemyController.midEnemyHealth = EditorGUILayout.FloatField("Health", enemyController.midEnemyHealth);
            enemyController.midEnemySpeed = EditorGUILayout.FloatField("Speed", enemyController.midEnemySpeed);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        largeEnemyChecklist.target = EditorGUILayout.ToggleLeft("Large Enemy Stats", largeEnemyChecklist.target);

        if (EditorGUILayout.BeginFadeGroup(largeEnemyChecklist.faded))
        {
            EditorGUI.indentLevel++;

            enemyController.largeEnemyAttackSpeed = EditorGUILayout.FloatField("Attack Speed", enemyController.largeEnemyAttackSpeed);
            enemyController.largeEnemyDamage = EditorGUILayout.FloatField("Damage", enemyController.largeEnemyDamage);
            enemyController.largeEnemyHealth = EditorGUILayout.FloatField("Health", enemyController.largeEnemyHealth);
            enemyController.largeEnemySpeed = EditorGUILayout.FloatField("Speed", enemyController.largeEnemySpeed);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        //base.OnInspectorGUI();
    }
}
