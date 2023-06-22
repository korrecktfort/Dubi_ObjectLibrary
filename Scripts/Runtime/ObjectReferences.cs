using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dubi.Library
{
    [CreateAssetMenu(menuName ="Dubi/ObjectLibrary/Library")]
    public class ObjectReferences : ScriptableObject
    {
        [SerializeField] GameObject[] prefabs = new GameObject[0];
        [System.NonSerialized] GameObject[] instances = new GameObject[0];
       
        public void Instantiate(Transform parent, out ISource[] iSourceArray)
        {
            List<GameObject> list = new List<GameObject>();

            foreach(GameObject prefab in this.prefabs)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.SetParent(parent);
                list.Add(obj);
            }

            this.instances = list.ToArray();

            /// Internal iSource distribution to prevent stack overflow behaviour
            iSourceArray = parent.GetComponentsInChildren<ISource>(true);
            foreach (ISource iSource in iSourceArray)
                foreach (IInjectComponent iInject in parent.GetComponentsInChildren<IInjectComponent>(true))
                    iInject.InjectComponent(iSource as Component);

#if UNITY_2021_3_OR_NEWER
            foreach (GameObject instance in this.instances)
                foreach (Canvas canvas in instance.GetComponentsInChildren<Canvas>(true))
                {
                    canvas.enabled = false;
                    canvas.enabled = true;
                }
#endif
        }
    }
}
