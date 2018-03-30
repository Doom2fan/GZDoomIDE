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

#endregion

namespace GZDoomIDE.Data {
    public class SemVer {
        #region ================== Properties

        public byte Major { get; set; } = 0;
        public byte Minor { get; set; } = 0;
        public byte Patch { get; set; } = 0;

        public int AsInt { get => ((Major << 16) | (Minor << (8)) | Patch); }

        #endregion

        #region ================== Constructors

        /// <summary>
        /// Creates a semVer with the specified values.
        /// </summary>
        /// <param name="maj">The major version.</param>
        /// <param name="min">The minor version.</param>
        /// <param name="patch">The patch version.</param>
        public SemVer (byte maj, byte min, byte patch) {
            Major = maj;
            Minor = min;
            Patch = patch;
        }

        #endregion

        #region ================== Instance methods

        /// <summary>
        /// Checks if both versions are API-compatible. (Note: This does not necessarily mean they aren't compatible, just that they might not be.)
        /// </summary>
        /// <param name="other">The semVer to compare to</param>
        /// <returns>Returns a bool indicating whether the versions are compatible</returns>
        bool IsAPICompatible (SemVer other) {
            return (Major == other.Major);
        }

        public override string ToString () {
            return String.Format ("{0}.{1}.{2}", Major, Minor, Patch);
        }

        #endregion

        #region ================== Comparison operators

        public override bool Equals (object obj) {
            if (!(obj.GetType () == typeof (SemVer)))
                return false;

            return AsInt == (obj as SemVer).AsInt;
        }
        public override int GetHashCode () {
            return AsInt;
        }

        public static bool operator == (SemVer lhs, SemVer rhs) { return lhs.AsInt == rhs.AsInt; }
        public static bool operator != (SemVer lhs, SemVer rhs) { return lhs.AsInt != rhs.AsInt; }
        public static bool operator > (SemVer lhs, SemVer rhs) { return lhs.AsInt > rhs.AsInt; }
        public static bool operator < (SemVer lhs, SemVer rhs) { return lhs.AsInt < rhs.AsInt; }
        public static bool operator >= (SemVer lhs, SemVer rhs) { return lhs.AsInt >= rhs.AsInt; }
        public static bool operator <= (SemVer lhs, SemVer rhs) { return lhs.AsInt <= rhs.AsInt; }

        #endregion
    }
}
