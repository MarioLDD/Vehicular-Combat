using UnityEngine;
using Cinemachine;

public class LimitCameraYRotation : MonoBehaviour
{
    public CinemachineFreeLook freeLookCam;
    public float minYRotation = -80.0f; // Establece el límite mínimo en grados.
    public float maxYRotation = 80.0f;  // Establece el límite máximo en grados.

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
        //    // Obtiene la rotación actual del objetivo en el eje X.
        //    float currentYRotation = freeLookCam.m_YAxis.Value;

        //    // Limita la rotación al rango permitido.
        //    currentYRotation = Mathf.Clamp(currentYRotation, minYRotation, maxYRotation);

        //    // Establece la rotación restringida de nuevo en el objetivo.
        //    freeLookCam.m_YAxis.m_MaxValue = minYRotation;

        //}
    }
}