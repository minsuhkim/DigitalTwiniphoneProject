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

    private Vector3 velocity;


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
        }

        iphoneRotationText.text = "iphone Quaternion" + transform.localRotation.ToString();
        iphoneRotationEulerText.text = "iphone Euler" + transform.localRotation.eulerAngles.ToString();

        // 아이폰 좌표계 -> 유니티 좌표계
        //Vector3 accel = AccelToUnity(Input.acceleration);
        Vector3 accel = gyro * AccelToUnity(Input.acceleration);
        accel += Vector3.up;
        accelText.text = "Accel " + accel.ToString();
        //accelText.text = "Accel " + Input.acceleration.ToString();

        // 속도 = 가속도의 적분
        velocity += accel * Time.deltaTime;
        velocityText.text = "Velocity " + velocity.ToString();

        // 위치 = 속도의 적분
        transform.localPosition += velocity * Time.deltaTime;
        positionText.text = "Position" + transform.localPosition.ToString();
    }

    private Vector3 AccelToUnity(Vector3 original)
    {
        return new Vector3(original.x, original.z, original.y);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.z, q.y, -q.w);
    }
}
