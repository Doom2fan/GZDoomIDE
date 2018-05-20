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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion

namespace GZDoomIDE.Windows {
    public partial class SplashScreen : Form {
        #region ================== Constructor

        protected SplashScreen () {
            InitializeComponent ();
        }

        #endregion

        #region ================== Types

        public enum WorkBarState {
            Hidden,
            Continuous,
            Marquee,
            Blocks,
        };

        protected delegate void SetLabelDelegate (string text);
        protected delegate void SetWorkBarDelegate (WorkBarState state, int progress);
        protected delegate void SetTotalBarDelegate (int progress);

        #endregion

        #region ================== Static members

        /// <summary>
        /// The splash screen instance.
        /// </summary>
        protected static SplashScreen Splash { get; set; }

        #endregion

        #region ================== Static methods

        public static void ShowSplash () {
            if (Splash != null && !Splash.IsDisposed)
                throw new Exception ("Attempted to show the splash screen while it is already shown.");

            Thread thread = new Thread (new ThreadStart (ShowSplashInternal));
            thread.IsBackground = true;
            thread.SetApartmentState (ApartmentState.STA);
            thread.Start ();
        }

        protected static void ShowSplashInternal () {
            Splash = new SplashScreen ();
            Application.Run (Splash);
        }

        public static void CloseSplash () {
            if (Splash == null || Splash.IsDisposed)
                throw new Exception ("Attempted to perform an action on the splash screen while it was null or disposed.");

            if (Splash.InvokeRequired) {
                Splash.Invoke (new Action (CloseSplash));
                return;
            }

            Splash.Close ();
            Splash = null;
        }

        public static void SetOperationLabel (string text) {
            if (Splash == null || Splash.IsDisposed)
                throw new Exception ("Attempted to perform an action on the splash screen while it was null or disposed.");

            if (Splash.operationLabel.InvokeRequired) {
                Splash.BeginInvoke (new SetLabelDelegate (SetOperationLabel), text);
                return;
            }

            Splash.operationLabel.Text = text;
        }

        public static void SetWorkLabel (string text) {
            if (Splash == null || Splash.IsDisposed)
                throw new Exception ("Attempted to perform an action on the splash screen while it was null or disposed.");

            if (Splash.workLabel.InvokeRequired) {
                Splash.BeginInvoke (new SetLabelDelegate (SetWorkLabel), text);
                return;
            }

            Splash.workLabel.Text = text;
        }

        public static void SetWorkBar (WorkBarState state, int progress) {
            if (Splash == null || Splash.IsDisposed)
                throw new Exception ("Attempted to perform an action on the splash screen while it was null or disposed.");

            if (Splash.workProgressBar.InvokeRequired) {
                Splash.BeginInvoke (new SetWorkBarDelegate (SetWorkBar), state, progress);
                return;
            }

            switch (state) {
                case WorkBarState.Continuous: Splash.workProgressBar.Style = ProgressBarStyle.Continuous; break;
                case WorkBarState.Marquee: Splash.workProgressBar.Style = ProgressBarStyle.Marquee; break;
                case WorkBarState.Blocks: Splash.workProgressBar.Style = ProgressBarStyle.Blocks; break;
            }

            Splash.workProgressBar.Visible = (state != WorkBarState.Hidden);
            Splash.workProgressBar.Value = progress;
        }

        public static void SetTotalBar (int progress) {
            if (Splash == null || Splash.IsDisposed)
                throw new Exception ("Attempted to perform an action on the splash screen while it was null or disposed.");

            if (Splash.totalProgressBar.InvokeRequired) {
                Splash.BeginInvoke (new SetTotalBarDelegate (SetTotalBar), progress);
                return;
            }

            Splash.totalProgressBar.Value = progress;
        }

        #endregion
    }
}
