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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#endregion

namespace GZDoomIDE.Data {
    public class WorkspaceData {
        #region ================== Instance members
        /// <summary>
        /// The workspace's name.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// The projects contained in this workspace.
        /// </summary>
        [JsonConverter (typeof(ProjectFilesConverter))]
        public ProjectData [] ProjectFiles { get; set; }
        #endregion

        #region ================== Instance methods
        /// <summary>
        /// Saves a ProjectData instance to the specified file.
        /// </summary>
        /// <param name="projPath">The file to save to.</param>
        /// <returns>Returns true if the data was saved successfully.</returns>
        public bool Save (string projPath) {
            return Save (projPath, this);
        }
        #endregion

        #region ================== Static members
        private static JsonSerializer serializer = JsonSerializer.Create (JsonConvert.DefaultSettings ());
        #endregion

        #region ================== Static methods
        /// <summary>
        /// Loads the data from the specified file.
        /// </summary>
        /// <param name="wspPath">The file to load.</param>
        /// <returns>A WorkspaceData contained the data that was read from the file</returns>
        public static WorkspaceData Load (string wspPath) {
            if (wspPath is null)
                throw new ArgumentNullException ("projPath");
            else if (String.IsNullOrWhiteSpace (wspPath))
                throw new ArgumentException ("Argument projPath cannot be empty or whitespace.", "projPath");

            using (var txtReader = new System.IO.StreamReader (wspPath, Encoding.UTF8)) {
                using (var jsonReader = new JsonTextReader (txtReader))
                    return serializer.Deserialize<WorkspaceData> (jsonReader);
            }
        }

        /// <summary>
        /// Saves the data to the specified file.
        /// </summary>
        /// <param name="wspPath">The file to save to.</param>
        /// <param name="data">The data to be saved.</param>
        /// <returns>Returns true if the data was saved successfully.</returns>
        public static bool Save (string wspPath, WorkspaceData data) {
            if (wspPath is null)
                throw new ArgumentNullException ("projPath");
            else if (String.IsNullOrWhiteSpace (wspPath))
                throw new ArgumentException ("Argument projPath cannot be empty or whitespace.", "projPath");

            if (data is null)
                throw new ArgumentNullException ("data");

            using (var txtWriter = new System.IO.StreamWriter (wspPath, false, Encoding.UTF8)) {
                using (var jsonWriter = new JsonTextWriter (txtWriter)) {
                    serializer.Serialize (jsonWriter, data);

                    return true;
                }
            }
        }
        #endregion
    }

    public class ProjectData {
        public enum LineEndings {
            Any = 0,
            CrLf,
            Lf,
            Cr,
        }

        #region ================== Instance members
        /// <summary>
        /// The project file is invalid.
        /// </summary>
        [JsonIgnore]
        public bool IsInvalid { get; internal set; } = false;

        /// <summary>
        /// The project's name.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// The project's type.
        /// </summary>
        public string Type { get; set; } = null;

        /// <summary>
        /// The path to the project file.
        /// </summary>
        [JsonIgnore]
        public string ProjectFilePath { get; internal set; } = null;

        /// <summary>
        /// The folder the project's files are contained in.
        /// </summary>
        public string SourcePath { get; set; } = null;

        /// <summary>
        /// The line endings used by the project.
        /// </summary>
        public LineEndings LineEnding { get; set; } = LineEndings.Any;
        #endregion

        #region ================== Instance methods
        /// <summary>
        /// Saves a ProjectData instance to the specified file.
        /// </summary>
        /// <param name="projPath">The file to save to.</param>
        /// <returns>Returns true if the data was saved successfully.</returns>
        public bool Save (string projPath) {
            return Save (projPath, this);
        }
        #endregion

        #region ================== Static members
        private static JsonSerializer serializer = JsonSerializer.Create (JsonConvert.DefaultSettings ());
        #endregion

        #region ================== Static methods
        /// <summary>
        /// Loads the data from the specified file.
        /// </summary>
        /// <param name="projPath">The file to load.</param>
        /// <returns>A ProjectData contained the data that was read from the file</returns>
        public static ProjectData Load (string projPath) {
            if (projPath is null)
                throw new ArgumentNullException ("projPath");
            else if (String.IsNullOrWhiteSpace (projPath))
                throw new ArgumentException ("Argument projPath cannot be empty or whitespace.", "projPath");

            ProjectData data;
            using (var txtReader = new System.IO.StreamReader (projPath, Encoding.UTF8)) {
                using (var jsonReader = new JsonTextReader (txtReader)) {
                    data = serializer.Deserialize<ProjectData> (jsonReader);
                }
            }

            data.IsInvalid = false;
            data.ProjectFilePath = projPath;

            return data;
        }

        /// <summary>
        /// Saves the data to the specified file.
        /// </summary>
        /// <param name="projPath">The file to save to.</param>
        /// <param name="data">The data to be saved.</param>
        /// <returns>Returns true if the data was saved successfully.</returns>
        public static bool Save (string projPath, ProjectData data) {
            if (projPath is null)
                throw new ArgumentNullException ("projPath");
            else if (String.IsNullOrWhiteSpace (projPath))
                throw new ArgumentException ("Argument projPath cannot be empty or whitespace.", "projPath");

            if (data is null)
                throw new ArgumentNullException ("data");

            using (var txtWriter = new System.IO.StreamWriter (projPath, false, Encoding.UTF8)) {
                using (var jsonWriter = new JsonTextWriter (txtWriter)) {
                    serializer.Serialize (jsonWriter, data);

                    return true;
                }
            }
        }
        #endregion
    }

    internal class ProjectFilesConverter : JsonConverter {
        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var projectPaths = ReadArrayData (reader);
            var data = new List<ProjectData> (projectPaths.Count);

            foreach (string path in projectPaths) {
                ProjectData projData;

                if (System.IO.File.Exists (path))
                    projData = ProjectData.Load (path);
                else {
                    projData = new ProjectData ();
                    projData.IsInvalid = true;
                    projData.ProjectFilePath = path;
                }

                data.Add (projData);
            }

            return data.ToArray ();
        }

        private List<string> ReadArrayData (JsonReader reader) {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonSerializationException (String.Format ("Unexpected token when reading projects array: {0}", CultureInfo.InvariantCulture, reader.TokenType));

            List<string> stringsArray = new List<string> ();

            while (reader.Read ()) {
                switch (reader.TokenType) {
                    case JsonToken.Integer:
                        stringsArray.Add (Convert.ToString (reader.Value, CultureInfo.InvariantCulture));
                        break;
                    case JsonToken.EndArray:
                        return stringsArray;
                    case JsonToken.Comment:
                        // skip
                        break;
                    default:
                        throw new JsonSerializationException (String.Format ("Unexpected token when reading projects array: {0}", CultureInfo.InvariantCulture, reader.TokenType));
                }
            }

            throw new JsonSerializationException ("Unexpected end when reading bytes.");
        }

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
            var workspace = value as WorkspaceData;

            writer.WriteStartArray ();

            foreach (ProjectData data in workspace.ProjectFiles)
                writer.WriteValue (data.SourcePath);

            writer.WriteEndArray ();
        }

        public override bool CanConvert (Type objectType) { return objectType == typeof (ProjectData []); }
    }
}
