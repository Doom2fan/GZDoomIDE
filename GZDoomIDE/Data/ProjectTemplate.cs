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

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;

#endregion

namespace GZDoomIDE.Data {
    /// <summary>
    /// Thrown by template classes.
    /// </summary>
    public class IDETemplateException : Exception {
        public IDETemplateException () : base () { }
        public IDETemplateException (string message) : base (message) { }
        public IDETemplateException (string message, Exception innerException) : base (message, innerException) { }
    }

    public class ProjectTemplate {
        public enum TemplateType {
            Workspace = 0,
            Project,
        };

        #region ================== Constructors

        /// <summary>
        /// Creates a new ProjectTemplate.
        /// </summary>
        public ProjectTemplate () { }

        #endregion

        #region ================== Instance members

        /// <summary>
        /// The type of template.
        /// </summary>
        [JsonConverter (typeof (StringEnumConverter))]
        public TemplateType Type { get; set; }
        /// <summary>
        /// The template's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The template's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The files that need to be preprocessed
        /// </summary>
        [JsonProperty ("Preprocess")]
        public string [] FilesToPreprocess { get; set; }

        /// <summary>
        /// The path to the Zip file that contains this template.
        /// </summary>
        [JsonIgnore]
        public StreamSource ZipSource { get; set; }

        #endregion

        #region ================== Static methods

        /// <summary>
        /// Reads a template from a file.
        /// </summary>
        /// <param name="filePath">The file to load the template from.</param>
        /// <returns>A ProjectTemplate.</returns>
        public static ProjectTemplate FromFile (string filePath) {
            if (filePath is null)
                throw new ArgumentNullException ("filePath");
            if (!File.Exists (filePath))
                throw new ArgumentException ("The specified zip file does not exist", "filePath");

            using (var src = new FileSource (filePath))
                return FromStreamSource (src);
        }

        /// <summary>
        /// Reads a template from a StreamSource.
        /// </summary>
        /// <param name="streamSource">The StreamSource to load the template from.</param>
        /// <returns>A ProjectTemplate.</returns>
        public static ProjectTemplate FromStreamSource (StreamSource streamSource) {
            ProjectTemplate ret = null;

            using (var stream = streamSource.GetStream ()) {
                try {
                    using (var zipFile = new ZipFile (stream)) {
                        if (!zipFile.TestArchive (true, TestStrategy.FindFirstError, null))
                            throw new IDETemplateException ("The zip file contained in the specified StreamSource is damaged.");

                        var infoEntry = zipFile.GetEntry ("info.json");

                        if (infoEntry is null)
                            throw new IDETemplateException ("The specified zip file is not a project template.");

                        var serializer = JsonSerializer.Create ();
                        using (var reader = new JsonTextReader (new StreamReader (zipFile.GetInputStream (infoEntry)))) {
                            ret = serializer.Deserialize<ProjectTemplate> (reader);

                            reader.CloseInput = true;
                            reader.Close ();
                        }

                        ret.ZipSource = streamSource;

                        zipFile.Close ();
                    }
                } catch (Exception) {
                    throw;
                }
            }

            return ret;
        }

        #endregion
    }
}
