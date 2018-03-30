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
using System.Text;

#endregion

namespace GZDoomIDE {
    public class Logger {
        public virtual StringBuilder Text { get; protected set; } = new StringBuilder ();

        public virtual void Write (string fmt, params object [] args) {
            Text.Append (String.Format (fmt, args));
            Console.Write (fmt, args);
        }

        public virtual void WriteLine (string fmt, params object [] args) {
            Text.Append (String.Format (fmt, args));
            Console.Write (fmt, args);
        }
    }

    public class DebugLogger : Logger {
        public override void Write (string fmt, params object [] args) {
            Text.Append (String.Format (fmt, args));
            Console.Write (fmt, args);
        }

        public override void WriteLine (string fmt, params object [] args) {
            Text.Append (String.Format (fmt, args));
            Console.Write (fmt, args);
        }
    }
}
