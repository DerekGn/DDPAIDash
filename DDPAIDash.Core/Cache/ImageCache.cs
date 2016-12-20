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
using Windows.Storage;
using Windows.Storage.FileProperties;
using SharpCompress.Readers;

namespace DDPAIDash.Core.Cache
{
#warning TODO make async
    internal class ImageCache : IImageCache
    {
        public void Cache(Stream stream)
        {
            using (var reader = ReaderFactory.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    if (reader.Entry.IsDirectory)
                    {
                        ApplicationData.Current.LocalCacheFolder.CreateFolderAsync(reader.Entry.Key).AsTask();
                    }
                    else
                    {
                        var newFile =
                            ApplicationData.Current.LocalCacheFolder.CreateFileAsync(reader.Entry.Key.Replace('/', '\\'))
                                .AsTask()
                                .Result;

                        using (var outStream = newFile.OpenStreamForWriteAsync().Result)
                        {
                            reader.WriteEntryTo(outStream);
                        }
                    }
                }
            }
        }

        public void Cache(string imageFileName, Stream stream)
        {
            var newFile = ApplicationData.Current.LocalCacheFolder.CreateFileAsync(imageFileName).AsTask().Result;

            using (var outStream = newFile.OpenStreamForWriteAsync().Result)
            {
                stream.CopyTo(outStream);
            }
        }

        public bool Contains(string name)
        {
            return
                ApplicationData.Current.LocalCacheFolder.GetItemsAsync()
                    .AsTask()
                    .Result.FirstOrDefault(i => i.Name == name) != null;
        }

        public void Flush(TimeSpan olderThan)
        {
            var files = ApplicationData.Current.LocalCacheFolder.GetFilesAsync().AsTask().Result;

            foreach (var storageFile in files)
            {
                if (DateTime.Now.Subtract(storageFile.DateCreated.UtcDateTime) >= olderThan)
                {
                    storageFile.DeleteAsync().AsTask();
                }
            }

            var folders = ApplicationData.Current.LocalCacheFolder.GetFoldersAsync().AsTask().Result;

            foreach (var folder in folders)
            {
                if (DateTime.Now.Subtract(folder.DateCreated.UtcDateTime) >= olderThan)
                {
                    folder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask();
                }
            }
        }

        public Stream Get(string name)
        {
            var folder =
                ApplicationData.Current.LocalCacheFolder.GetFoldersAsync()
                    .AsTask()
                    .Result.FirstOrDefault(f => f.Name == name);
            StorageFile file;

            if (folder != null)
            {
                file = folder.GetFilesAsync().AsTask().Result.First();
            }
            else
            {
                file = ApplicationData.Current.LocalCacheFolder.GetFileAsync(name).AsTask().Result;

                if (file == null)
                    throw new FileNotFoundException("File not found in cache", name);
            }

            return file.OpenStreamForReadAsync().Result;
        }

        public Stream GetThumbnailStream(string name)
        {
            var folder =
                ApplicationData.Current.LocalCacheFolder.GetFoldersAsync()
                    .AsTask()
                    .Result.FirstOrDefault(f => f.Name == name);
            StorageFile file;

            if (folder != null)
            {
                file = folder.GetFilesAsync().AsTask().Result.First();
            }
            else
            {
                file = ApplicationData.Current.LocalCacheFolder.GetFileAsync(name).AsTask().Result;

                if (file == null)
                    throw new FileNotFoundException("File not found in cache", name);
            }

            return file.GetThumbnailAsync(ThumbnailMode.ListView).AsTask().Result.AsStream();
        }
    }
}