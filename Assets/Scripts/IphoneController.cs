using UnityEngine;
using UnityEngine.UI;

public class IphoneController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Text gyroQuaternionText;
    [SerializeField] private Text gyroEulerText;
    [SerializeField] private Text iphoneRotationText;
    [SerializeField] private Text iphoneRotationEulerText;
    [Header("Position")]
    [SerializeField] private Text positionText;
    [SerializeField] private Text accelText;
    [SerializeField] private Text velocityText;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 prevVelocity = Vector3.zero;
    private Vector3 prevAcceleration = Vector3.zero;
    
    private bool isStarted = false;
    
    // // 필터 계수 (0 < alpha < 1), 값이 작을수록 고주파만 통과
    // [SerializeField] private float accelHighPassAlpha = 0.95f;
    //
    // private Vector3 rawAccelFiltered = Vector3.zero;
    // private Vector3 prevRawAccel = Vector3.zero;
    
    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            Debug.Log("System input chk");
        }
        Debug.Log(Input.gyro.enabled);
    }

    private void Update()
    {
        Quaternion gyro = Quaternion.identity;
        if (SystemInfo.supportsGyroscope)
        {
            gyro = GyroToUnity(Input.gyro.attitude);
            transform.localRotation = gyro;
            gyroQuaternionText.text = "Gyro Quaternion" + Input.gyro.attitude.ToString();
            gyroEulerText.text = "Gyro Euler" + Input.gyro.attitude.eulerAngles.ToString();
            
            iphoneRotationText.text = "iphone Quaternion" + transform.localRotation.ToString();
            iphoneRotationEulerText.text = "iphone Euler" + transform.localRotation.eulerAngles.ToString();
        }

        if (isStarted)
        {
            // 아이폰 좌표계 -> 유니티 좌표계
            // 로컬 좌표 기준 가속도 -> 월드 좌표 기준 가속도 
            Vector3 rawAccel = AccelToUnity(Input.acceleration);
            
            Vector3 currentAcceleration = gyro * rawAccel;
            currentAcceleration += Vector3.up;
            accelText.text = "Accel " + currentAcceleration.ToString();
        
            // 속도 = 가속도의 적분
            currentVelocity = 0.5f * (prevAcceleration +  currentAcceleration) * Time.deltaTime + prevVelocity;
            velocityText.text = "Velocity " + currentVelocity.ToString();

            // 위치 = 속도의 적분
            transform.position += 0.5f*(prevVelocity + currentVelocity) * Time.deltaTime;
            positionText.text = "Position" + transform.localPosition.ToString();
        
        
            prevAcceleration = currentAcceleration;
            prevVelocity = currentVelocity;
        }
    }

    private Vector3 AccelToUnity(Vector3 original)
    {
        return new Vector3(original.x, original.z, original.y);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.z, q.y, -q.w);
    }

    public void OnClickStartButton()
    {
        isStarted = true;
    }
}