using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // イベントの利用
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    // プレイヤー関連 ---------------------------------------------------
    private GameObject PlayerObj = null;        // プレイヤーオブジェクト
    private Vector3 CurrentPos = Vector3.zero;  // プレイヤー座標格納用
    private int[,] PlayerPos;                   // プレイヤー座標比較用

    public float MoveSpeed = 1.0f;              // プレイヤーの動く速さ
    private int PlayerDir = 0;
    public bool IsMoving = false;               // プレイヤーが動いているか

    private GamePad _gameInputs;             //
    private Vector2 _moveInputValue;            //プレイヤームーブcontroller兼キーボード

    // プレイヤーが全方向に進めるかどうか
    private bool IsAdvance_KeyW = false;        // Z+方向
    private bool IsAdvance_KeyA = false;        // Z-方向
    private bool IsAdvance_KeyS = false;        // X-方向
    private bool IsAdvance_KeyD = false;        // X-方向

    // プレイヤーがどの方向に進んだか
    [SerializeField]
    public bool PressKey_W = false;
    [SerializeField]
    public bool PressKey_A = false;
    [SerializeField]
    public bool PressKey_S = false;
    [SerializeField]
    public bool PressKey_D = false;

    [SerializeField]
    public int StepCount = 0;                  // 歩数
    // ------------------------------------------------------------------

    // 2Dマップ生成スクリプト用 -----------------------------------------
    [SerializeField]
    private GameObject GameClear_Text;
    [SerializeField]
    private GameObject GameOver_Text;
    public bool IsTouched = false;
    // ------------------------------------------------------------------

    // 2Dマップ生成スクリプト用 -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // マップ情報格納用
    private bool bMapLoading = false;           // マップがロードされたか
    // ------------------------------------------------------------------

    // 歩数カウントを伝えるイベント
    public event Action Walk;
    public event Action Auto;
    private List<int> log;
    private int logIdx = 0;
    private bool replay = false;
    // 次のシーンチェンジ可能かどうか
    public bool ClearCheck = false;
    // ワープチェック(myst)
    public GameObject WarpEffect;
    private int WarpCount;
    public int WarpTime = 210;
    private bool GoalCheak;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        StageMake = GameObject.Find("StageMake");
        log = StageMake.GetComponent<ElementGenerator>().GetEnemyData().GetReplay();
        // Input Actionインスタンス生成
        _gameInputs = new GamePad();

        // Actionイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        // Input Actionを機能させるためには、
        // 有効化する必要がある
        _gameInputs.Enable();

        WarpCount = 0;
        GoalCheak = false;

    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();
        Debug.Log(_moveInputValue);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && StepCount == 0)
        {
            if (log.Count > 0)
                replay = true;
            else
                Debug.Log("一度以上クリアしてください");
        }
        // 一度だけ実行
        if (!bMapLoading)
        {
            // ダンジョンマップ読み込み
            elementGenerator = StageMake.GetComponent<ElementGenerator>();
            map = elementGenerator.GetMapGenerate();

            // プレイヤー座標読み込み
            CurrentPos.x = elementGenerator.GetMapPlayer().x;
            CurrentPos.y = 2.0f;
            CurrentPos.z = elementGenerator.GetMapPlayer().y;

            bMapLoading = true;
        }

        // 進む先に壁があるかどうか
        IsAdvancePlayer();

        //// プレイヤーの向きを変える
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    PlayerDir = !PlayerDir;
        //}

        // 進む方向にプレイヤーを向かせる
        if (PlayerDir == 0) // 前
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else if (PlayerDir == 1) // 後ろ
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }
        else if (PlayerDir == 2) // 左
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        else if (PlayerDir == 3) // 右
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        }

        // プレイヤーが止まっていて、プレイヤーが進める状態であったら
        if (PlayerObj.transform.position == CurrentPos && !GoalCheak &&!IsTouched)
        {
            // 止まっている
            IsMoving = false;

            if (!IsAdvance_KeyW)
            {
                if (Input.GetKeyDown(KeyCode.W) || (replay&&log[logIdx] == 0) || _moveInputValue.y > 0)
                {
                    if (log.Count > logIdx + 1)
                        logIdx++;
                    PressKey_W = true;
                    PlayerDir = 0;
                    CurrentPos.z += 1.0f;
                    Walk?.Invoke();
                    StepCount++;
                }
            }
            if (!IsAdvance_KeyS)
            {
                if (Input.GetKeyDown(KeyCode.S)|| (replay&&log[logIdx] == 2) || _moveInputValue.y < 0)
                {
                    if (log.Count > logIdx + 1)
                        logIdx++;
                    PressKey_S = true;
                    PlayerDir = 1;
                    CurrentPos.z -= 1.0f;
                    Walk?.Invoke();
                    StepCount++;
                }
            }
            if (!IsAdvance_KeyA)
            {
                if (Input.GetKeyDown(KeyCode.A) || (replay&&log[logIdx] == 1)||_moveInputValue.x < 0)
                {
                    if (log.Count > logIdx+1)
                        logIdx++;
                    PressKey_A = true;
                    PlayerDir = 3;
                    CurrentPos.x -= 1.0f;
                    Walk?.Invoke();
                    StepCount++;
                }
            }
            if (!IsAdvance_KeyD )
            {
                if (Input.GetKeyDown(KeyCode.D) || (replay&&log[logIdx] == 3) || _moveInputValue.x > 0)
                {
                    if (log.Count > logIdx+1)
                        logIdx++;
                    PressKey_D = true;

                    PlayerDir = 2;
                    CurrentPos.x += 1.0f;
                    Walk?.Invoke();
                    StepCount++;
                }
            }


        }
        else
        {
            // 動いている
            IsMoving = true;

            PressKey_W = false;
            PressKey_A = false;
            PressKey_S = false;
            PressKey_D = false;
        }

        // ゴールしているか
        if(map[(int)CurrentPos.x,(int)CurrentPos.z] == 3 && !IsTouched)
        {
            replay = false;
            GoalCheak = true;
            //StageMake.GetComponent<ElementGenerator>().GetEnemyData().SetReplay(GetComponent<AutoRun>().GetDir());
            //GameClear_Text.SetActive(true);
            WarpCount++;
            WarpEffect.SetActive(true);
            if (WarpCount >= WarpTime && !IsTouched)
            {               
                ClearCheck = true;
            }

        }

        if(IsTouched)
        {
            GameOver_Text.SetActive(true);
            WarpEffect.SetActive(false);
        }

        // 移動処理
        PlayerObj.transform.position = Vector3.MoveTowards(PlayerObj.transform.position, CurrentPos, MoveSpeed * Time.deltaTime);
    }


    private void IsAdvancePlayer()
    {
        // 前に壁があるか
        if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 2 || (4 <= map[(int)CurrentPos.x, (int)CurrentPos.z+1]) && (map[(int)CurrentPos.x, (int)CurrentPos.z+1] <= 24))
        {
            IsAdvance_KeyW = true;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 3)
        {
            IsAdvance_KeyW = false;
        }

        // 後ろに壁があるか
        if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 2 || (4 <= map[(int)CurrentPos.x, (int)CurrentPos.z-1]) && (map[(int)CurrentPos.x, (int)CurrentPos.z-1] <= 24))
        {
            IsAdvance_KeyS = true;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 3)
        {
            IsAdvance_KeyS = false;
        }

        // 左に壁があるか
        if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2 || (4 <= map[(int)CurrentPos.x - 1, (int)CurrentPos.z]) && (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] <= 24))
        {
            IsAdvance_KeyA = true;
        }
        else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 3)
        {
            IsAdvance_KeyA = false;
        }

        // 右に壁があるか
        if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2 || (4 <= map[(int)CurrentPos.x + 1, (int)CurrentPos.z]) && (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] <= 24))
        {
            IsAdvance_KeyD = true;
        }
        else if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 1 || map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 3)
        {
            IsAdvance_KeyD = false;
        }
    }

    public void MovePosPlayer(float targetXPos,float targetZPos)
    {
        CurrentPos.x = targetXPos;
        CurrentPos.y = 2.0f;
        CurrentPos.z = targetZPos;
    }
}
