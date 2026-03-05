using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class BTVisualizer : EditorWindow
{
    private Vector2 _scrollOffset = Vector2.zero;
    private float _zoomScale = 1.0f;
    private const float MinSpacing = 160f;
    private const float VerticalSpacing = 120f; // 노드 간 세로 간격

    [MenuItem("Window/AI/BT Visualizer")]
    public static void ShowWindow()
    {
        BTVisualizer window = GetWindow<BTVisualizer>("BT Visualizer Professional");
        window.FitWindowToTree(); // 창이 열릴 때 사이즈 맞춤
    }

    // --- [추가] 노드 트리의 크기를 계산하여 윈도우 사이즈를 맞춤 ---
    private void FitWindowToTree()
    {
        if (!Application.isPlaying) return;
        BT bt = Selection.activeGameObject?.GetComponent<BT>();
        if (bt?.Root == null) return;

        float totalWidth = GetNodeWidth(bt.Root) + 100f; // 좌우 여백 포함
        float totalHeight = GetTreeDepth(bt.Root) * VerticalSpacing + 150f; // 상하 여백 포함

        // 모니터 해상도보다 커지지 않도록 제한 (유니티 에디터 기준)
        float targetWidth = Mathf.Min(totalWidth, 1600f);
        float targetHeight = Mathf.Min(totalHeight, 900f);

        this.position = new Rect(100, 100, targetWidth, targetHeight);
        _scrollOffset = Vector2.zero; // 스크롤 초기화
        _zoomScale = 1.0f; // 줌 초기화
    }

    // 트리의 최대 깊이(층수)를 계산
    private int GetTreeDepth(Node node)
    {
        List<Node> children = GetChildren(node);
        if (children == null || children.Count == 0) return 1;

        int maxChildDepth = 0;
        foreach (var child in children)
        {
            maxChildDepth = Mathf.Max(maxChildDepth, GetTreeDepth(child));
        }
        return 1 + maxChildDepth;
    }

    private void OnGUI()
    {
        if (!Application.isPlaying) {
            EditorGUILayout.HelpBox("플레이 모드에서만 작동합니다.", MessageType.Info);
            return;
        }

        BT bt = Selection.activeGameObject?.GetComponent<BT>();
        if (bt?.Root == null) return;

        // 하단에 사이즈 맞춤 버튼 추가 (수동으로 맞추고 싶을 때)
        if (GUI.Button(new Rect(10, position.height - 30, 120, 20), "Fit to Window")) {
            FitWindowToTree();
        }

        HandleEvents();

        EditorZoomArea.Begin(_zoomScale, new Rect(0, 0, position.width, position.height));
        // 창 크기에 맞춘 중앙 정렬 시작점 계산
        Vector2 startPos = new Vector2(position.width / 2, 50) + _scrollOffset;
        DrawNodeRecursive(bt.Root, startPos);
        EditorZoomArea.End();

        Repaint();
    }

    private void DrawNodeRecursive(Node node, Vector2 pos)
    {
        // 틱 시스템 체크
        bool wasExecuted = node.LastTickCount == Time.frameCount;

        Color themeColor;
        string stateText;

        if (!wasExecuted) {
            themeColor = new Color(0.3f, 0.3f, 0.3f);
            stateText = "Inactive";
        } else {
            themeColor = node.CurrentState switch {
                State.Running => new Color(0.1f, 0.8f, 1.0f),
                State.Success => new Color(0.2f, 0.8f, 0.2f),
                State.Failure => new Color(0.9f, 0.3f, 0.3f),
                _ => new Color(0.5f, 0.5f, 0.5f)
            };
            stateText = node.CurrentState.ToString();
        }

        Rect rect = new Rect(pos.x - 70, pos.y, 140, 60);

        // 노드 드로잉 (기존 전문 디자인 코드와 동일)
        GUI.color = new Color(0, 0, 0, 0.3f);
        GUI.Box(new Rect(rect.x + 3, rect.y + 3, rect.width, rect.height), "", (GUIStyle)"window");
        GUI.color = Color.white;
        GUI.backgroundColor = wasExecuted ? new Color(0.15f, 0.15f, 0.15f) : new Color(0.1f, 0.1f, 0.1f);
        GUI.Box(rect, "", (GUIStyle)"window");
        GUI.backgroundColor = themeColor;
        GUI.Box(new Rect(rect.x, rect.y, rect.width, 5), "");
        GUI.backgroundColor = Color.white;

        string displayName = GetNodeName(node);
        var labelStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter, fontSize = 11, fontStyle = FontStyle.Bold };
        labelStyle.normal.textColor = wasExecuted ? Color.white : Color.gray;
        GUI.Label(new Rect(rect.x, rect.y + 12, rect.width, 25), displayName, labelStyle);
        labelStyle.fontSize = 10; labelStyle.fontStyle = FontStyle.Normal; labelStyle.normal.textColor = themeColor;
        GUI.Label(new Rect(rect.x, rect.y + 35, rect.width, 20), $"[{stateText}]", labelStyle);

        List<Node> children = GetChildren(node);
        if (children != null && children.Count > 0)
        {
            float totalWidth = GetNodeWidth(node);
            float currentX = pos.x - totalWidth / 2f;

            foreach (var child in children)
            {
                float childWidth = GetNodeWidth(child);
                Vector2 childPos = new Vector2(currentX + childWidth / 2f, pos.y + VerticalSpacing);
                Handles.color = wasExecuted ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 0.1f);
                DrawOrthogonalLine(rect, childPos);
                DrawNodeRecursive(child, childPos);
                currentX += childWidth;
            }
        }
    }

    // --- 나머지 헬퍼 함수들 (기존과 동일) ---
    private void DrawOrthogonalLine(Rect parentRect, Vector2 childPos) {
        Handles.BeginGUI();
        Vector3 start = new Vector3(parentRect.center.x, parentRect.yMax);
        Vector3 end = new Vector3(childPos.x, childPos.y);
        float midY = start.y + (end.y - start.y) * 0.5f;
        Handles.DrawPolyLine(start, new Vector3(start.x, midY), new Vector3(end.x, midY), end);
        Handles.EndGUI();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing / _zoomScale) + 10;
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing / _zoomScale) + 10;
        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
        float offsetX = _scrollOffset.x % gridSpacing; float offsetY = _scrollOffset.y % gridSpacing;
        for (int i = -10; i < widthDivs; i++) Handles.DrawLine(new Vector3(gridSpacing * i + offsetX, 0, 0), new Vector3(gridSpacing * i + offsetX, position.height, 0));
        for (int j = -10; j < heightDivs; j++) Handles.DrawLine(new Vector3(0, gridSpacing * j + offsetY, 0), new Vector3(position.width, gridSpacing * j + offsetY, 0));
        Handles.color = Color.white; Handles.EndGUI();
    }

    private string GetNodeName(Node node) {
        var field = typeof(Node).GetField("Name", BindingFlags.Public | BindingFlags.Instance);
        string name = field?.GetValue(node) as string;
        return string.IsNullOrEmpty(name) ? node.GetType().Name : name;
    }

    private float GetNodeWidth(Node node) {
        List<Node> children = GetChildren(node);
        if (children == null || children.Count == 0) return MinSpacing;
        float totalWidth = 0;
        foreach (var child in children) totalWidth += GetNodeWidth(child);
        return Mathf.Max(totalWidth, MinSpacing);
    }

    private List<Node> GetChildren(Node node) {
        FieldInfo field = node.GetType().GetField("_children", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        var currentType = node.GetType();
        while (field == null && currentType != null) {
            field = currentType.GetField("_children", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            currentType = currentType.BaseType;
        }
        return field?.GetValue(node) as List<Node>;
    }

    private void HandleEvents() {
        Event e = Event.current;
        if (e.type == EventType.MouseDrag && (e.button == 1 || e.button == 2)) { _scrollOffset += e.delta / _zoomScale; e.Use(); }
        if (e.type == EventType.ScrollWheel) { _zoomScale = Mathf.Clamp(_zoomScale - e.delta.y * 0.01f, 0.3f, 2.0f); e.Use(); }
    }
}

// 줌 영역을 처리하기 위한 헬퍼 클래스
public class EditorZoomArea
{
    private static Matrix4x4 _prevGuiMatrix;
    public static void Begin(float zoomScale, Rect screenRect)
    {
        GUI.EndGroup();
        _prevGuiMatrix = GUI.matrix;

        Matrix4x4 translation = Matrix4x4.TRS(screenRect.min, Quaternion.identity, Vector3.one);
        Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
        GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

        GUI.BeginGroup(new Rect(0, 0, screenRect.width / zoomScale, screenRect.height / zoomScale));
    }
    public static void End()
    {
        GUI.EndGroup();
        GUI.matrix = _prevGuiMatrix;
        GUI.BeginGroup(new Rect(0, 21, Screen.width, Screen.height));
    }
}
