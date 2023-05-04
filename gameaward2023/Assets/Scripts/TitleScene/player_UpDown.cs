using UnityEngine;

public class player_UpDown : MonoBehaviour
{
    // 振幅
    [SerializeField] private float _amplitude = 1;

    // 周期
    [SerializeField] private float _period = 1;

    // 位相
    [SerializeField, Range(0, 1)] private float _phase = 0;

    // 動かす軸
    private enum Axis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private Axis _axis;

    private Transform _transform;
    private Vector3 _initPosition;

    // 初期化
    private void Awake()
    {
        _transform = transform;
        _initPosition = _transform.localPosition;
    }

    private void Update()
    {
        // 周期と位相を考慮した現在時間計算
        var t = 4 * _amplitude * (Time.time / _period + _phase + 0.25f);

        // 往復した値を計算
        var value = Mathf.PingPong(t, 2 * _amplitude) - _amplitude;

        // 変位計算
        var changePos = Vector3.zero;

        switch (_axis)
        {
            case Axis.X:
                changePos.x = value;
                break;
            case Axis.Y:
                changePos.y = value;
                break;
            case Axis.Z:
                changePos.z = value;
                break;
        }

        // 位置を反映
        _transform.localPosition = _initPosition + changePos;
    }
}