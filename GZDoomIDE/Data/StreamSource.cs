#region ================== Copyright (C) 2018 Chronos Ouroboros

/*
 *  GZDoom IDE - An IDE for GZDoom modding/game-making.
 *  Copyright (C) 2018 Chronos Ouroboros
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
**/

#endregion

#region ================== Namespaces

using GZDoomIDE.Properties;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace GZDoomIDE.Data {
    public abstract class StreamSource : IDisposable {
        #region ================== Instance members

        /// <summary>
        /// The path to the source.
        /// </summary>
        public virtual string Path { get; protected set; }

        /// <summary>
        /// Is the stream disposed?
        /// </summary>
        public virtual bool IsDisposed { get; protected set; } = false;

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Gets the stream for this stream source.
        /// </summary>
        /// <returns>A Stream.</returns>
        public abstract Stream GetStream ();

        /// <summary>
        /// Disposes of the StreamSource.
        /// </summary>
        public abstract void Dispose ();

        #endregion
    }
    
    class FileSource : StreamSource {
        #region ================== Constructors

        /// <summary>
        /// Creates a new FileSource from the specified file path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public FileSource (string path) {
            if (path is null)
                throw new ArgumentNullException ("path");
            if (String.IsNullOrWhiteSpace (path))
                throw new ArgumentException ("Path cannot be empty or whitespace.", "path");

            Path = path;
        }

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Gets a stream for the file.
        /// </summary>
        /// <returns>A stream.</returns>
        public override Stream GetStream () {
            if (IsDisposed)
                throw new ObjectDisposedException ("FileSource");

            return new FileStream (Path, FileMode.Open);
        }

        public override void Dispose () {
            if (!IsDisposed) {
                Path = null;
                IsDisposed = true;
            }
        }

        #endregion
    }

    class ResourceSource : StreamSource {
        #region ================== Constructors

        /// <summary>
        /// Creates a new ResourceSource from the specified resource name.
        /// </summary>
        /// <param name="path">The resource's name.</param>
        public ResourceSource (string path) {
            if (path is null)
                throw new ArgumentNullException ("path");
            if (String.IsNullOrWhiteSpace (path))
                throw new ArgumentException ("Path cannot be empty or whitespace.", "path");

            Path = path;
        }

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Gets a stream to the resource.
        /// </summary>
        /// <returns>An UnmanagedMemoryStream.</returns>
        public override Stream GetStream () {
            if (IsDisposed)
                throw new ObjectDisposedException ("ResourceSource");

            return System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (Path);
        }

        public override void Dispose () {
            if (!IsDisposed) {
                Path = null;
                IsDisposed = true;
            }
        }

        #endregion
    }

    class ZipEntrySource : StreamSource {
        #region ================== Constructors

        /// <summary>
        /// Creates a new ZipFileSource from the specified zip file entry.
        /// </summary>
        /// <param name="zip">The zip the entry is in.</param>
        /// <param name="entry">The entry in the zip.</param>
        public ZipEntrySource (ZipFile zip, ZipEntry entry) {
            if (zip is null)
                throw new ArgumentNullException ("zip");

            if (entry is null)
                throw new ArgumentNullException ("entry");
            if (entry.IsDirectory)
                throw new ArgumentException ("Zip entry cannot be a directory", "entry");

            Zip = zip;
            Entry = entry;
        }

        #endregion

        #region ================== Instance members

        /// <summary>
        /// The entry's path.
        /// </summary>
        public override string Path {
            get { return Entry.Name; }
            protected set {
                throw new Exception ("Cannot set the path of a ZipEntrySource.");
            }
        }

        /// <summary>
        /// The Zip file the file stream is contained in.
        /// </summary>
        public ZipFile Zip { get; protected set; }

        public ZipEntry Entry { get; protected set; }

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Gets a stream to the resource.
        /// </summary>
        /// <returns>An UnmanagedMemoryStream.</returns>
        public override Stream GetStream () {
            if (IsDisposed)
                throw new ObjectDisposedException ("ZipFileSource");

            return Zip.GetInputStream (Entry);
        }

        public override void Dispose () {
            if (!IsDisposed) {
                Zip = null;
                Entry = null;
                IsDisposed = true;
            }
        }

        #endregion
    }
}
