﻿using System;

namespace InfinityEngine.Core.Object
{
    [Serializable]
    public class UObject : FDisposable
    {
        public string name;

        public UObject()
        {
            name = "";
        }

        public UObject(string name)
        {
            this.name = name;
        }

        protected override void Disposed() 
        {
            base.Disposed();
        }
    }
}