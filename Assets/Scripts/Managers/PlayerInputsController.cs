using UnityEngine;
using XRCardboard;

namespace Managers
{
    public class PlayerInputsController : MonoBehaviour
    {
        [SerializeField] private XRCardboardController _xrCardboardController;
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newBullet = BulletSpawner.BaseInstance.Spawn();
            
                // this is too complicated really, could have just added an origin transform to the camera and reference it
                var instantiationDistanceFromCamera = 0.3f;
                var shootingOriginWorldPosition =
                    _xrCardboardController.Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,
                        instantiationDistanceFromCamera));

                newBullet.SetPosition(shootingOriginWorldPosition);
                newBullet.ShootInDirection(_xrCardboardController.CameraTransform.forward);
            }
        }
    }
}