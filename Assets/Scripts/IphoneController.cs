using UnityEngine;
using UnityEngine.UI;

public class IphoneController : MonoBehaviour
{
    public Text gyroQuaternionText;
    public Text gyroEulerText;
    public Text iphoneRotationText;
    public Text iphoneRotationEulerText;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            Debug.Log("System input chk");
        }
        Debug.Log(Input.gyro.enabled);
    }

    void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Quaternion gyro = GyroToUnity(Input.gyro.attitude);
            transform.localRotation = gyro;
            gyroQuaternionText.text = "Gyro Quaternion" + Input.gyro.attitude.ToString();
            gyroEulerText.text = "Gyro Euler" + Input.gyro.attitude.eulerAngles.ToString();
        }

        iphoneRotationText.text = "iphone Quaternion" + transform.localRotation.ToString();
        iphoneRotationEulerText.text = "iphone Euler" + transform.localRotation.eulerAngles.ToString();
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.z, q.y, -q.w);
    }
}
