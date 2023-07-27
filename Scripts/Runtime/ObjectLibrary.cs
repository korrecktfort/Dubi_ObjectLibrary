using System.Collections.Generic;
using UnityEngine;
using Dubi.SingletonSpace;
using System.Linq;
using Rewired;

namespace Dubi.Library
{
    public class ObjectLibrary : Singleton<ObjectLibrary>
    {                
        ISource[] sourceComponents = new ISource[0];
        string[] instantiated = new string[0];
        
        public void LoadLibrary(string libraryName)
        {
            foreach(string instantiated in this.instantiated)
            {
                if (instantiated == libraryName)
                    /// Library already instantiated
                    return;
            }

            ObjectReferences library = Resources.Load<ObjectReferences>(libraryName);

            if (library != null)
            {
                GameObject libraryParent = new GameObject("!" + libraryName + " - Parent");
                DontDestroyOnLoad(libraryParent);
                Transform parent = libraryParent.transform;

                library.Instantiate(parent, out ISource[] iSourceArray);

                List<ISource> list = this.sourceComponents.ToList();

                foreach (ISource iSource in iSourceArray)
                    if (!list.Contains(iSource))
                        list.Add(iSource);

                this.sourceComponents = list.ToArray();

                List<string> instantiatedList = this.instantiated.ToList();
                instantiatedList.Add(libraryName);
                this.instantiated = instantiatedList.ToArray();

            }
        }       
        
        //public void RegisterSourceComponent(ISource iSource)
        //{
        //    List<ISource> list = this.sourceComponents.ToList();

        //    if (!list.Contains(iSource))
        //    {
        //        list.Add(iSource);
        //        this.sourceComponents = list.ToArray();
        //    }
        //}

        public T GetSourceComponent<T>(string libraryName) where T : Component
        {            
            LoadLibrary(libraryName);

            foreach (ISource source in this.sourceComponents.ToArray())
                if (source is T)
                    return source as T;

            return null;
        }
    }
}
