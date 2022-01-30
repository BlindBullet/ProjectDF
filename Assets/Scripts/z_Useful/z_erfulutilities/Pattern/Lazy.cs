using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUtils
{
    public class Lazy<T>
    {
        private T _value;
        public bool IsOudated { get; private set; }
        public T Value
        {
            get
            {
                IsOudated = false;
                if (UpdateCalled != null)
                    UpdateCalled(this);
                return _value;
            }
            set
            {
                _value = value;
                IsOudated = true;
            }
        }

        public Lazy(T value, Action<Lazy<T>> updatedCalled)
        {
            this._value = value;
            this.UpdateCalled = updatedCalled;
        }

        protected Action<Lazy<T>> UpdateCalled;
    }
}
