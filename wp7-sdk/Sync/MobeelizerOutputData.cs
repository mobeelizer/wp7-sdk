using System;
using System.IO.IsolatedStorage;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7.Sync
{
    internal class MobeelizerOutputData
    {
        Others.File dataFile;

        StreamWriter outputStream;

        ZipOutputStream zip;

        private List<String> deletedFiles;

        internal MobeelizerOutputData(Others.File file, Others.File tmpFile)
        {
            this.dataFile = tmpFile;
            outputStream = new StreamWriter(tmpFile.OpenToWrite());
            zip = new ZipOutputStream(file.OpenToWrite());
            zip.UseZip64 = UseZip64.Off;
            deletedFiles = new List<String>();
        }

        internal void WriteEntity(MobeelizerJsonEntity next)
        {
            try
            {
                outputStream.Write(next.GetJson());
                outputStream.Write('\n');
            }
            catch (IOException e)
            {
                outputStream.Close();
                throw new InvalidOperationException(e.Message, e);
            }
        }

        internal void WriteFile(string guid, Stream stream)
        {
            try
            {
                zip.PutNextEntry(new ZipEntry(guid));
                stream.CopyTo(zip);
                zip.CloseEntry();
            }
            catch (IOException e)
            {
                zip.Close();
                outputStream.Close();
                throw new InvalidOperationException(e.Message, e);
            }
        }

        internal void Close()
        {
            Stream dataInputStream = null;
            try
            {
                this.outputStream.Close();
                zip.PutNextEntry(new ZipEntry(MobeelizerInputData.DATA_ENTRY_NAME));
                dataInputStream = dataFile.OpenToRead();
                dataInputStream.CopyTo(zip);
                dataInputStream.Close();
                zip.CloseEntry();
                zip.PutNextEntry(new ZipEntry(MobeelizerInputData.DELETED_FILES_ENTRY_NAME));
                WriteLines(deletedFiles, "\n", zip);
                zip.CloseEntry();
                zip.Close();
            }
            catch (IOException e)
            {
                zip.Close();
                dataInputStream.Close();
                outputStream.Close();
                throw new InvalidOperationException(e.Message, e);
            }
        }

        private void WriteLines(IList<String> lines, String lineEnding, Stream os)
        {
            if (lines == null)
            {
                return;
            }

            foreach (String line in lines)
            {
                if (line != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(line);
                    os.Write(bytes, 0, bytes.Length);
                }

                byte[] endBytes = Encoding.UTF8.GetBytes(lineEnding);
                os.Write(endBytes, 0, endBytes.Length);
            }
        }
    }
}
