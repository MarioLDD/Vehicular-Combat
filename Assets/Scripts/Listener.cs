using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour
{
    public List<GameObject> objetosParaCentrar; // Lista de GameObjects que quieres centrar
    private GameObject objetoCentrado; // El GameObject que se centrará

    private void Start()
    {
        objetoCentrado = gameObject;
    }
    void Update()
    {
        if (objetosParaCentrar.Count > 0)
        {
            Vector3 puntoMedio = CalcularPuntoMedio();
            objetoCentrado.transform.position = puntoMedio;
        }
    }

    Vector3 CalcularPuntoMedio()
    {
        Vector3 puntoMedio = Vector3.zero;

        foreach (var objeto in objetosParaCentrar)
        {
            puntoMedio += objeto.transform.position;
        }

        puntoMedio /= objetosParaCentrar.Count;

        return puntoMedio;
    }
}
