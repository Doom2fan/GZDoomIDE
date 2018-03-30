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
using System.Collections;
using System.Collections.Generic;

#endregion

namespace GZDoomIDE.Data {
    public enum ErrorType {
        Error = 0,
        Warning,
        Information,
    }

    public class IDEError {
        public IDEError (ErrorType type, string msg, string proj = "")
            : this (type, msg, proj, new Dictionary<string, string> ()) {
        }
        public IDEError (ErrorType type, string msg, string proj, Dictionary<string,string> data) {
            Type = type;
            Message = msg;
            Project = proj;
            Data = data;
        }

        public virtual ErrorType Type { get; set; }
        public virtual string Message { get; set; }
        public virtual string Project { get; set; }
        public virtual string File    { get; set; }
        public virtual int LineNum    { get; set; } = 0;
        public virtual Dictionary<string, string> Data { get; protected set; }
    }

    public class IDEErrorList : IList<IDEError>, ICollection<IDEError>, IEnumerable<IDEError> {
        private List<IDEError> errList;

        #region ================== Constructors

        public IDEErrorList () {
            errList = new List<IDEError> ();
        }
        public IDEErrorList (int capacity) {
            errList = new List<IDEError> (capacity);
        }
        public IDEErrorList (IEnumerable<IDEError> collection) {
            errList = new List<IDEError> (collection);
        }

        #endregion

        #region ================== Events

        public event EventHandler<EventArgs> OnChanged;
        protected virtual void OnChange (EventArgs e) { OnChanged?.Invoke (this, e); }

        #endregion

        #region ================== Interfaces

        #region IEnumerable

        public IEnumerator<IDEError> GetEnumerator () {
            return ((IEnumerable<IDEError>) errList).GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator () {
            return ((IEnumerable<IDEError>) errList).GetEnumerator ();
        }

        #endregion

        #region ICollection

        public int Count => errList.Count;

        public bool IsReadOnly => false;

        public void Add (IDEError item) {
            errList.Add (item);
            OnChange (EventArgs.Empty);
        }

        public bool Remove (IDEError item) {
            bool ret = errList.Remove (item);
            OnChange (EventArgs.Empty);
            return ret;
        }

        public void Clear () {
            errList.Clear ();
            OnChange (EventArgs.Empty);
        }

        public bool Contains (IDEError item) {
            return errList.Contains (item);
        }

        public void CopyTo (IDEError [] array, int arrayIndex) {
            errList.CopyTo (array, arrayIndex);
        }

        #endregion

        #region IList

        public IDEError this [int index] {
            get => errList [index];
            set {
                errList [index] = value;
                OnChange (EventArgs.Empty);
            }
        }

        public int IndexOf (IDEError item) {
            return errList.IndexOf (item);
        }

        public void Insert (int index, IDEError item) {
            errList.Insert (index, item);
            OnChange (EventArgs.Empty);
        }

        public void RemoveAt (int index) {
            errList.RemoveAt (index);
            OnChange (EventArgs.Empty);
        }

        #endregion

        #endregion
    }

    public class IDEErrorProvider {
        protected IDEErrorList _errors;
        public virtual IDEErrorList Errors {
            get => _errors;
            protected set {
                if (!(_errors is null)) // If we already had one set before, unregister from the OnChanged event
                    _errors.OnChanged -= _errors_OnChanged;

                _errors = value;
                _errors.OnChanged += _errors_OnChanged;
            }
        }

        #region ================== Constructors

        public IDEErrorProvider () {
            Errors = new IDEErrorList ();
        }

        #endregion

        #region ================== Events

        public event EventHandler<EventArgs> OnChanged;
        protected virtual void OnChange (EventArgs e) { OnChanged?.Invoke (this, e); }

        #endregion

        private void _errors_OnChanged (object sender, EventArgs e) {
            OnChange (e);
        }
    }
}
