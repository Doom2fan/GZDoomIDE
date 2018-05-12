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
using System.Drawing;

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
        #region ================== Constructors/Destructors

        /// <summary>
        /// Creates a new ProjectTemplate.
        /// </summary>
        public ProjectTemplate () { }

        ~ProjectTemplate () {
            if (!(IconPath is null) && !(IconImage is null)) {
                IconImage.Dispose ();
                IconImage = null;
            }

            if (!(ZipSource is null)) {
                ZipSource.Dispose ();
                ZipSource = null;
            }
        }

        #endregion

        #region ================== Instance members

        /// <summary>
        /// The template's name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; }
        /// <summary>
        /// The template's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The project file.
        /// </summary>
        [JsonRequired]
        public string ProjectFile { get; set; }
        /// <summary>
        /// The default project name.
        /// </summary>
        public string DefaultProjectName { get; set; } = "Project";

        /// <summary>
        /// The files that need to be preprocessed
        /// </summary>
        [JsonProperty ("Preprocess")]
        public string [] FilesToPreprocess { get; set; }

        /// <summary>
        /// The path to the template's icon.
        /// </summary>
        [JsonProperty ("Icon")]
        public string IconPath { get; set; }

        /// <summary>
        /// The path to the Zip file that contains this template.
        /// </summary>
        [JsonIgnore]
        public StreamSource ZipSource { get; set; }

        /// <summary>
        /// The template's icon.
        /// </summary>
        [JsonIgnore]
        public Image IconImage { get; set; }

        #endregion

        #region ================== Instance methods

        public bool CreateProject (string path, string projectName, out ProjectData projData) {
            if (path is null)
                throw new ArgumentNullException ("path");
            else if (String.IsNullOrWhiteSpace (path))
                throw new ArgumentException ("Path cannot be empty or whitespace.", "path");

            if (!Directory.Exists (path))
                Directory.CreateDirectory (path);

            string projPath = Path.Combine (path, @".\" + projectName);
            if (Directory.Exists (projPath))
                throw new Exception ("A project with the specified name already exists in the specified path.");

            PreprocessorMacro [] macros = new PreprocessorMacro [] {
                PreprocessorMacro.AsToken ('$', "ProjectName", projectName),
                PreprocessorMacro.AsToken ('$', "IDEVersion", Constants.Version.ToString ()),
            };

            using (var zipStream = ZipSource.GetStream ())
            using (var zip = new ZipFile (zipStream)) {
                string templateFolder = ContentsFolderName + "/";

                var folders = new List<ZipEntry> ();
                var files = new List<ZipEntry> ();

                foreach (ZipEntry entry in zip) {
                    if (!entry.Name.StartsWith (templateFolder))
                        continue;

                    if (entry.IsDirectory)
                        folders.Add (entry);
                    else if (entry.IsFile)
                        files.Add (entry);
                }

                foreach (var entry in folders) {
                    if (!entry.Name.StartsWith (templateFolder) || !entry.IsDirectory)
                        continue;

                    string filePath = entry.Name.Substring (templateFolder.Length);
                    Directory.CreateDirectory (Path.Combine (path, Utils.Preprocess (filePath, macros)));
                }

                foreach (var entry in files) {
                    if (!entry.Name.StartsWith (templateFolder) || !entry.IsFile)
                        continue;

                    string filePath = entry.Name.Substring (templateFolder.Length);
                    Stream fileContents = null;

                    try {
                        if (FilesToPreprocess.Contains (filePath)) {
                            byte [] fileBytes;

                            using (var zs = zip.GetInputStream (entry)) {
                                fileBytes = new byte [entry.Size];

                                for (int i = 0; i < fileBytes.Length; i++)
                                    fileBytes [i] = 0;
                                
                                zs.Read (fileBytes, 0, (int) entry.Size);

                                fileBytes = Encoding.UTF8.GetBytes (Utils.Preprocess (Encoding.UTF8.GetString (fileBytes), macros));
                            }
                            
                            fileContents = new MemoryStream (fileBytes, false);
                        } else
                            fileContents = zip.GetInputStream (entry);

                        using (var fs = File.Create (Path.Combine (path, Utils.Preprocess (filePath, macros)))) {
                            fileContents.Seek (0, SeekOrigin.Begin);
                            fileContents.CopyTo (fs);
                        }
                    } finally {
                        if (!(fileContents is null)) {
                            fileContents.Dispose ();
                            fileContents = null;
                        }
                    }
                }
            }

            if ((projData = ProjectData.Load (Path.Combine (path, Utils.Preprocess (ProjectFile, macros)))) is null)
                return false;

            return true;
        }

        #endregion

        #region ================== Static members

        public const string ContentsFolderName = @"template";

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
            
            return FromStreamSource (new FileSource (filePath));
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
                        if (!(ret.IconPath is null)) {
                            if (ret.IconPath.StartsWith ("@")) {
                                ret.IconImage = (Bitmap) Properties.Resources.ResourceManager.GetObject (ret.IconPath.Substring (1));
                            } else {
                                var entry = zipFile.GetEntry (ret.IconPath);

                                if (!(entry is null)) {
                                    using (var iconStream = zipFile.GetInputStream (entry)) {
                                        ret.IconImage = new Bitmap (iconStream);
                                    }
                                }
                            }
                        }

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
