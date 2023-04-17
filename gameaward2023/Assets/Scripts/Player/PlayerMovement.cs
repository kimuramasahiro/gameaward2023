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
    private bool PlayerDir = false;             // プレイヤーの向き(false:右 true:左)
    public bool IsMoving = false;               // プレイヤーが動いているか

    private Controller _gameIputs;

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

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        StageMake = GameObject.Find("StageMake");

        // Input Actionインスタンス生成
        _gameInputs = new Controller();

        // Actionイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        // Input Actionを機能させるためには、
        // 有効化する必要がある
        _gameInputs.Enable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // 一度だけ実行
        if (!bMapLoading)
        {
            // ダンジョンマップ読み込み
            elementGenerator = StageMake.GetComponent<ElementGenerator>();
            map = elementGenerator.GetMapGenerate();

            // プレイヤー座標読み込み
            CurrentPos.x = elementGenerator.GetMapPlayer().x;
            CurrentPos.y = 1.5f;
            CurrentPos.z = elementGenerator.GetMapPlayer().y;

            bMapLoading = true;
        }

        // 進む先に壁があるかどうか
        IsAdvancePlayer();

        // プレイヤーの向きを変える
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerDir = !PlayerDir;
        }

        // 進む方向にプレイヤーを向かせる
        if (!PlayerDir)
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        // プレイヤーが止まっていて、プレイヤーが進める状態であったら
        if (PlayerObj.transform.position == CurrentPos)
        {
            // 止まっている
            IsMoving = false;

            if (!IsAdvance_KeyW)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    PressKey_W = true;
                    CurrentPos.z += 1.0f;
                    Walk?.Invoke();
                    StepCount++;
                }
            }
            if (!IsAdvance_KeyS)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    PressKey_S = true;
                    CurrentPos.z -= 1.0f;
                    Walk?.Invoke();
                    StepCount++;
                }
            }
            if (!IsAdvance_KeyA)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    PressKey_A = true;
                    PlayerDir = true;
                    CurrentPos.x -= 1.0f;
                    Walk?.Invoke();
                    StepCount++;
                }
            }
            if (!IsAdvance_KeyD)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    PressKey_D = true;

                    PlayerDir = false;
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
            GameClear_Text.SetActive(true);
        }

        if(IsTouched)
        {
            GameOver_Text.SetActive(true);
        }

        // 移動処理
        PlayerObj.transform.position = Vector3.MoveTowards(PlayerObj.transform.position, CurrentPos, MoveSpeed * Time.deltaTime);
    }

    private void IsAdvancePlayer()
    {
        // 前に壁があるか
        if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 2)
        {
            IsAdvance_KeyW = true;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 3)
        {
            IsAdvance_KeyW = false;
        }

        // 後ろに壁があるか
        if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 2)
        {
            IsAdvance_KeyS = true;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 3)
        {
            IsAdvance_KeyS = false;
        }

        // 左に壁があるか
        if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
        {
            IsAdvance_KeyA = true;
        }
        else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 3)
        {
            IsAdvance_KeyA = false;
        }

        // 右に壁があるか
        if (map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x + 1, (int)CurrentPos.z] == 2)
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
