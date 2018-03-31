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
        public string Path { get; protected set; }

        /// <summary>
        /// Is the stream disposed?
        /// </summary>
        public bool IsDisposed { get; protected set; }

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
                throw new ObjectDisposedException ("FileSource");

            return Resources.ResourceManager.GetStream (Path);
        }

        public override void Dispose () {
            if (!IsDisposed) {
                Path = null;
                IsDisposed = true;
            }
        }

        #endregion
    }
}
