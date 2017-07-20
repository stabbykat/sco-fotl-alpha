using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;

[InitializeOnLoad]
[CustomEditor(typeof(CamDebug))]
public class CamDebugEditor : Editor {

    static CamDebugEditor()
    {
        if (EditorApplication.update != null)
        {
            if (!EditorApplication.update.GetInvocationList().Any(d => d.Method == typeof(CamDebugEditor).GetMethod("CamUpdate", System.Reflection.BindingFlags.Static)))
                EditorApplication.update += CamUpdate;
            else
                Debug.Log("Found delegate");
        }
        else
            EditorApplication.update += CamUpdate;
    }

    static void CamUpdate()
    {
        CamDebug c;
        if((c = SceneView.FindObjectOfType<CamDebug>()) != null)
        {
            MoveCam(c);
        }
    }
    public override void OnInspectorGUI()
    {
        Camera cam = ((CamDebug)target).GetComponent<Camera>();
        if (cam != null)
        {
            if (cam.isActiveAndEnabled)
                EditorGUILayout.LabelField("PosRot", string.Format("<{0}><{1}>", cam.transform.position, cam.transform.rotation));
        } else Debug.Log("No cam found");
    }

    static Camera cRef;
    static void MoveCam(Object c)
    {
        Camera cam = ((CamDebug)c).GetComponent<Camera>();
        if (cam != null)
        {
            if (cRef == null) cRef = EditorWindow.GetWindow<SceneView>().camera;
            if (cRef != null)
            {
                Transform t = cRef.transform;
                cam.transform.position = t.position;
                cam.transform.rotation = t.rotation;
                cam.fieldOfView = cRef.fieldOfView;
                if ((cam.orthographic = cRef.orthographic) == true)
                    cam.orthographicSize = cRef.orthographicSize;

            } else Debug.Log("Camera is null");
        }
    }
    void MoveCam()
    {
        GameObject obj = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().GetRootGameObjects().First(o => o.name == target.name);
        Camera cam = obj.GetComponent<Camera>();
        if (cam != null)
        {
            if (EditorWindow.GetWindow<SceneView>().camera != null)
            {
                Transform t = EditorWindow.GetWindow<SceneView>().camera.transform;
                cam.transform.position = t.position;
                cam.transform.rotation = t.rotation;
            } else Debug.Log("Camera is null");
        }
    }
    
        /*
        int cId;
        cId = GUIUtility.GetControlID(FocusType.Passive);

        switch(Event.current.GetTypeForControl(cId))
        {
            case EventType.MouseDown:
                GUIUtility.hotControl = cId;
                Debug.Log("ok mousedown");
                Event.current.Use();
                break;
            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                Event.current.Use();
                break;
        }

        RaycastHit rcHit;
        if(Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out rcHit))
        {
            Handles.color = (Event.current.type == EventType.MouseDown && Event.current.button == 0) ? Color.red : Color.yellow;
            Vector3 noisePos = rcHit.point;
            Handles.DrawLine(noisePos + new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)), noisePos + new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)));
        }
        */
}
