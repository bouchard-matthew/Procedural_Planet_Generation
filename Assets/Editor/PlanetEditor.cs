using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet Planet;
    Editor ShapeEditor;
    Editor ColorEditor;
    public override void OnInspectorGUI()
    {
        using var check = new EditorGUI.ChangeCheckScope();
        using (check) {
            base.OnInspectorGUI();
            if (check.changed)
            {
                Planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Generate Planet") || !Planet.hasBeenInitialized)
        {
            Planet.GeneratePlanet();
            Planet.hasBeenInitialized = true;
        }

        DrawSettingsEditor(Planet.ShapeSettings, Planet.UpdatePlanetShape, ref Planet.IsShapeSettingsFoldoutOpen, ref ShapeEditor);
        DrawSettingsEditor(Planet.ColorSettings, Planet.UpdatePlanetColor, ref Planet.IsColorSettingsFoldoutOpen, ref ColorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool toggleFoldout, ref Editor editor)
    {
        if (settings != null)
        {
            toggleFoldout = EditorGUILayout.InspectorTitlebar(toggleFoldout, settings);

            using var check = new EditorGUI.ChangeCheckScope();
            if (toggleFoldout)
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if (check.changed == true)
                {
                    onSettingsUpdated?.Invoke();
                }
            }
        }
    }

    private void OnEnable()
    {
        Planet = (Planet)target;
    }
}