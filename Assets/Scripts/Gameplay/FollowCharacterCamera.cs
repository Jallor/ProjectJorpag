using UnityEngine;
using NaughtyAttributes;

public class FollowCharacterCamera : MonoBehaviour
{
    private Camera _CurrentCamera = null;
    [SerializeField] private IWorldEntity _EntityToFollow = null;
    [SerializeField] private float _CameraSpeed = 2f;

    [MinMaxSlider(0.0f, 1.0f)]
    [SerializeField] private Vector2 _HorizontalBound = new Vector2(0.4f, 0.6f);
    [MinMaxSlider(0.0f, 1.0f)]
    [SerializeField] private Vector2 _HardHorizontalBound = new Vector2(0.2f, 0.8f);
    [MinMaxSlider(0.0f, 1.0f)]
    [SerializeField] private Vector2 _VerticalBound = new Vector2(0.4f, 0.6f);
    [MinMaxSlider(0.0f, 1.0f)]
    [SerializeField] private Vector2 _HardVerticalBound = new Vector2(0.2f, 0.8f);

    public void Update()
    {
        if (_EntityToFollow == null)
        {
            if (EntityManager.Inst != null)
            {
                _EntityToFollow = EntityManager.Inst.TryGetEntityByID(0);
            }
            return;
        }
        if (_CurrentCamera == null)
        {
            _CurrentCamera = Camera.main;
        }

        Vector3 entityPosition = _EntityToFollow.GetWorldPosition();

        Vector2 charaCurrentInCamPos = _CurrentCamera.WorldToScreenPoint(entityPosition);
        Vector2 charaPercentPos = new Vector2(charaCurrentInCamPos.x / _CurrentCamera.pixelWidth,
            charaCurrentInCamPos.y / _CurrentCamera.pixelHeight);

        Vector3 offsetToApply = new Vector3();
        // Check Left
        if (charaPercentPos.x < _HorizontalBound.x)
        {
            offsetToApply.x = -_CameraSpeed * Time.deltaTime;

            if (charaPercentPos.x < _HardHorizontalBound.x)
            {
                float leftBound = _CurrentCamera.pixelWidth * _HardHorizontalBound.x;
                float leftBoundPosition = _CurrentCamera.ScreenToWorldPoint(new Vector3(leftBound, 0, 0)).x;
                offsetToApply.x += entityPosition.x - leftBoundPosition;
            }
        }
        // Check Right
        else if (charaPercentPos.x > _HorizontalBound.y)
        {
            offsetToApply.x = _CameraSpeed * Time.deltaTime;
            
            if (charaPercentPos.x > _HardHorizontalBound.y)
            {
                float rightBound = _CurrentCamera.pixelWidth * _HardHorizontalBound.y;
                float rightBoundPosition = _CurrentCamera.ScreenToWorldPoint(new Vector3(rightBound, 0, 0)).x;
                offsetToApply.x += entityPosition.x - rightBoundPosition;
            }
        }
        // Check Bottom
        if (charaPercentPos.y < _VerticalBound.x)
        {
            offsetToApply.y = -_CameraSpeed * Time.deltaTime;

            if (charaPercentPos.y < _HardVerticalBound.x)
            {
                float bottomBound = _CurrentCamera.pixelHeight * _HardVerticalBound.x;
                float bottomBoundPosition = _CurrentCamera.ScreenToWorldPoint(new Vector3(0, bottomBound, 0)).y;
                offsetToApply.y += entityPosition.y - bottomBoundPosition;
            }
        }
        // Check Top
        else if (charaPercentPos.y > _VerticalBound.y)
        {
            offsetToApply.y = _CameraSpeed * Time.deltaTime;
            
            if (charaPercentPos.y > _HardVerticalBound.y)
            {
                float topBound = _CurrentCamera.pixelHeight * _HardVerticalBound.y;
                float topBoundPosition = _CurrentCamera.ScreenToWorldPoint(new Vector3(0, topBound, 0)).y;
                offsetToApply.y += entityPosition.y - topBoundPosition;
            }
        }
        _CurrentCamera.transform.Translate(offsetToApply);
    }
}
