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
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

#endregion

namespace GZDoomIDE.Data {
    public class WorkspaceData {
        #region ================== Instance members

        /// <summary>
        /// The path to the workspace file.
        /// </summary>
        [JsonIgnore]
        public string WorkspaceFilePath { get; internal set; } = null;

        /// <summary>
        /// The workspace's name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = null;

        /// <summary>
        /// The projects contained in this workspace.
        /// </summary>
        [JsonConverter (typeof (ProjectFilesConverter))]
        public ObservableCollection<ProjectData> ProjectFiles { get; set; } = new ObservableCollection<ProjectData> ();

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Saves the ProjectData instance.
        /// </summary>
        /// <returns>Returns true if the data was saved successfully.</returns>
        public bool Save () {
            return Save (WorkspaceFilePath, this);
        }

        /// <summary>
        /// Saves the ProjectData instance to the specified file.
        /// </summary>
        /// <param name="wspPath">The file to save to.</param>
        /// <returns>Returns true if the data was saved successfully.</returns>
        public bool Save (string wspPath) {
            return Save (wspPath, this);
        }

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

            using (var txtReader = new System.IO.StreamReader (wspPath, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader (txtReader)) {
                var ret = Program.Data.JsonSerializer.Deserialize<WorkspaceData> (jsonReader);
                ret.WorkspaceFilePath = wspPath;
                return ret;
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
                    Program.Data.JsonSerializer.Serialize (jsonWriter, data);

                    return true;
                }
            }
        }

        #endregion
    }

    public class ProjectData {
        [JsonConverter (typeof (StringEnumConverter))]
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
        /// The project file is loaded.
        /// </summary>
        [JsonIgnore]
        public bool IsLoaded { get; internal set; } = false;

        /// <summary>
        /// The project's name.
        /// </summary>
        [JsonRequired]
        public string Name { get; set; } = null;

        /// <summary>
        /// The project's type.
        /// </summary>
        [JsonRequired]
        [JsonConverter (typeof (ProjectTypeConverter))]
        public ProjectType Type { get; set; } = null;

        /// <summary>
        /// The path to the project file.
        /// </summary>
        [JsonIgnore]
        public string ProjectFilePath { get; internal set; } = null;

        /// <summary>
        /// The folder the project's files are contained in.
        /// </summary>
        [JsonRequired]
        public string SourcePath { get; set; } = null;

        /// <summary>
        /// The line endings used by the project.
        /// </summary>
        public LineEndings LineEnding { get; set; } = LineEndings.Any;

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Saves the ProjectData instance to the specified file.
        /// </summary>
        /// <param name="projPath">The file to save to.</param>
        /// <returns>Returns true if the data was saved successfully.</returns>
        public bool Save (string projPath) {
            if (IsInvalid)
                throw new Exception ("The project data is invalid.");
            else if (!IsLoaded)
                throw new Exception ("The project data is not loaded.");

            return Save (projPath, this);
        }

        /// <summary>
        /// Copies the values from another instance of ProjectData.
        /// </summary>
        /// <param name="other">The instance to copy from.</param>
        public void Copy (ProjectData other) {
            var properties = typeof (ProjectData).GetProperties ();
            foreach (var prop in properties) {
                prop.SetValue (this, prop.GetValue (other));
            }

            var fields = typeof (ProjectData).GetFields ();
            foreach (var field in fields) {
                field.SetValue (this, field.GetValue (other));
            }
        }

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
                    data = Program.Data.JsonSerializer.Deserialize<ProjectData> (jsonReader);
                }
            }

            data.IsInvalid = false;
            data.IsLoaded = true;
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

            if (data.IsInvalid)
                throw new Exception ("The project data is invalid.");
            else if (!data.IsLoaded)
                throw new Exception ("The project data is not loaded.");

            if (data is null)
                throw new ArgumentNullException ("data");

            using (var txtWriter = new System.IO.StreamWriter (projPath, false, Encoding.UTF8)) {
                using (var jsonWriter = new JsonTextWriter (txtWriter)) {
                    Program.Data.JsonSerializer.Serialize (jsonWriter, data);

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
            var data = new ObservableCollection<ProjectData> ();

            foreach (string path in projectPaths) {
                ProjectData projData;

                projData = new ProjectData ();
                projData.IsInvalid = false;
                projData.IsLoaded = false;
                projData.ProjectFilePath = path;

                data.Add (projData);
            }

            return data;
        }

        private List<string> ReadArrayData (JsonReader reader) {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonSerializationException (String.Format ("Unexpected token when reading projects array: {0}", CultureInfo.InvariantCulture, reader.TokenType));

            List<string> stringsArray = new List<string> ();

            while (reader.Read ()) {
                switch (reader.TokenType) {
                    case JsonToken.String:
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
            var projects = (value as ObservableCollection<ProjectData>);

            writer.WriteStartArray ();

            foreach (ProjectData data in projects)
                writer.WriteValue (data.ProjectFilePath);

            writer.WriteEndArray ();
        }

        public override bool CanConvert (Type objectType) { return objectType == typeof (ObservableCollection<ProjectData>); }
    }

    internal class ProjectTypeConverter : JsonConverter {
        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType != JsonToken.String)
                throw new JsonSerializationException (String.Format ("Unexpected token when reading project type: {0}", CultureInfo.InvariantCulture, reader.TokenType));

            string key = (string) reader.Value;

            if (Program.Data.ProjectTypes.ContainsKey (key))
                return Activator.CreateInstance (Program.Data.ProjectTypes [key]);
            else
                return new UnknownProjectType (key);
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
            string typeName;

            if (value.GetType () == typeof (UnknownProjectType))
                typeName = (value as UnknownProjectType).TypeName;
            else {
                var projType = value.GetType ();

                typeName = Program.Data.ProjectTypes.First (kvp => kvp.Value == projType).Key;
            }

            writer.WriteValue (typeName);
        }

        public override bool CanConvert (Type objectType) { return objectType == typeof (ProjectType); }
    }
}
