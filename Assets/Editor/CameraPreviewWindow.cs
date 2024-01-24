using UnityEngine;
using UnityEditor;

public class CameraPreviewWindow : EditorWindow
{
    private Camera _camera = null;
    private static CameraPreviewWindow window;

    private string[] options = new string[] { "16:9", "4:3", "1:1" };
    private int index = 0;

    [MenuItem("Tools/Camera Preview")]
    internal static void Init()
    {
        window = (CameraPreviewWindow)GetWindow(typeof(CameraPreviewWindow), false, "Camera Preview");
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 400f, 300f);
    }

    internal void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        Camera[] cameras = FindObjectsOfType<Camera>();

        if(cameras.Length == 0)
        {
            GUILayout.Label("No cameras on stage.", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            return;
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            var pressed = GUILayout.Button(i.ToString(), new GUIStyle(GUI.skin.GetStyle("Button"))
            {
                alignment = TextAnchor.MiddleCenter,
                fixedWidth = 30,
                fixedHeight = 30,
            });
            if (pressed)
            {
                _camera = cameras[i];
            }
        }

        EditorGUILayout.EndHorizontal();

        index = EditorGUILayout.Popup(index, options, GUILayout.MaxWidth(50));

        if (_camera == null) { _camera = cameras[0]; }

        float ratio = index switch
        {
            0 => 16f/9f,
            1 => 4f/3f,
            2 => 1,
            _ => 1,
        };
        float X = (window.position.width - window.position.width * .95f) * .5f;
        Vector2 size = new Vector2(window.position.size.x, window.position.size.y / ratio);
        Rect rect = new Rect(new Vector2(X, 60), size * .95f);
        Handles.DrawCamera(rect, _camera);
    }
}