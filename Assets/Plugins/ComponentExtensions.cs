using UnityEngine;

public static class ComponentExtensions
{
    public static bool TryGetComponentInParents<T>(this GameObject gameObject, out T component) where T : Component
    {
        component = null;

        if (gameObject == null)
            return false;

        // Intenta obtener el componente en el GameObject actual
        component = gameObject.GetComponent<T>();

        if (component != null)
            return true;

        // Si el componente no se encuentra en el GameObject actual, busca en los padres
        Transform parent = gameObject.transform.parent;
        while (parent != null)
        {
            component = parent.GetComponent<T>();
            if (component != null)
                return true;
            parent = parent.parent;
        }

        // Si no se encuentra en ningún padre, devuelve false
        return false;
    }
}
