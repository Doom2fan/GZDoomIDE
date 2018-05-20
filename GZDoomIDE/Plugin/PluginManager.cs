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

using GZDoomIDE.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#endregion

namespace GZDoomIDE.Plugin {
    internal sealed class PluginManager {
        #region ================== Properties

        public List<PluginData> Plugins { get; private set; }

        public bool IsDisposed { get; private set; }

        #endregion

        #region ================== Constructor / Disposer

        public PluginManager () {
            Plugins = new List<PluginData> ();

            // We have no destructor
            GC.SuppressFinalize (this);
        }

        public void Dispose () {
            if (!IsDisposed) {
                foreach (PluginData p in Plugins)
                    p.Dispose ();
                
                IsDisposed = true;
            }
        }

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Loads the specified DLL as a plugin.
        /// </summary>
        /// <param name="file">The DLL to load.</param>
        public void LoadPlugin (string file) {
            PluginData p;

            try {
                p = PluginData.Load (file);
            } catch (InvalidProgramException) {
                p = null;
            }

            // Continue if no errors
            if (!(p is null) && (!p.IsDisposed)) {
                // Add to plugins list
                Plugins.Add (p);

                Program.Data.LoadProjectTypes (p);

                p.Plug.MainWindow = Program.MainWindow;

                // Plugin is now initialized
                p.Plug.OnInitialize ();
            }
        }

        #endregion

        #region ================== Callbacks

        #region ================== Text editor

        #region ================== Text modified

        /// <summary>
        /// Called before text is deleted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnBeforeDelete (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.BeforeModificationEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_BeforeDelete (window, editor, e);
            }
        }

        /// <summary>
        /// Called before text is inserted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnBeforeInsert (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.BeforeModificationEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_BeforeInsert (window, editor, e);
            }
        }

        /// <summary>
        /// Called before text is inserted. Allows changing the inserted text.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnInsertCheck (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.InsertCheckEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_InsertCheck (window, editor, e);
            }
        }

        /// <summary>
        /// Called when text is inserted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnInsert (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.ModificationEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_Insert (window, editor, e);
            }
        }

        /// <summary>
        /// Called when text is deleted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnDelete (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.ModificationEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_Delete (window, editor, e);
            }
        }

        /// <summary>
        /// Called when the user types a character.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnCharAdded (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.CharAddedEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_CharAdded (window, editor, e);
            }
        }

        #endregion

        #region ================== Misc callbacks

        /// <summary>
        /// Called when an annotation has changed.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnChangeAnnotation (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.ChangeAnnotationEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_ChangeAnnotation (window, editor, e);
            }
        }

        /// <summary>
        /// Called when the mouse is kept in one position (hovers) for a period of time.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnDwellStart (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.DwellEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_DwellStart (window, editor, e);
            }
        }

        /// <summary>
        /// Called when the mouse moves from its dwell start position.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnDwellEnd (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.DwellEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_DwellEnd (window, editor, e);
            }
        }

        /// <summary>
        /// Called when the text editor control's UI is updated
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnUpdateUI (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.UpdateUIEventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_UpdateUI (window, editor, e);
            }
        }

        /// <summary>
        /// Called when the text editor is zooomed.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        internal void TextEditor_OnZoomChanged (TextEditorWindow window, ScintillaNET.Scintilla editor, EventArgs e) {
            foreach (var plugin in Plugins) {
                plugin.Plug.TextEditor_ZoomChanged (window, editor, e);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
