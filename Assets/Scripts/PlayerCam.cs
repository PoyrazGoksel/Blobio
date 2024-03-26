using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _offSet = 10f;
    [SerializeField] private float _panAngle = 60f;
    [SerializeField] private Transform _slimeTrans;
    [SerializeField] private Transform _myTrans;
    
    private void Update()
    {
        Vector3 offSetVect = Vector3.back * _offSet;

        Quaternion angleAxis = Quaternion.AngleAxis(_panAngle, Vector3.right);

        Vector3 rotatedVect = angleAxis * offSetVect;

        _myTrans.position = _slimeTrans.position + rotatedVect;
        _myTrans.LookAt(_slimeTrans);
    }
}