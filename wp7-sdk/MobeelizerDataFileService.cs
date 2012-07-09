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
using System.IO.IsolatedStorage;
using System.IO;

namespace Com.Mobeelizer.Mobile.Wp7
{
    public class MobeelizerDataFileService
    {
        private MobeelizerApplication application;

        public MobeelizerDataFileService(MobeelizerApplication application)
        {
            // TODO: Complete member initialization
            this.application = application;
        }

        internal bool PrepareOutputFile(IsolatedStorageFileStream outputFile)
        {
            MemoryStream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write("cos tam cos");
                writer.Flush();

                stream.CopyTo(outputFile);
            }
            return true;
           // throw new NotImplementedException();
        }

        internal bool ProcessInputFile(IsolatedStorageFileStream inputFile, bool isAllSynchronization)
        {
            // TODO
            return true;
        }
    }
}
