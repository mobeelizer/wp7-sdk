using System.Collections.Generic;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    public class MobeelizerTableEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> enumerator;

        internal MobeelizerTableEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public T Current
        {
            get 
            {
                return this.enumerator.Current;
            }
        }

        public void Dispose()
        {
            this.enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get 
            {
                return this.enumerator.Current; 
            }
        }

        public bool MoveNext()
        {
            return this.enumerator.MoveNext();
        }

        public void Reset()
        {
            this.enumerator.Reset();
        }
    }
}
