using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dubi.Library
{
    public interface IInjectComponent
    {       
        public void InjectComponent(Component component);
    }
}

