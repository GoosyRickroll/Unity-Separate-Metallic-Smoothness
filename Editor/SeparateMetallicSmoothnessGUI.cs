#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SeparateMetallicSmoothnessGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        EditorGUILayout.LabelField(
        "Separate Metallic & Smoothness",
        EditorStyles.boldLabel
        );

        EditorGUILayout.LabelField(
        "Created by Sheepish90",
        EditorStyles.miniLabel
        );

        EditorGUILayout.Space(10);
        
        // Draw the regular inspector, excluding hidden level properties.
        base.OnGUI(materialEditor, properties);
        EditorGUILayout.Space(6);
        if (GUILayout.Button("Smoothness Levels..."))
            LevelsWindow.Open(materialEditor.targets, "Smoothness");
        if (GUILayout.Button("Metallic Levels..."))
            LevelsWindow.Open(materialEditor.targets, "Metallic");
    }
}

public class LevelsWindow : EditorWindow
{
    private Material[] materials;
    private string channel;
    private const float MinGap = 0.001f;

    public static void Open(Object[] targets, string selectedChannel)
    {
        var window = CreateInstance<LevelsWindow>();
        window.channel = selectedChannel;
        window.titleContent = new GUIContent(selectedChannel + " Levels");
        window.materials = System.Array.ConvertAll(targets, t => t as Material);
        window.minSize = new Vector2(340, 220);
        window.Show();
    }

    private void OnGUI()
    {
        if (materials == null || materials.Length == 0 || materials[0] == null)
        {
            EditorGUILayout.HelpBox("Select a material and reopen this window.", MessageType.Info);
            return;
        }
        var mat = materials[0];
        string blackKey = "_" + channel + "Black";
        string whiteKey = "_" + channel + "White";
        string gammaKey = "_" + channel + "Gamma";
        if (!mat.HasProperty(blackKey))
        {
            EditorGUILayout.HelpBox("This material does not use the updated shader.", MessageType.Warning);
            return;
        }
        EditorGUILayout.LabelField(channel + " Levels", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Adjusts the texture values before the Strength multiplier. Changes are saved on the material.", MessageType.None);
        float black = mat.GetFloat(blackKey);
        float white = mat.GetFloat(whiteKey);
        float gamma = mat.GetFloat(gammaKey);
        EditorGUI.BeginChangeCheck();
        black = EditorGUILayout.Slider("Black Point", black, 0f, 0.999f);
        white = EditorGUILayout.Slider("White Point", white, 0.001f, 1f);
        gamma = EditorGUILayout.Slider("Gamma", gamma, 0.1f, 5f);
        if (EditorGUI.EndChangeCheck())
        {
            black = Mathf.Min(black, white - MinGap);
            white = Mathf.Max(white, black + MinGap);
            Apply(blackKey, whiteKey, gammaKey, black, white, gamma);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Reset Levels"))
            Apply(blackKey, whiteKey, gammaKey, 0f, 1f, 1f);
        EditorGUILayout.HelpBox("Black = 0, White = 1, Gamma = 1 leaves values unchanged. Lowering White Point brightens midtones; increasing Gamma also brightens midtones.", MessageType.Info);
    }

    private void Apply(string blackKey, string whiteKey, string gammaKey, float black, float white, float gamma)
    {
        foreach (var mat in materials)
        {
            if (mat == null || !mat.HasProperty(blackKey)) continue;
            Undo.RecordObject(mat, "Change " + channel + " Levels");
            mat.SetFloat(blackKey, black);
            mat.SetFloat(whiteKey, white);
            mat.SetFloat(gammaKey, gamma);
            EditorUtility.SetDirty(mat);
        }
        SceneView.RepaintAll();
        Repaint();
    }
}
#endif
