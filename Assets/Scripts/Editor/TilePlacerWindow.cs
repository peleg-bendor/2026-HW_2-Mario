using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Editor window for stamping one tile at a time into the scene by clicking, alongside the level
// window's all-at-once build. It places only what the tile prefab map already knows about, so
// everything it creates can be written back out as level data.
public class TilePlacerWindow : EditorWindow
{
    [SerializeField] private TilePrefabMap tilePrefabMap;
    [SerializeField] private GameObject levelParent;
    [SerializeField] private int selectedIndex;

    // Off by default and easy to switch back off, because while it's on the Scene view stops
    // selecting things on click.
    private bool placingEnabled;

    [MenuItem("Tools/Tile Placer")]
    public static void ShowWindow()
    {
        GetWindow<TilePlacerWindow>("Tile Placer");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        tilePrefabMap = EditorGUILayout.ObjectField("Tile Prefabs", tilePrefabMap, typeof(TilePrefabMap), false) as TilePrefabMap;
        levelParent = EditorGUILayout.ObjectField("Parent", levelParent, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space();

        List<TilePrefabMap.Entry> entries = UsableEntries();
        if (entries.Count == 0 || levelParent == null)
        {
            placingEnabled = false;
            EditorGUILayout.HelpBox("Assign a parent and a tile prefab map with at least one prefab in it.", MessageType.Info);
            return;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, entries.Count - 1);

        string[] names = new string[entries.Count];
        for (int i = 0; i < entries.Count; i++)
            names[i] = entries[i].tileId + " - " + entries[i].prefab.name;

        selectedIndex = EditorGUILayout.Popup("Tile", selectedIndex, names);
        placingEnabled = EditorGUILayout.Toggle("Placing Enabled", placingEnabled);

        EditorGUILayout.HelpBox(placingEnabled
            ? "Click in the Scene view to place. Clicking won't select anything while this is on."
            : "Placing is off, so the Scene view behaves normally.", MessageType.None);
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!placingEnabled || levelParent == null)
            return;

        List<TilePrefabMap.Entry> entries = UsableEntries();
        if (entries.Count == 0)
            return;

        // Claims the Scene view's default click handling, so placing a tile doesn't also select
        // whatever happened to be under the cursor.
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event current = Event.current;
        if (!TryGetCell(current.mousePosition, out Vector3 cell))
            return;

        Handles.color = Color.yellow;
        Handles.DrawWireCube(levelParent.transform.TransformPoint(cell), Vector3.one);

        if (current.type == EventType.MouseMove)
            sceneView.Repaint();

        // Alt is Unity's own camera modifier, so a click holding it is someone orbiting rather
        // than someone placing a tile.
        if (current.type == EventType.MouseDown && current.button == 0 && !current.alt)
        {
            Place(entries[Mathf.Clamp(selectedIndex, 0, entries.Count - 1)].prefab, cell);
            current.Use();
        }
    }

    private List<TilePrefabMap.Entry> UsableEntries()
    {
        List<TilePrefabMap.Entry> usable = new List<TilePrefabMap.Entry>();
        if (tilePrefabMap == null || tilePrefabMap.Entries == null)
            return usable;

        foreach (TilePrefabMap.Entry entry in tilePrefabMap.Entries)
        {
            if (entry != null && entry.prefab != null)
                usable.Add(entry);
        }

        return usable;
    }

    private bool TryGetCell(Vector2 mousePosition, out Vector3 cell)
    {
        cell = Vector3.zero;

        // The level is flat on z = 0, so a cell is wherever the cursor's ray crosses that plane.
        // A Scene view rotated to look along it has no answer, hence the failure case.
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        Plane levelPlane = new Plane(Vector3.forward, Vector3.zero);
        if (!levelPlane.Raycast(ray, out float distance))
            return false;

        // Rounded in the parent's own space, since that's the space tiles are placed and stored
        // in. Rounding in world space instead would offset every placement by however far the
        // parent sits from the origin.
        Vector3 local = levelParent.transform.InverseTransformPoint(ray.GetPoint(distance));
        cell = new Vector3(Mathf.Round(local.x), Mathf.Round(local.y), 0f);
        return true;
    }

    private void Place(GameObject prefab, Vector3 cell)
    {
        int undoGroup = Undo.GetCurrentGroup();

        ClearCell(cell);

        GameObject tile = PrefabUtility.InstantiatePrefab(prefab, levelParent.transform) as GameObject;
        if (tile == null)
        {
            Debug.LogWarning("Could not place " + prefab.name);
            return;
        }

        tile.transform.localPosition = cell;

        // Instantiated through PrefabUtility rather than plain Instantiate, so the tile keeps a
        // link to its prefab - which is the only way saving can work out what tile id it is.
        Undo.RegisterCreatedObjectUndo(tile, "Place Tile");

        Undo.SetCurrentGroupName("Place Tile");
        Undo.CollapseUndoOperations(undoGroup);

        Debug.Log("Placed " + prefab.name + " at (" + cell.x + ", " + cell.y + ")");
    }

    private void ClearCell(Vector3 cell)
    {
        Transform parent = levelParent.transform;

        // A cell holds one tile, since that's all the level file can store. Painting over
        // something replaces it rather than leaving two objects stacked in the same place.
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Transform child = parent.GetChild(i);
            if (Mathf.RoundToInt(child.localPosition.x) == Mathf.RoundToInt(cell.x) &&
                Mathf.RoundToInt(child.localPosition.y) == Mathf.RoundToInt(cell.y))
                Undo.DestroyObjectImmediate(child.gameObject);
        }
    }
}
