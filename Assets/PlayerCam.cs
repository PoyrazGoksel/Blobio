using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _offSet = 10f;
    [SerializeField] private float _panAngle = 60f;
    [SerializeField] private Transform _slimeTrans;
    
    private void Update()
    {
        Vector3 offSetVect = Vector3.back * _offSet /* Vector3(0f, 0f, -10f) */;

        Quaternion angleAxis = Quaternion.AngleAxis(_panAngle, Vector3.right);

        Vector3 rotatedVect = angleAxis * offSetVect;

        transform.position = _slimeTrans.position + rotatedVect;
        transform.LookAt(_slimeTrans);
    }
}