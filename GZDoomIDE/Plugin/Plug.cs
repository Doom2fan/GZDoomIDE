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

using GZDoomIDE.Data;
using GZDoomIDE.Windows;
using System;

#endregion

namespace GZDoomIDE.Plugin {
    public class Plug : IDisposable {
        #region ================== Properties

        internal PluginData Plugin { get; set; }

        protected internal MainWindow MainWindow { get; internal set; }

        /// <summary>
		/// Indicates if the plugin has been disposed.
		/// </summary>
		public bool IsDisposed { get; protected set; }

        /// <summary>
        /// Override this to return a more descriptive name for your plugin.
        /// Default is the library filename without extension.
        /// </summary>
        public virtual string Name { get { return Plugin.Name; } }
        
        /// <summary>
        /// The plugin's name.
        /// </summary>
        public virtual SemVer Version { get; }

        /// <summary>
        /// The lowest version this plugin works in
        /// </summary>
        public virtual SemVer MinimumIDEVersion { get; }

        /// <summary>
        /// The highest version this plugin works in
        /// </summary>
        public virtual SemVer MaximumIDEVersion { get; } = null;

        #endregion

        #region ================== Constructor / Disposer

        /// <summary>
        /// This is the key link between the IDE core and the plugin.
        /// Every plugin must expose a single class that inherits this class.
        /// <para>
        /// NOTE: Some methods cannot be used in this constructor, because the plugin
        /// is not yet fully initialized. Instead, use the Initialize method to do
        /// your initializations.
        /// </para>
        /// </summary>
        public Plug () {
            // Initialize

            // We have no destructor
            GC.SuppressFinalize (this);
        }

        /// <summary>
        /// This is called by the IDE core when the plugin is being disposed.
        /// </summary>
        public virtual void Dispose () {
            // Not already disposed?
            if (!IsDisposed) {
                // Clean up
                Plugin = null;

                // Done
                IsDisposed = true;
            }
        }

        #endregion

        #region ================== Callbacks

        public virtual void OnInitialize () { }

        #region ================== Text editor

        #region ================== Text modified

        /// <summary>
        /// Called before text is deleted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_BeforeDelete (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.BeforeModificationEventArgs e) { }

        /// <summary>
        /// Called before text is inserted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_BeforeInsert (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.BeforeModificationEventArgs e) { }

        /// <summary>
        /// Called before text is inserted. Allows changing the inserted text.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_InsertCheck (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.InsertCheckEventArgs e) { }

        /// <summary>
        /// Called when text is inserted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_Insert (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.ModificationEventArgs e) { }

        /// <summary>
        /// Called when text is deleted.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_Delete (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.ModificationEventArgs e) { }

        /// <summary>
        /// Called when the user types a character.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_CharAdded (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.CharAddedEventArgs e) { }

        #endregion

        #region ================== Misc callbacks

        /// <summary>
        /// Called when an annotation has changed.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_ChangeAnnotation (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.ChangeAnnotationEventArgs e) { }

        /// <summary>
        /// Called when the mouse is kept in one position (hovers) for a period of time.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_DwellStart (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.DwellEventArgs e) { }

        /// <summary>
        /// Called when the mouse moves from its dwell start position.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_DwellEnd (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.DwellEventArgs e) { }

        /// <summary>
        /// Called when the text editor control's UI is updated.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_UpdateUI (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.UpdateUIEventArgs e) { }

        /// <summary>
        /// Called when the text editor needs to be styled.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_StyleNeeded (TextEditorWindow window, ScintillaNET.Scintilla editor, ScintillaNET.StyleNeededEventArgs e) { }
        
        /// <summary>
        /// Called when the text editor is zooomed.
        /// </summary>
        /// <param name="editor">The editor that called the callback.</param>
        /// <param name="e">The callback's args.</param>
        public virtual void TextEditor_ZoomChanged (TextEditorWindow window, ScintillaNET.Scintilla editor, EventArgs e) { }

        #endregion

        #endregion

        #endregion
    }
}
