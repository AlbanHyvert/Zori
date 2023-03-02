using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _worldCam = null;
    [SerializeField] private Camera _cinematicCam = null;

    public Camera Camera
    => _worldCam;

    public void InBattle(bool value)
    {
        if(value == true)
        {
            _cinematicCam.gameObject.SetActive(false);
            _worldCam.gameObject.SetActive(false);

            return;
        }
        else
        {
            _cinematicCam.gameObject.SetActive(false);
            _worldCam.gameObject.SetActive(true);
        }
    }

    public void InCinematic(bool value)
    {
        if (value == true)
        {
            _worldCam.gameObject.SetActive(false);
            _cinematicCam.gameObject.SetActive(true);

            return;
        }
        else
        {
            _worldCam.gameObject.SetActive(false);
            _cinematicCam.gameObject.SetActive(true);
        }
    }
}
