using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // エディタ拡張
using System.IO; // ディレクトリ
using System.Linq; // ディレクトリ
[CustomEditor(typeof(EnemyData))]
public class MapEditor : Editor
{
    EnemyData data;
    // マップサイズ
    private int mapSize = 11;
    private int[] map;
    private DefaultAsset directory;
    private string path;
    // サブウィンドウ
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
        GUILayout.FlexibleSpace(); // フィールドの右寄せ
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
        GUILayout.FlexibleSpace(); // フィールドの右寄せ
        // ディレクトリを指定させる
         
        directory = (DefaultAsset)EditorGUILayout.ObjectField("ディレクトリを指定", directory, typeof(DefaultAsset), true);
        if (directory != null)
        {
            // DefaultAssetのパスを取得する
            path = AssetDatabase.GetAssetPath(directory);
            if (string.IsNullOrEmpty(path)) return;

            // ディレクトリでなければ、指定を解除する
            bool isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            if (isDirectory == false)
            {
                directory = null;
            }
        }
        else
        {
            // DefaultAssetのパスを取得する
            path = "Assets/Resources/MapTile";
            if (string.IsNullOrEmpty(path)) return;

            // ディレクトリでなければ、指定を解除する
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
    /// ディレクトリのパスを取得する
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
    // マップウィンドウのサイズ
    const float windowWidth = 750.0f;
    const float windowHeight = 750.0f;
    // マップサイズ
    private int mapSize;
    private int[] map;
    private string[] imageMap;
    // 親ウィンドウの参照を持つ
    private MapEditor parent;
    private Rect[,] gridRect;// グリッドの配列 
    private string path;
    private string imagePath;
    private EnemyData data;
    // サブウィンドウを開く
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
        // グリッドの作成
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
        // グリッド線を描画する
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
        
        // クリックされた位置を探して、その場所に画像データを入れる
        Event e = Event.current;
        if (e.type == EventType.MouseDown)
        {
            Vector2 pos = Event.current.mousePosition;
            int xx;
            // x位置を先に計算して、計算回数を減らす
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
            // 後はy位置だけ探す
            for (int yy = 0; yy < mapSize; yy++)
            {
                if (gridRect[yy, xx].Contains(pos))
                {
                    // 消しゴムの時はデータを消す
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

        // 選択した画像を描画する
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
    // グリッド線を描画
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
    // グリッドデータを生成
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
        // Undoに対応する
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
        GUILayout.FlexibleSpace(); // フィールドの右寄せ
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
        GUILayout.FlexibleSpace(); // フィールドの右寄せ
        if (GUILayout.Button("set map"))
        {
            SetMapData();
        }
        EditorGUILayout.EndVertical();
    }
}
