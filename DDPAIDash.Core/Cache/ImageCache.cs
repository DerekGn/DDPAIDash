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
using Windows.Storage;
using SharpCompress.Readers;
using System.Linq;

namespace DDPAIDash.Core.Cache
{
    internal class ImageCache : IImageCache
    {
        public void Cache(Stream stream)
        {
            using (var reader = ReaderFactory.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    if(reader.Entry.IsDirectory)
                    {
                        StorageFolder newfolder = ApplicationData.Current.LocalFolder.CreateFolderAsync(reader.Entry.Key).AsTask().Result;
                    }
                    else
                    {
                        StorageFile newFile = ApplicationData.Current.LocalFolder.CreateFileAsync(reader.Entry.Key.Replace('/', '\\')).AsTask().Result;

                        using (Stream outStream = newFile.OpenStreamForWriteAsync().Result)
                        {
                            reader.WriteEntryTo(outStream);
                        }
                    }
                }
            }
        }

        public void Cache(string imageFileName, Stream stream)
        {
            StorageFile newFile = ApplicationData.Current.LocalFolder.CreateFileAsync(imageFileName).AsTask().Result;

            using (Stream outStream = newFile.OpenStreamForWriteAsync().Result)
            {
                stream.CopyTo(outStream);
            }
        }

        public bool Contains(string name)
        {
#warning TOASYNC
            return ApplicationData.Current.LocalFolder.GetItemsAsync().AsTask().Result.FirstOrDefault(i => i.Name == name) != null;
        }

        public Stream Get(string name)
        {
            var folder = ApplicationData.Current.LocalFolder.GetFoldersAsync().AsTask().Result.FirstOrDefault(f => f.Name == name);
            StorageFile file;

            if (folder != null)
            {
                file = folder.GetFilesAsync().AsTask().Result.First();
            }
            else
            {
                file = ApplicationData.Current.LocalFolder.GetFileAsync(name).AsTask().Result;

                if (file == null)
                    throw new FileNotFoundException("File not found in cache", name);
            }

            return file.OpenStreamForReadAsync().Result;
        }
        
        public Stream GetThumbnailStream(string name)
        {
            var folder = ApplicationData.Current.LocalFolder.GetFoldersAsync().AsTask().Result.FirstOrDefault(f => f.Name == name);
            StorageFile file;

            if (folder != null)
            {
                file = folder.GetFilesAsync().AsTask().Result.First();
            }
            else
            {
                file = ApplicationData.Current.LocalFolder.GetFileAsync(name).AsTask().Result;

                if (file == null)
                    throw new FileNotFoundException("File not found in cache", name);
            }

            return file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.ListView).AsTask().Result.AsStream();
        }
    }
}
