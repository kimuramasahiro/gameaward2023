using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; // �C�x���g�̗��p
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // �v���C���[�֘A ---------------------------------------------------
    private GameObject PlayerObj = null;        // �v���C���[�I�u�W�F�N�g
    private Vector3 CurrentPos = Vector3.zero;  // �v���C���[���W�i�[�p
    private int[,] PlayerPos;                   // �v���C���[���W��r�p

    public float MoveSpeed = 1.0f;              // �v���C���[�̓�������
    private bool PlayerDir = false;             // �v���C���[�̌���(false:�E true:��)
    public bool IsMoving = false;               // �v���C���[�������Ă��邩

    private Controller _gameIputs;

    // �v���C���[���S�����ɐi�߂邩�ǂ���
    private bool IsAdvance_KeyW = false;        // Z+����
    private bool IsAdvance_KeyA = false;        // Z-����
    private bool IsAdvance_KeyS = false;        // X-����
    private bool IsAdvance_KeyD = false;        // X-����

    // �v���C���[���ǂ̕����ɐi�񂾂�
    [SerializeField]
    public bool PressKey_W = false;
    [SerializeField]
    public bool PressKey_A = false;
    [SerializeField]
    public bool PressKey_S = false;
    [SerializeField]
    public bool PressKey_D = false;

    [SerializeField]
    public int StepCount = 0;                  // ����
    // ------------------------------------------------------------------

    // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    [SerializeField]
    private GameObject GameClear_Text;
    [SerializeField]
    private GameObject GameOver_Text;
    public bool IsTouched = false;
    // ------------------------------------------------------------------

    // 2D�}�b�v�����X�N���v�g�p -----------------------------------------
    private GameObject StageMake;               // 
    private ElementGenerator elementGenerator;  // 
    private int[,] map;                         // �}�b�v���i�[�p
    private bool bMapLoading = false;           // �}�b�v�����[�h���ꂽ��
    // ------------------------------------------------------------------

    // �����J�E���g��`����C�x���g
    public event Action Walk;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
        StageMake = GameObject.Find("StageMake");

        // Input Action�C���X�^���X����
        _gameInputs = new Controller();

        // Action�C�x���g�o�^
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        // Input Action���@�\�����邽�߂ɂ́A
        // �L��������K�v������
        _gameInputs.Enable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Move�A�N�V�����̓��͎擾
        _moveInputValue = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // ��x�������s
        if (!bMapLoading)
        {
            // �_���W�����}�b�v�ǂݍ���
            elementGenerator = StageMake.GetComponent<ElementGenerator>();
            map = elementGenerator.GetMapGenerate();

            // �v���C���[���W�ǂݍ���
            CurrentPos.x = elementGenerator.GetMapPlayer().x;
            CurrentPos.y = 1.5f;
            CurrentPos.z = elementGenerator.GetMapPlayer().y;

            bMapLoading = true;
        }

        // �i�ސ�ɕǂ����邩�ǂ���
        IsAdvancePlayer();

        // �v���C���[�̌�����ς���
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerDir = !PlayerDir;
        }

        // �i�ޕ����Ƀv���C���[����������
        if (!PlayerDir)
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
        {
            PlayerObj.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        // �v���C���[���~�܂��Ă��āA�v���C���[���i�߂��Ԃł�������
        if (PlayerObj.transform.position == CurrentPos)
        {
            // �~�܂��Ă���
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
            // �����Ă���
            IsMoving = true;

            PressKey_W = false;
            PressKey_A = false;
            PressKey_S = false;
            PressKey_D = false;
        }

        // �S�[�����Ă��邩
        if(map[(int)CurrentPos.x,(int)CurrentPos.z] == 3 && !IsTouched)
        {
            GameClear_Text.SetActive(true);
        }

        if(IsTouched)
        {
            GameOver_Text.SetActive(true);
        }

        // �ړ�����
        PlayerObj.transform.position = Vector3.MoveTowards(PlayerObj.transform.position, CurrentPos, MoveSpeed * Time.deltaTime);
    }

    private void IsAdvancePlayer()
    {
        // �O�ɕǂ����邩
        if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 2)
        {
            IsAdvance_KeyW = true;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z + 1] == 3)
        {
            IsAdvance_KeyW = false;
        }

        // ���ɕǂ����邩
        if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 0 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 2)
        {
            IsAdvance_KeyS = true;
        }
        else if (map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 1 || map[(int)CurrentPos.x, (int)CurrentPos.z - 1] == 3)
        {
            IsAdvance_KeyS = false;
        }

        // ���ɕǂ����邩
        if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 0 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 2)
        {
            IsAdvance_KeyA = true;
        }
        else if (map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 1 || map[(int)CurrentPos.x - 1, (int)CurrentPos.z] == 3)
        {
            IsAdvance_KeyA = false;
        }

        // �E�ɕǂ����邩
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
