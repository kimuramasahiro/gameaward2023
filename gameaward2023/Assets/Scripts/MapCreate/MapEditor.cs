using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // �G�f�B�^�g��
using System.IO; // �f�B���N�g��
using System.Linq; // �f�B���N�g��
[CustomEditor(typeof(EnemyData))]
public class MapEditor : Editor
{
    EnemyData data;
    // �}�b�v�T�C�Y
    private int mapSize = 11;
    private int[] map;
    private DefaultAsset directory;
    private string path;
    // �T�u�E�B���h�E
    private CreateMap subWindow;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        data = target as EnemyData;
        map = data.GetMap();
        data.SetMapSize(mapSize);
        DrawDirectory();
        DrawMapWindowButton();
    }
    private void DrawMapWindowButton()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace(); // �t�B�[���h�̉E��
        if (GUILayout.Button("open map editor"))
        {
            if (subWindow == null)
            {
                subWindow = CreateMap.WillAppear(this);
            }
            else
            {
                subWindow.Focus();
            }
        }
        EditorGUILayout.EndVertical();
    }
    
    private void DrawDirectory()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace(); // �t�B�[���h�̉E��
        // �f�B���N�g�����w�肳����
         
        directory = (DefaultAsset)EditorGUILayout.ObjectField("�f�B���N�g�����w��", directory, typeof(DefaultAsset), true);
        if (directory != null)
        {
            // DefaultAsset�̃p�X���擾����
            path = AssetDatabase.GetAssetPath(directory);
            if (string.IsNullOrEmpty(path)) return;

            // �f�B���N�g���łȂ���΁A�w�����������
            bool isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            if (isDirectory == false)
            {
                directory = null;
            }
        }
        else
        {
            // DefaultAsset�̃p�X���擾����
            path = "Assets/Resources/MapTile";
            if (string.IsNullOrEmpty(path)) return;

            // �f�B���N�g���łȂ���΁A�w�����������
            bool isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            if (isDirectory == false)
            {
                directory = null;
            }
        }
        EditorGUILayout.EndVertical();
    }
    public int GetMapSize()
    {
        return mapSize;
    }
    
    public string GetAssetPath()
    {
        return path;
    }

    public EnemyData GetEnemyData()
    {
        return data;
    }
    /// <summary>
    /// �f�B���N�g���̃p�X���擾����
    /// </summary>
    public string GetDirectoryPath()
    {
        if (directory == null) return null;

        return AssetDatabase.GetAssetPath(directory);
    }
}
public class CreateMap : EditorWindow
{
    private enum IMAGE
    {
        none,
        goal,
        land,
        sea,


    }
    // �}�b�v�E�B���h�E�̃T�C�Y
    const float windowWidth = 750.0f;
    const float windowHeight = 750.0f;
    // �}�b�v�T�C�Y
    private int mapSize;
    private int[] map;
    private string[] imageMap;
    // �e�E�B���h�E�̎Q�Ƃ�����
    private MapEditor parent;
    private Rect[,] gridRect;// �O���b�h�̔z�� 
    private string path;
    private string imagePath;
    private EnemyData data;
    // �T�u�E�B���h�E���J��
    public static CreateMap WillAppear(MapEditor _parent)
    {
        CreateMap window = (CreateMap)EditorWindow.GetWindow(typeof(CreateMap), false);
        window.Show();
        window.minSize = new Vector2(windowWidth, windowHeight);
        window.SetParent(_parent);
        window.mapSize = _parent.GetMapSize();
        window.data = _parent.GetEnemyData();
        
        window.init();
        window.path = _parent.GetAssetPath();
        // �O���b�h�̍쐬
        window.gridRect = window.CreateGrid(window.mapSize);
        return window;
    }
    private void init()
    {
        map = new int[mapSize* mapSize];
        imageMap = new string[mapSize* mapSize];
        for (int yy = 0; yy < mapSize; ++yy)
            for (int xx = 0; xx < mapSize; ++xx)
            {
                map[yy * mapSize + xx] = -1;
                imageMap[yy * mapSize + xx] = "";
            }
        if (data.isSaved())
        {
            map = data.GetMap();
            imageMap = data.GetImageMap();
        }
    }
    private void SetParent(MapEditor _parent)
    {
        parent = _parent;
    }

    private void OnGUI()
    {
        // �O���b�h����`�悷��
        for (int yy = 0; yy < mapSize; yy++)
        {
            for (int xx = 0; xx < mapSize; xx++)
            {
                DrawGridLine(gridRect[yy, xx]);
            }
        }
        DrawImageParts();

        DrawResetMapButton();
        
        DrawSetData();
        
        // �N���b�N���ꂽ�ʒu��T���āA���̏ꏊ�ɉ摜�f�[�^������
        Event e = Event.current;
        if (e.type == EventType.MouseDown)
        {
            Vector2 pos = Event.current.mousePosition;
            int xx;
            // x�ʒu���Ɍv�Z���āA�v�Z�񐔂����炷
            for (xx = 0; xx < mapSize; xx++)
            {
                Rect r = gridRect[0, xx];
                if (r.x <= pos.x && pos.x <= r.x + r.width)
                {
                    break;
                }
            }
            if(xx == mapSize)
            {
                return;
            }
            // ���y�ʒu�����T��
            for (int yy = 0; yy < mapSize; yy++)
            {
                if (gridRect[yy, xx].Contains(pos))
                {
                    // �����S���̎��̓f�[�^������
                    if(imagePath == null)
                    {
                        imageMap[yy * mapSize + xx] = "";
                        map[yy * mapSize + xx] = -1;
                    }
                    else if (imagePath.IndexOf(IMAGE.none.ToString()) > -1)
                    {
                        imageMap[yy * mapSize + xx] = "";
                        map[yy * mapSize + xx] = -1;
                    }
                    else if (imagePath.IndexOf(IMAGE.sea.ToString()) > -1)
                    {
                        imageMap[yy * mapSize + xx] = imagePath;
                        map[yy * mapSize + xx] = 0;
                    }
                    else if (imagePath.IndexOf(IMAGE.land.ToString()) > -1)
                    {
                        imageMap[yy * mapSize + xx] = imagePath;
                        map[yy * mapSize + xx] = 1;
                    }
                    else if (imagePath.IndexOf(IMAGE.goal.ToString()) > -1)
                    {
                        imageMap[yy * mapSize + xx] = imagePath;
                        map[yy * mapSize + xx] = 3;
                    }
                    else
                    {
                        imageMap[yy* xx] = "";
                        map[yy* xx] = -1;
                    }
                    Repaint();
                    break;
                }
            }
        }

        // �I�������摜��`�悷��
        for (int yy = 0; yy < mapSize; yy++)
        {
            for (int xx = 0; xx < mapSize; xx++)
            {
                if(imageMap[yy * mapSize + xx] == null)
                {
                    break;
                }
                if (imageMap[yy * mapSize + xx] != null && imageMap[yy * mapSize + xx].Length > 0)
                {
                    Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(imageMap[yy * mapSize + xx], typeof(Texture2D));
                    GUI.DrawTexture(gridRect[yy, xx], tex);
                }
            }
        }
    }
    // �O���b�h����`��
    private void DrawGridLine(Rect r)
    {
        // grid
        Handles.color = new Color(1f, 1f, 1f, 0.5f);

        // upper line
        Handles.DrawLine(
            new Vector2(r.position.x, r.position.y),
            new Vector2(r.position.x + r.size.x, r.position.y));

        // bottom line
        Handles.DrawLine(
            new Vector2(r.position.x, r.position.y + r.size.y),
            new Vector2(r.position.x + r.size.x, r.position.y + r.size.y));

        // left line
        Handles.DrawLine(
            new Vector2(r.position.x, r.position.y),
            new Vector2(r.position.x, r.position.y + r.size.y));

        // right line
        Handles.DrawLine(
            new Vector2(r.position.x + r.size.x, r.position.y),
            new Vector2(r.position.x + r.size.x, r.position.y + r.size.y));
    }
    // �O���b�h�f�[�^�𐶐�
    private Rect[,] CreateGrid(int div)
    {
        int sizeW = div;
        int sizeH = div;

        float x = 0.0f;
        float y = windowHeight * 0.1f;
        float w = windowWidth * 0.6f / div;
        float h = windowHeight * 0.6f / div;

        Rect[,] resultRects = new Rect[sizeH, sizeW];

        for (int yy = 0; yy < sizeH; yy++)
        {
            x = windowHeight * 0.1f;
            for (int xx = 0; xx < sizeW; xx++)
            {
                Rect r = new Rect(new Vector2(x, y), new Vector2(w, h));
                resultRects[yy, xx] = r;
                x += w;
            }
            y += h;
        }

        return resultRects;
    }
    
    private void SetMapData()
    {
        data.SetMap(map);
        data.SetImageMap(imageMap);
        data.Save();
        EditorUtility.SetDirty(data);
        // Undo�ɑΉ�����
        Undo.RegisterCompleteObjectUndo(data, "map");
        Undo.RegisterCompleteObjectUndo(data, "imageMap");
        Undo.RegisterCompleteObjectUndo(data, "save");
        AssetDatabase.SaveAssets();
    }
    private void DrawImageParts()
    {
        if (!string.IsNullOrEmpty(path))
        {
            float x = 0.0f;
            float y = 00.0f;
            float w = 50.0f;
            float h = 50.0f;
            float maxW = 300.0f;
            
            string[] names = AssetDatabase.FindAssets("", new[] { path }).Select(AssetDatabase.GUIDToAssetPath).ToArray();
            //Directory.GetFiles(path, "*.png");
            EditorGUILayout.BeginVertical();
            foreach (string d in names)
            {
                if (x > maxW)
                {
                    x = 0.0f;
                    y += h;
                    EditorGUILayout.EndHorizontal();
                }
                if (x == windowHeight * 0.1f)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                GUILayout.FlexibleSpace();
                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(d, typeof(Texture2D));
                if (GUILayout.Button(tex, GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                {
                    imagePath = d;
                }
                GUILayout.FlexibleSpace();
                x += w;
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void DrawResetMapButton()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace(); // �t�B�[���h�̉E��
        if (GUILayout.Button("reset map"))
        {
            data.Reset();
            map = new int[mapSize* mapSize];
            imageMap = new string[mapSize* mapSize];
            for (int yy = 0; yy < mapSize; ++yy)
                for (int xx = 0; xx < mapSize; ++xx)
                {
                    map[yy * mapSize + xx] = -1;
                    imageMap[yy * mapSize + xx] = "";
                }
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawSetData()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace(); // �t�B�[���h�̉E��
        if (GUILayout.Button("set map"))
        {
            SetMapData();
        }
        EditorGUILayout.EndVertical();
    }
}
