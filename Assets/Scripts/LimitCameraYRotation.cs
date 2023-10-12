using UnityEngine;
using Cinemachine;

public class LimitCameraYRotation : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public float minYRotation = -80.0f; // Establece el l�mite m�nimo en grados.
    public float maxYRotation = 80.0f;  // Establece el l�mite m�ximo en grados.

    private void Start()
    {
        freeLookCam.m_XAxis.m_Wrap = false;
        freeLookCam.m_XAxis.m_MinValue = minYRotation;
        freeLookCam.m_XAxis.m_MaxValue = maxYRotation;
    }
    private void Update()
    {
        //if (freeLookCam != null)
        //{
        //    // Obtiene la rotaci�n actual del objetivo en el eje X.
        //    float currentYRotation = freeLookCam.m_YAxis.Value;

        //    // Limita la rotaci�n al rango permitido.
        //    currentYRotation = Mathf.Clamp(currentYRotation, minYRotation, maxYRotation);

        //    // Establece la rotaci�n restringida de nuevo en el objetivo.
        //    freeLookCam.m_YAxis.m_MaxValue = minYRotation;

        //}
    }
}