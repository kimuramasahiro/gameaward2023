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
    private int mapSize = 25;
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
        // ディレクトリを指定させる　一旦させない
         
        //directory = (DefaultAsset)EditorGUILayout.ObjectField("ディレクトリを指定", directory, typeof(DefaultAsset), true);
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
        hero,
        _1,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        _11,
        _12,
        _13,


    }
    // マップウィンドウのサイズ
    const float windowWidth = 750.0f;
    const float windowHeight = 750.0f;
    // マップサイズ
    private int mapSize;
    private int[] map;
    private string[] imageMap;
    private int[] mapEnemyLog;
    // 親ウィンドウの参照を持つ
    private MapEditor parent;
    private Rect[,] gridRect;// グリッドの配列 
    private string path;
    private string imagePath;
    private string landImagePath;
    private string seaImagePath;
    private string goalImagePath;
    private string[] enemyImagePath = new string[1];
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
        mapEnemyLog = new int[data.enemies.Count() + 1];
        for (int i = 0; i<data.enemies.Count() + 1; ++i)
            mapEnemyLog[i] = -1;

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

        DrawSetData();
        
        DrawResetMapButton();
        
        // クリックされた位置を探して、その場所に画像データを入れる
        Event e = Event.current;
        
        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            if (e.button == 1)// 右
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
                if (xx == mapSize)
                {
                    return;
                }
                // 後はy位置だけ探す
                for (int yy = 0; yy < mapSize; yy++)
                {
                    if (gridRect[yy, xx].Contains(pos))
                    {

                        if (map[yy * mapSize + xx] > 9)
                        {

                            map[yy * mapSize + xx] %= 10;
                            if (map[yy * mapSize + xx] == 0)
                            {
                                imageMap[yy * mapSize + xx] = seaImagePath;
                            }
                            else if (map[yy * mapSize + xx] == 1)
                            {
                                imageMap[yy * mapSize + xx] = landImagePath;
                            }
                            else if (map[yy * mapSize + xx] == 3)
                            {
                                imageMap[yy * mapSize + xx] = goalImagePath;
                            }
                        }
                        else
                        {
                            imageMap[yy * mapSize + xx] = "";
                            map[yy * mapSize + xx] = -1;
                        }
                        Repaint();
                        break;
                    }
                }
            }
            else if (e.button == 0)//左
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
                if (xx == mapSize)
                {
                    return;
                }
                // 後はy位置だけ探す
                for (int yy = 0; yy < mapSize; yy++)
                {
                    if (gridRect[yy, xx].Contains(pos))
                    {
                        // 消しゴムの時はデータを消す
                        if (imagePath == null)
                        {
                            if (map[yy * mapSize + xx] > 9)
                            {

                                map[yy * mapSize + xx] %= 10;
                                if (map[yy * mapSize + xx] == 0)
                                {
                                    imageMap[yy * mapSize + xx] = seaImagePath;
                                }
                                else if (map[yy * mapSize + xx] == 1)
                                {
                                    imageMap[yy * mapSize + xx] = landImagePath;
                                }
                                else if (map[yy * mapSize + xx] == 3)
                                {
                                    imageMap[yy * mapSize + xx] = goalImagePath;
                                }
                            }
                            else
                            {
                                imageMap[yy * mapSize + xx] = "";
                                map[yy * mapSize + xx] = -1;
                            }
                        }
                        else if (imagePath.IndexOf(IMAGE.none.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] > 9)
                            {
                                map[yy * mapSize + xx] %= 10;
                                if (map[yy * mapSize + xx] == 0)
                                {
                                    imageMap[yy * mapSize + xx] = seaImagePath;
                                }
                                else if (map[yy * mapSize + xx] == 1)
                                {
                                    imageMap[yy * mapSize + xx] = landImagePath;
                                }
                                else if (map[yy * mapSize + xx] == 3)
                                {
                                    imageMap[yy * mapSize + xx] = goalImagePath;
                                }
                            }
                            else
                            {
                                imageMap[yy * mapSize + xx] = "";
                                map[yy * mapSize + xx] = -1;
                            }

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
                        else if (imagePath.IndexOf(IMAGE._1.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 10;
                                mapEnemyLog[0] = yy * mapSize + xx;
                                data.enemies[0].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._2.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 20;
                                mapEnemyLog[1] = yy * mapSize + xx;
                                data.enemies[1].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._3.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 30;
                                mapEnemyLog[2] = yy * mapSize + xx;
                                data.enemies[2].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._4.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 40;
                                mapEnemyLog[3] = yy * mapSize + xx;
                                data.enemies[3].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._5.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 50;
                                mapEnemyLog[4] = yy * mapSize + xx;
                                data.enemies[4].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._6.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 60;
                                mapEnemyLog[5] = yy * mapSize + xx;
                                data.enemies[5].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._7.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 70;
                                mapEnemyLog[6] = yy * mapSize + xx;
                                data.enemies[6].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._8.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 80;
                                mapEnemyLog[7] = yy * mapSize + xx;
                                data.enemies[7].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._9.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 90;
                                mapEnemyLog[8] = yy * mapSize + xx;
                                data.enemies[8].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._10.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 100;
                                mapEnemyLog[9] = yy * mapSize + xx;
                                data.enemies[9].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._11.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 110;
                                mapEnemyLog[10] = yy * mapSize + xx;
                                data.enemies[10].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._12.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 120;
                                mapEnemyLog[11] = yy * mapSize + xx;
                                data.enemies[11].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE._13.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 130;
                                mapEnemyLog[12] = yy * mapSize + xx;
                                data.enemies[12].SetPos(new Vector2(xx, yy));
                            }

                        }
                        else if (imagePath.IndexOf(IMAGE.hero.ToString()) > -1)
                        {
                            if (map[yy * mapSize + xx] != -1 && map[yy * mapSize + xx] < 10)
                            {
                                imageMap[yy * mapSize + xx] = imagePath;
                                map[yy * mapSize + xx] += 1000;
                                mapEnemyLog[data.enemies.Count()] = yy * mapSize + xx;
                                data.SetHeroPos(new Vector2(xx, yy));
                            }

                        }
                        else
                        {
                            imageMap[yy * xx] = "";
                            map[yy * xx] = -1;
                        }
                        Repaint();
                        break;
                    }
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
                    if (map[yy * mapSize + xx] > 9)
                    {
                        enemyImagePath[0] = imageMap[yy * mapSize + xx];
                        int mapBase = map[yy * mapSize + xx] % 10;
                        
                        if (mapBase == 0)
                        {
                            imageMap[yy * mapSize + xx] = seaImagePath;
                        }
                        else if (mapBase == 1)
                        {
                            imageMap[yy * mapSize + xx] = landImagePath;
                        }
                        else if (mapBase == 3)
                        {
                            imageMap[yy * mapSize + xx] = goalImagePath;
                        }
                        Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(imageMap[yy * mapSize + xx], typeof(Texture2D));
                        GUI.DrawTexture(gridRect[yy, xx], tex, ScaleMode.StretchToFill, true);
                        int enemyData = (map[yy * mapSize + xx] - (map[yy * mapSize + xx] % 10)) / 10;
                        Debug.Log(enemyData);
                        Debug.Log(map[yy * mapSize + xx]);
                        

                        

                        if (data.enemies.Count() >= enemyData||enemyData==100)
                        {
                            int enemyDataLog;
                            if (enemyData == 100)
                                enemyDataLog = data.enemies.Count();
                            else
                                enemyDataLog = enemyData-1;

                            if (mapEnemyLog[enemyDataLog] == yy * mapSize + xx || mapEnemyLog[enemyDataLog] == -1)
                            {
                                Texture2D tex1 = (Texture2D)AssetDatabase.LoadAssetAtPath(enemyImagePath[0], typeof(Texture2D));
                                GUI.DrawTexture(gridRect[yy, xx], tex1, ScaleMode.StretchToFill, true);
                                Debug.Log(imageMap[yy * mapSize + xx] + "::" + enemyImagePath[0] + "::" + map[yy * mapSize + xx] + ":y=" + yy + ":x=" + xx);
                                imageMap[yy * mapSize + xx] = enemyImagePath[0];
                            }
                            else
                            {
                                map[yy * mapSize + xx] = mapBase;
                            }
                        }
                        else
                        {
                            map[yy * mapSize + xx] = mapBase;
                        }
                        

                    }
                    else
                    {
                        Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(imageMap[yy * mapSize + xx], typeof(Texture2D));
                        GUI.DrawTexture(gridRect[yy, xx], tex);
                    }
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
        float y = windowHeight * 0.2f;
        float w = windowWidth * 0.7f / div;
        float h = windowHeight * 0.7f / div;

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
        Undo.RegisterCompleteObjectUndo(data, "map");
        Undo.RegisterCompleteObjectUndo(data, "imageMap");
        Undo.RegisterCompleteObjectUndo(data, "save");
        AssetDatabase.SaveAssets();
    }
    private void DrawImageParts()
    {
        if (!string.IsNullOrEmpty(path))
        {
            float x = 00.0f;
            float y = 00.0f;
            float w = 40.0f;
            float h = 40.0f;
            float maxW = 750.0f/*windowHeight * 0.1f*/;

            List<string> names = AssetDatabase.FindAssets("", new[] { path }).Select(AssetDatabase.GUIDToAssetPath).ToList();
            List<string> enemiesNames = AssetDatabase.FindAssets("_", new[] { path }).Select(AssetDatabase.GUIDToAssetPath).ToList();
            Debug.Log(enemiesNames.Count());
            foreach (string d in enemiesNames)
            {
                Debug.Log(d);
            }
            names.RemoveRange(data.enemies.Count(), enemiesNames.Count() - data.enemies.Count());
                //Directory.GetFiles(path, "*.png");
                EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            foreach (string d in names)
            {
                //if (x == 0.0f)
                //{
                //    EditorGUILayout.BeginHorizontal();
                //}
                if (x > maxW)
                {
                    x = 0.0f;
                    y += h;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                
                GUILayout.FlexibleSpace();
                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(d, typeof(Texture2D));
                if (d.IndexOf(IMAGE.land.ToString()) > -1)
                {
                    landImagePath = d;
                }
                else if (d.IndexOf(IMAGE.sea.ToString()) > -1)
                {
                    seaImagePath = d;
                }
                else if (d.IndexOf(IMAGE.goal.ToString()) > -1)
                {
                    goalImagePath = d;
                }
                if (GUILayout.Button(tex, GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                {
                    imagePath = d;
                }
                //GUILayout.FlexibleSpace();
                x += w;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }

    private void DrawResetMapButton()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
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
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawSetData()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace(); // フィールドの右寄せ
        if (GUILayout.Button("set map"))
        {
            SetMapData();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}
