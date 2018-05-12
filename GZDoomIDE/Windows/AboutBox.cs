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

using System;
using System.Reflection;
using System.Windows.Forms;

#endregion

namespace GZDoomIDE.Windows {
    partial class AboutBox : Form {
        public AboutBox () {
            InitializeComponent ();

            this.Text = String.Format ("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format ("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;

            Program.Data.Themer.ApplyTheme (this);
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle {
            get {
                object [] attributes = Assembly.GetExecutingAssembly ().GetCustomAttributes (typeof (AssemblyTitleAttribute), false);
                if (attributes.Length > 0) {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute) attributes [0];
                    if (titleAttribute.Title != "") {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension (Assembly.GetExecutingAssembly ().CodeBase);
            }
        }

        public string AssemblyVersion {
            get {
                return Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();
            }
        }

        public string AssemblyProduct {
            get {
                object [] attributes = Assembly.GetExecutingAssembly ().GetCustomAttributes (typeof (AssemblyProductAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyProductAttribute) attributes [0]).Product;
            }
        }

        public string AssemblyCopyright {
            get {
                object [] attributes = Assembly.GetExecutingAssembly ().GetCustomAttributes (typeof (AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                }
                return ((AssemblyCopyrightAttribute) attributes [0]).Copyright;
            }
        }
        #endregion
    }
}
