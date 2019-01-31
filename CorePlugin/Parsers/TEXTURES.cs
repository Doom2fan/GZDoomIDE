using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GZDoomIDE.Data;

namespace CorePlugin.Parsers.ZDoomParsers {
    public class TEXTURESParser : ZDoomParser {
        /// <summary>
        /// The errors encountered, if any.
        /// </summary>
        public List<(string, ZDoomToken)> ParsingError { get; protected internal set; }

        public TEXTURESParser () {
            ParsingError = new List<(string, ZDoomToken)> ();
        }

        public TEXTURESParser (Stream stream) : this () {
            base.Initialize (stream);
        }

        new public void Initialize (Stream stream) {
            base.Initialize (stream);
        }

        protected readonly string [] identifiers = {
            "remap",
            "define",
            "texture",
            "sprite",
            "walltexture",
            "flat",
            "graphic",
        };
        protected string GetActualIdentifier (ZDoomToken tk) {
            if (tk.Type == ZDoomTokens.String && identifiers.Contains (tk.Text)) {
                ParsingError.Add (("YOUR KEYWORD SHENANIGANS ARE NOT WELCOME HERE, MONSTER. (Expected an identifier, got a string)", tk));
                return null;
            } else if (tk.Type != ZDoomTokens.Identifier) {
                ParsingError.Add ((string.Format ("Expected an identifier, got {0}.", tk.GetName ()), tk));
                return null;
            }

            return tk.Text;
        }

        /// <summary>
        /// Parses the lump.
        /// </summary>
        /// <returns></returns>
        public bool Parse (Func<string, bool> includeCallback) {
            if (FileData is null)
                throw new ZDoomParserException ("The parser was not initialized before calling Parse");
            if (includeCallback is null)
                throw new ArgumentNullException ("includeCallback");

            ZDoomToken tk;
            while ((tk = Next ()) != null) {
                if (tk.Type == ZDoomTokens.Hash) {
                    tk = Next ();
                    if (tk.Type != ZDoomTokens.Identifier) {
                        ParsingError.Add (($"Expected \"include\", got {tk.GetName ()}.", tk));
                        return false;
                    } else if (!tk.Text.Equals ("include", StringComparison.CurrentCultureIgnoreCase)) {
                        ParsingError.Add (($"Expected \"include\", got {tk.Text}.", tk));
                        return false;
                    }

                    tk = Next ();
                    if (tk.Type != ZDoomTokens.String) {
                        ParsingError.Add (($"Expected a string, got {tk.GetName ()}.", tk));
                        return false;
                    }

                    if (!includeCallback (tk.Text)) {
                        ParsingError.Add (($"Could not parse included file \"{tk.Text}\".", tk));
                        return false;
                    }
                }

                var keyword = GetActualIdentifier (tk);
                if (keyword is null)
                    return false;

                TextureType texType;
                switch (keyword.ToLower ()) {
                    case "texture":     texType = TextureType.Override;  break;
                    case "sprite":      texType = TextureType.Sprite;    break;
                    case "walltexture": texType = TextureType.Wall;      break;
                    case "flat":        texType = TextureType.Flat;      break;
                    case "graphic":     texType = TextureType.MiscPatch; break;

                    default:
                        ParsingError.Add (($"Unknown keyword \"{tk.Text}\".", tk));
                        return false;
                }

                if (!ParseTexture (TextureType.Override))
                    return false;
            }

            return true;
        }

        protected bool ParseTexture (TextureType type) {

            return true;
        }
    }
}
