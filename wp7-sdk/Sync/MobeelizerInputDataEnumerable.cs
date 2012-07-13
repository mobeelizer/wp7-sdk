using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace Com.Mobeelizer.Mobile.Wp7.Sync
{
    public class MobeelizerInputDataEnumerable : IEnumerable<MobeelizerJsonEntity>
    {
        private MobeelizerInputDataEnumerator enumerator;

        public MobeelizerInputDataEnumerable(MobeelizerInputData mobeelizerInputData)
        {
            this.enumerator = new MobeelizerInputDataEnumerator(mobeelizerInputData.GetDataInputStream());
        }

        public IEnumerator<MobeelizerJsonEntity> GetEnumerator()
        {
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return enumerator;
        }
    }

    public class MobeelizerInputDataEnumerator : IEnumerator<MobeelizerJsonEntity>
    {
        private MobeelizerJsonEntity current;

        private StreamReader stream;

        public MobeelizerInputDataEnumerator(System.IO.Stream stream)
        {
            this.stream = new StreamReader(stream);
        }

        public MobeelizerJsonEntity Current
        {
            get 
            {
                return current;
            }
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        object IEnumerator.Current
        {
            get 
            {
                return this.Current;
            }
        }

        public bool MoveNext()
        {
            try
            {
                String cur = stream.ReadLine();
                if (String.IsNullOrWhiteSpace(cur))
                {
                    return false;
                }

                this.current = new MobeelizerJsonEntity(cur);
                return true;
            }
            catch (IOException) { }
            catch (OutOfMemoryException) { }
            catch (InvalidOperationException) { }
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
