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

using GZDoomIDE;
using GZDoomIDE.Data;
using GZDoomIDE.Plugin;
using GZDoomIDE.Windows;
using ScintillaNET;
using System;

#endregion

namespace CorePlugin {
    public class IDEPlug : Plug {
        public override SemVer MinimumIDEVersion => new SemVer (Constants.MajorVersion, Constants.MinorVersion, Constants.PatchVersion);
        public override SemVer MaximumIDEVersion => new SemVer (Constants.MajorVersion, Constants.MinorVersion, Constants.PatchVersion);

        ZScriptParser parser;
        public IDEPlug () : base () {
            parser = new ZScriptParser ();

            parser.Setup ("ZScriptGrammar.egt");
        }

        public override void TextEditor_Insert (TextEditorWindow window, Scintilla editor, ModificationEventArgs e) {
            if (CheckIfZScript (window.FilePath, window.Project))
                ParseZScript (editor.Text, window.FilePath, window.Project);
        }

        bool CheckIfZScript (string filePath, ProjectData project) {
            if (String.IsNullOrWhiteSpace (filePath))
                return false;
            
            return System.IO.Path.GetExtension (filePath).Equals (".zs", System.StringComparison.OrdinalIgnoreCase);
        }

        void ParseZScript (string code, string filePath, ProjectData project) {
            parser.FailMessage = string.Empty;
            parser.Parse (code);

            MainWindow.CurFileErrors.Errors.Clear ();
            if (parser.FailMessage != string.Empty)
                MainWindow.CurFileErrors.Errors.Add (new IDEError (ErrorType.Error, parser.FailMessage, (project != null ? project.Name : "")));
        }
    }

    class ZScriptParser {
        private GOLD.Parser parser = new GOLD.Parser ();

        public GOLD.Reduction Root;     //Store the top of the tree
        public string FailMessage;

        public bool Setup (string filePath) {
            return parser.LoadTables (filePath);
        }

        public bool Parse (string text) {
            //This procedure starts the GOLD Parser Engine and handles each of the
            //messages it returns. Each time a reduction is made, you can create new
            //custom object and reassign the .CurrentReduction property. Otherwise, 
            //the system will use the Reduction object that was returned.
            //
            //The resulting tree will be a pure representation of the language 
            //and will be ready to implement.

            GOLD.ParseMessage response;
            bool done;                      //Controls when we leave the loop
            bool accepted = false;          //Was the parse successful?

            parser.Open (ref text);
            parser.TrimReductions = false;  //Please read about this feature before enabling  

            done = false;
            while (!done) {
                response = parser.Parse ();

                switch (response) {
                    case GOLD.ParseMessage.LexicalError:
                        // Cannot recognize token
                        FailMessage = "Lexical Error:\n" +
                                      "Position: " + parser.CurrentPosition ().Line + ", " + parser.CurrentPosition ().Column + "\n" +
                                      "Read: " + parser.CurrentToken ().Data;
                        done = true;
                        break;

                    case GOLD.ParseMessage.SyntaxError:
                        // Expecting a different token
                        FailMessage = "Syntax Error:\n" +
                                      "Position: " + parser.CurrentPosition ().Line + ", " + parser.CurrentPosition ().Column + "\n" +
                                      "Read: " + parser.CurrentToken ().Data + "\n" +
                                      "Expecting: " + parser.ExpectedSymbols ().Text ();
                        done = true;
                        break;

                    case GOLD.ParseMessage.Reduction:
                        // For this project, we will let the parser build a tree of Reduction objects
                        // parser.CurrentReduction = CreateNewObject(parser.CurrentReduction);
                        break;

                    case GOLD.ParseMessage.Accept:
                        // Accepted!
                        Root = (GOLD.Reduction) parser.CurrentReduction;    // The root node!                                  
                        done = true;
                        accepted = true;
                        break;

                    case GOLD.ParseMessage.TokenRead:
                        // You don't have to do anything here.
                        break;

                    case GOLD.ParseMessage.InternalError:
                        // INTERNAL ERROR! Something is horribly wrong.
                        done = true;
                        break;

                    case GOLD.ParseMessage.NotLoadedError:
                        // This error occurs if the CGT was not loaded.                   
                        FailMessage = "Tables not loaded";
                        done = true;
                        break;

                    case GOLD.ParseMessage.GroupError:
                        // GROUP ERROR! Unexpected end of file
                        FailMessage = "Runaway group";
                        done = true;
                        break;
                }
            }

            return accepted;
        }

    }

}
