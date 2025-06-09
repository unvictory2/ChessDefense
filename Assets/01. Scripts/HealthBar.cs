using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fill;
    public Vector3 offset = new Vector3(0, 2f, 0); // 인스펙터에서 각 프리팹마다 설정 가능

    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            // 위치 설정
            transform.position = transform.parent.position + offset;
            // 카메라를 바라보도록 회전
            transform.LookAt(transform.position + _cameraTransform.forward, _cameraTransform.up);
        }
    }

    public void SetHealth(float current, float max)
    {
        fill.fillAmount = current / max;
    }
}
