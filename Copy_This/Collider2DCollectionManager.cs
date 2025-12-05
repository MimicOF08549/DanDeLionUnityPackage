using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Collider2DCollectionManager : MonoBehaviour
{
    public List<GameObject> ListObjects
    {
        get
        {
            p_ObjectList.RemoveAll(objectslist => objectslist == null || objectslist is null);

            return p_ObjectList;
        }
    }

    private Collider2D p_Collider;
    [SerializeField]
    private List<GameObject> p_ObjectList = new();

    public UnityEvent<GameObject> OnColliderEnter = new(); 
    public UnityEvent<GameObject> OnColliderExit = new(); 
    public UnityEvent<GameObject> OnTriggerEnter = new(); 
    public UnityEvent<GameObject> OnTriggerExit = new(); 


    public List<T> GetObjectsWithComponent<T>() where T : Component
    {
        List<GameObject> listObjects = new();

        listObjects = ListObjects.FindAll(searchObject => searchObject.TryGetComponent<T>(out _));

        List<T> returnObjects = new();

        foreach (var xGameObject in listObjects)
        {
            returnObjects.Add(xGameObject.GetComponent<T>());
        }

        return returnObjects;
    }

    public List<GameObject> GetObjectsWithLayerIndex(params int[] layerIndex)
    {
        List<GameObject> listObjects = new();

        foreach (var layerobject in layerIndex)
        {
            listObjects.AddRange(ListObjects.FindAll(objInList => objInList.layer == layerobject));
        }

        return listObjects;
    }


    public List<T> GetObjectsWithComponentAndLayerIndex<T>(params int[] layerIndex) where T : Component
    {
        List<GameObject> listObjects = new();

        foreach (var layerobject in layerIndex)
        {
            listObjects.AddRange(ListObjects.FindAll(searchObject => searchObject.layer == layerobject && searchObject.TryGetComponent<T>(out _)));
        }

        List<T> returnObjects = new();

        foreach (var xGameObject in listObjects)
        {
            returnObjects.Add(xGameObject.GetComponent<T>());
        }

        return returnObjects;
    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (!ListObjects.Contains(other.gameObject))
        {
            ListObjects.Add(other.gameObject);
            OnTriggerEnter?.Invoke(other.gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (ListObjects.Contains(other.gameObject))
        {
            ListObjects.Remove(other.gameObject);
            OnTriggerExit?.Invoke(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!ListObjects.Contains(collision.gameObject))
        {
            ListObjects.Add(collision.gameObject);
            OnColliderEnter?.Invoke(collision.gameObject);
        }

    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (ListObjects.Contains(collision.gameObject))
        {
            ListObjects.Remove(collision.gameObject);
            OnColliderExit?.Invoke(collision.gameObject);
        }
    }


}

