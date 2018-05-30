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

using com.calitha.goldparser;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace GZDoomIDE.Editor {
    public class ScintillaTheme {
        public class ColorConverter : JsonConverter {
            public override bool CanConvert (Type objectType) { return objectType == typeof (Color?); }

            public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                switch (reader.TokenType) {
                    case JsonToken.Integer:
                        return new Color? (Color.FromArgb ((int) (((long) reader.Value) & 0xFFFFFFFF)));
                    case JsonToken.String: {
                            var str = (reader.Value as string);

                            if (!str.StartsWith ("#"))
                                return new Color? (Color.FromName ((string) reader.Value));
                            else
                                return Utils.ColorFromHex (str);
                        }
                    case JsonToken.Null:
                        return null;

                    default:
                        throw new Exception ();
                }
            }

            public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
                throw new NotImplementedException ();
            }
        }

        public class BasicStyle {
            [JsonConverter (typeof (ColorConverter))]
            public Color? Background { get; set; } = null;
            [JsonConverter (typeof (ColorConverter))]
            public Color? Foreground { get; set; } = null;

            public bool Bold { get; set; } = false;
            public bool Italic { get; set; } = false;
            public bool Underline { get; set; } = false;

            public BasicStyle (Color? bg, Color? fg, bool bold = false, bool italic = false, bool underline = false) {
                Background = bg;
                Foreground = fg;

                Bold = bold;
                Italic = italic;
                Underline = underline;
            }

            public void SetScintillaStyle (ScintillaNET.Style style) {
                style.BackColor = Background.Value;
                style.ForeColor = Foreground.Value;

                style.Bold = Bold;
                style.Italic = Italic;
                style.Underline = Underline;
            }
        }

        [JsonRequired]
        public string Name { get; set; }

        [JsonRequired]
        public BasicStyle Default { get; set; }
        public BasicStyle Whitespace { get; set; } = null;
        public BasicStyle Number { get; set; } = new BasicStyle (null, Color.FromArgb (-4862296));

        public BasicStyle Selection { get; set; } = null;
        public BasicStyle Caret { get; set; } = new BasicStyle (null, null);
        public BasicStyle AdditionalCarets { get; set; } = new BasicStyle (null, null);

        [JsonConverter (typeof (ColorConverter))]
        public Color? SelectedLineBack { get; set; } = null;
        public int    SelectedLineAlpha { get; set; } = 255;

        public BasicStyle LineNumber { get; set; } = null;
        public BasicStyle IndentGuide { get; set; } = null;
        public BasicStyle FoldDisplayText { get; set; } = null;

        public BasicStyle FoldMargin { get; set; } = new BasicStyle (SystemColors.ControlDarkDark, SystemColors.ControlLight);
        public BasicStyle FoldMarker { get; set; } = new BasicStyle (SystemColors.ControlDarkDark, SystemColors.ControlLight);

        [JsonConverter (typeof (StringEnumConverter))]
        public MarkerSymbol FoldMarkerFold { get; set; } = MarkerSymbol.BoxMinus;
        [JsonConverter (typeof (StringEnumConverter))]
        public MarkerSymbol FoldMarkerUnfold { get; set; } = MarkerSymbol.BoxPlus;
        [JsonConverter (typeof (StringEnumConverter))]
        public MarkerSymbol FoldMarkerEnd { get; set; } = MarkerSymbol.LCorner;

        [JsonConverter (typeof (StringEnumConverter))]
        public MarkerSymbol FoldMarkerNestedFold { get; set; } = MarkerSymbol.BoxMinusConnected;
        [JsonConverter (typeof (StringEnumConverter))]
        public MarkerSymbol FoldMarkerNestedUnfold { get; set; } = MarkerSymbol.BoxPlusConnected;
        [JsonConverter (typeof (StringEnumConverter))]
        public MarkerSymbol FoldMarkerNestedEnd { get; set; } = MarkerSymbol.TCorner;

        [JsonConverter (typeof (StringEnumConverter))]
        public MarkerSymbol FoldMarkerLine { get; set; } = MarkerSymbol.VLine;

        public BasicStyle BraceLight { get; set; } = new BasicStyle (null, null);
        public BasicStyle BraceBad { get; set; } = new BasicStyle (null, Color.Red);

        public BasicStyle CommentLine { get; set; } = new BasicStyle (null, Color.FromArgb (-2712187));
        public BasicStyle CommentBlock { get; set; } = new BasicStyle (null, Color.FromArgb (-2712187));
        public BasicStyle CommentDocLine { get; set; } = new BasicStyle (null, Color.Red);
        public BasicStyle CommentDocBlock { get; set; } = new BasicStyle (null, Color.Red);

        public BasicStyle Literal { get; set; } = new BasicStyle (null, Color.FromArgb (-4862296));
        public BasicStyle Constant { get; set; } = new BasicStyle (null, Color.FromArgb (-4862296));

        public BasicStyle Keyword { get; set; } = new BasicStyle (null, Color.FromArgb (-11100970));
        public BasicStyle Identifier { get; set; } = new BasicStyle (null, null);
        public BasicStyle Operator { get; set; } = new BasicStyle (null, null);

        public BasicStyle String { get; set; } = new BasicStyle (null, Color.FromArgb (-2712187));
        public BasicStyle StringVerbatim { get; set; } = new BasicStyle (null, Color.FromArgb (-2712187));
        public BasicStyle Character { get; set; } = new BasicStyle (null, Color.FromArgb (-2712187));
        public BasicStyle Regex { get; set; } = new BasicStyle (null, Color.FromArgb (-6663372));

        public BasicStyle Preprocessor { get; set; } = new BasicStyle (null, Color.FromArgb (-6579301));
        public BasicStyle Label { get; set; } = new BasicStyle (null, Color.FromArgb (-11100970));

        public BasicStyle Uri { get; set; } = new BasicStyle (null, Color.FromArgb (-11100970));
        public BasicStyle EscapeSequence { get; set; } = new BasicStyle (null, Color.Red);

        public static ScintillaTheme Load (string filePath) {
            if (filePath is null)
                throw new ArgumentNullException ("filePath");
            else if (string.IsNullOrWhiteSpace (filePath))
                throw new ArgumentException ("Argument projPath cannot be empty or whitespace.", "filePath");
            if (!File.Exists (filePath))
                throw new FileNotFoundException ("Could not find the specified theme file.", filePath);

            using (var stream = new FileStream (filePath, FileMode.Open))
                return Load (stream);
        }

        public static ScintillaTheme Load (Stream stream) {
            ScintillaTheme data;

            using (var txtReader = new StreamReader (stream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader (txtReader))
                data = Program.Data.JsonSerializer.Deserialize<ScintillaTheme> (jsonReader);

            if (data.Default == null ||
                data.Default.Background == null || data.Default.Foreground == null ||
                !data.Default.Background.HasValue || !data.Default.Foreground.HasValue)
                throw new Exception ("Default colors must be present!");

            if (data != null) {
                var props = typeof (ScintillaTheme).GetProperties ();

                foreach (var prop in props) {
                    if (prop.PropertyType != typeof (BasicStyle))
                        continue;

                    var pal = (prop.GetValue (data) as BasicStyle);

                    if (pal == null)
                        continue;

                    if (pal.Background == null || !pal.Background.HasValue)
                        pal.Background = data.Default.Background;

                    if (pal.Foreground == null || !pal.Foreground.HasValue)
                        pal.Foreground = data.Default.Foreground;
                }
            }

            return data;
        }
    }


}
