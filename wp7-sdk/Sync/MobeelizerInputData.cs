using System;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using System.IO.IsolatedStorage;
using System.IO;

namespace Com.Mobeelizer.Mobile.Wp7.Sync
{
    public class MobeelizerInputData
    {
        public const String DATA_ENTRY_NAME = "data";

        public const String DELETED_FILES_ENTRY_NAME = "deletedFiles";

        private ZipFile zipFile;

        public MobeelizerInputData(Others.File inputFile)
        {
            MemoryStream tmpStream = new MemoryStream();
            using (IsolatedStorageFileStream stream = inputFile.OpenToRead())
            {
                stream.CopyTo(tmpStream);
            }
            this.zipFile = new ZipFile(tmpStream);
        }

        internal Stream GetFile(String guid)
        {
            ZipEntry fileEntry = zipFile.GetEntry(guid);
            if (fileEntry == null)
            {
                throw new FileNotFoundException("File '" + guid + "' not foud.");
            }
            return zipFile.GetInputStream(fileEntry);
        }

        internal IList<String> GetFiles()
        {
            List<String> result = new List<String>();
            foreach(ZipEntry entry in zipFile)
            {
                if(entry.Name != DATA_ENTRY_NAME && entry.Name != DELETED_FILES_ENTRY_NAME)
                {
                    result.Add(entry.Name);
                }
            }

            return result;
        }

        internal IEnumerable<MobeelizerJsonEntity> GetInputData()
        {
            return new MobeelizerInputDataEnumerable(this);
        }

        internal IList<String> GetDeletedFiles()
        {
            ZipEntry entry = zipFile.GetEntry(DELETED_FILES_ENTRY_NAME);

            if (entry == null)
            {
                throw new InvalidOperationException("Zip entry " + DELETED_FILES_ENTRY_NAME + " hasn't been found");
            }

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(zipFile.GetInputStream(entry));
                String line;
                List<String> lines = new List<String>();
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                return lines;
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
            finally
            {
                if (reader != null)
                {
                    try
                    {
                        reader.Close();
                    }
                    catch (IOException )
                    {
                        
                    }
                }
            }
        }

        internal void Close()
        {
            zipFile.Close();
        }
    
        public Stream GetDataInputStream()
        {
            ZipEntry entry = zipFile.GetEntry(DATA_ENTRY_NAME);
            if (entry == null)
            {
                throw new InvalidOperationException("Zip entry " + DATA_ENTRY_NAME + " hasn't been found");
            }

            try
            {
                return zipFile.GetInputStream(entry);
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }
    }
}
