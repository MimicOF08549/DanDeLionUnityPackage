using System.Collections.Generic;
using UnityEngine;

namespace JayC {
    [RequireComponent(typeof(Collider))]
    public class ColliderCollectionManager : MonoBehaviour
    {
        public List<GameObject> ListObjects
        {
            get
            {
                p_ObjectList.RemoveAll(objectslist => objectslist == null || objectslist is null);

                return p_ObjectList;
            }
        }

        private Collider p_Collider;
        [SerializeField]
        private List<GameObject> p_ObjectList = new();

        void OnEnable()
        {
            // p_Collider = GetComponent<Collider>();
            // if (p_Collider)
            // {
            //     p_Collider.isTrigger = true;
            //     p_Collider.gameObject.layer = LayerMask.NameToLayer("CastArea");
            // }
        }


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




        void OnTriggerEnter(Collider other)
        {
            if (!ListObjects.Contains(other.gameObject))
            {
                ListObjects.Add(other.gameObject);
            }

        }

        void OnTriggerExit(Collider other)
        {
            if (ListObjects.Contains(other.gameObject))
            {
                ListObjects.Remove(other.gameObject);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!ListObjects.Contains(collision.gameObject))
            {
                ListObjects.Add(collision.gameObject);
            }

        }

        void OnCollisionExit(Collision collision)
        {
            if (ListObjects.Contains(collision.gameObject))
            {
                ListObjects.Remove(collision.gameObject);
            }
        }


    }
}
