using Cinemachine;
using UnityEngine;
using Utilities;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private CinemachineBasicMultiChannelPerlin _channelPerlin;

    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    private bool _canShake;

    private void Awake()
    {
        Instance = this;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _channelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        _canShake = AppManager.Instance.allowCameraShake;
    }

    public void ShakeCamera(float intensity, float time)
    {
        if(!_canShake) return;
        _channelPerlin.m_AmplitudeGain = intensity;
        _shakeTimer = time;
        _shakeTimerTotal = time;
        _startingIntensity = intensity;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            
            _channelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1-_shakeTimer / _shakeTimerTotal);
        }
    }
}
