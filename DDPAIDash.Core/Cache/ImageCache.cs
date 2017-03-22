/**
* MIT License
*
* Copyright (c) 2016 Derek Goslin < http://corememorydump.blogspot.ie/ >
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using SharpCompress.Readers;

namespace DDPAIDash.Core.Cache
{
    internal class ImageCache : IImageCache
    {
        public async Task CacheAsync(Stream stream)
        {
            using (var reader = ReaderFactory.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    if (reader.Entry.IsDirectory)
                    {
                        await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(reader.Entry.Key);
                    }
                    else
                    {
                        var newFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(reader.Entry.Key.Replace('/', '\\'));

                        using (var outStream = await newFile.OpenStreamForWriteAsync())
                        {
                            reader.WriteEntryTo(outStream);
                        }
                    }
                }
            }
        }

        public async Task CacheAsync(string imageFileName, Stream stream)
        {
            var newFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(imageFileName);

            using (var outStream = await newFile.OpenStreamForWriteAsync())
            {
                stream.CopyTo(outStream);
            }
        }

        public async Task<bool> ContainsAsync(string name)
        {
            bool result = false;

            try
            {
                await ApplicationData.Current.TemporaryFolder.GetItemAsync(name);
                result = true;
            }
            catch (FileNotFoundException)
            {
            }

            return result;
        }

        public async Task FlushAsync(TimeSpan olderThan)
        {
            foreach (var storageFile in await ApplicationData.Current.TemporaryFolder.GetFilesAsync())
            {
                if (DateTime.Now.Subtract(storageFile.DateCreated.UtcDateTime) >= olderThan)
                {
                    await storageFile.DeleteAsync();
                }
            }

            foreach (var folder in await ApplicationData.Current.TemporaryFolder.GetFoldersAsync())
            {
                if (DateTime.Now.Subtract(folder.DateCreated.UtcDateTime) >= olderThan)
                {
                    await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
        }

        public async Task<Stream> GetThumbnailStreamAsync(string name)
        {
            var storageItem = await ApplicationData.Current.TemporaryFolder.GetItemAsync(name);
            
            StorageFile file;

            if (storageItem.IsOfType(StorageItemTypes.Folder))
            {
                file = (await ((StorageFolder)storageItem).GetFilesAsync()).FirstOrDefault();
            }
            else
            {
                file = (StorageFile) storageItem;
            }
            
            return file != null ? (await file.GetThumbnailAsync(ThumbnailMode.ListView)).AsStream() : null;
        }
    }
}