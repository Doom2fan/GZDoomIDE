using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GZDoomIDE.Windows.Controls {
    public partial class IDETextBox : TextBox {
        public Color BorderColor { get; set; }

        [DllImport ("user32.dll")]
        protected static extern int ReleaseDC (IntPtr hWnd, IntPtr hDC);

        [DllImport ("user32.dll")]
        protected static extern IntPtr GetWindowDC (IntPtr hWnd);

        protected override void WndProc (ref Message m) {
            const int WM_SIZE = 0x05;
            const int WM_PAINT = 0x0F;
            const int WM_NCPAINT = 0x85;

            base.WndProc (ref m);

            if (m.Msg == WM_SIZE || m.Msg == WM_PAINT || m.Msg == WM_NCPAINT) {
                IntPtr hdc = GetWindowDC (m.HWnd);

                if ((int) hdc != 0) {
                    try {
                        using (var g = Graphics.FromHdc (hdc)) {
                            using (var p = new Pen (BorderColor, 1))
                                g.DrawRectangle (p, new Rectangle (0, 0, Width - 1, Height - 1));

                            g.Flush ();
                        }
                    } finally {
                        ReleaseDC (m.HWnd, hdc);
                    }
                }
            }
        }
    }
}
