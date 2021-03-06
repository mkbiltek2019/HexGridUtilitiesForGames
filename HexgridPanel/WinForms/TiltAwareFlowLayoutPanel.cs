﻿#region The MIT License - Copyright (C) 2012-2019 Pieter Geerkens
/////////////////////////////////////////////////////////////////////////////////////////
//                PG Software Solutions - Hex-Grid Utilities
/////////////////////////////////////////////////////////////////////////////////////////
// The MIT License:
// ----------------
// 
// Copyright (c) 2012-2019 Pieter Geerkens (email: pgeerkens@users.noreply.github.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to the following 
// conditions:
//     The above copyright notice and this permission notice shall be 
//     included in all copies or substantial portions of the Software.
// 
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//     EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//     OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
//     NON-INFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//     HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
//     FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
//     OTHER DEALINGS IN THE SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

using PGNapoleonics.HexgridPanel.WinForms;

namespace PGNapoleonics.HexgridPanel {
    /// <summary>A Tilt-Aware FlowLayoutPanel</summary>
    /// ///<remarks>Courtesy of Hans Passant: 
    /// <a>http://stackoverflow.com/questions/3562235/panel-not-getting-focus</a>
    /// </remarks>
    public class TiltAwareFlowLayoutPanel : FlowLayoutPanel, IScrollableControl {
        /// <summary>TODO</summary>
        public TiltAwareFlowLayoutPanel() : base() {
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
        }

        #region Implementation of "scrolling without focus"
        /// <inheritdoc/>
        protected override bool IsInputKey(Keys keyData)
            => keyData.IsInputKey() || base.IsInputKey(keyData);
        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e) { Focus(); base.OnMouseDown(e); }
        /// <inheritdoc/>
        protected override void OnEnter(EventArgs e)          { Invalidate(); base.OnEnter(e); }
        /// <inheritdoc/>
        protected override void OnLeave(EventArgs e)          { Invalidate(); base.OnLeave(e); }
        /// <inheritdoc/>
        protected override void OnMouseEnter(EventArgs e)     { base.OnMouseEnter(e); Focus(); }
        /// <inheritdoc/>
        protected override void OnMouseLeave(EventArgs e)     { Parent.Focus(); base.OnMouseEnter(e); }
        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e) {
            if (Focused  &&  ShowFocusCues) { this.DrawFocusRectangle(e.Graphics, new Point(-2,-2)); }
            base.OnPaint(e);
        }
        #endregion

        /// <summary>Occurs when the mouse tilt-wheel moves while the control has focus.</summary>
        public event EventHandler<MouseEventArgs> MouseHWheel;

        /// <inheritdoc/>
        public Point UnappliedScroll { get; set; } = new Point();

        /// <inheritdoc/>
        public Point ScrollLargeChange => new Point (120, 120);

        /// <summary>Raise a <see cref="MouseHWheel"/> event.</summary>
        /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data. </param>
        protected virtual void OnMouseHWheel(MouseEventArgs e) {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!AutoScroll) return;

            this.RollHorizontal(e.Delta * MouseHWheelStep/120);
            MouseHWheel?.Invoke(this, e);

            if(e is HandledMouseEventArgs eh) eh.Handled = true;
        }

        /// <summary>TODO</summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e) {
            if (e == null) throw new ArgumentNullException(nameof(e));
            base.OnMouseWheel(new MouseEventArgs(e.Button,e.Clicks,e.X,e.Y,
                                e.Delta*MouseVWheelStep/120));
        }

        /// <summary>TODO</summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vwheel")]
        public int MouseVWheelStep { set; get; }
        /// <summary>TODO</summary>
        public int MouseHWheelStep { set; get; }

        /// <summary>TODO</summary>
        public void ScrollByOneControl(bool forward) {
            var rectangle   = ClientRectangle;  rectangle.Inflate(-4,-4);
            var control     = GetChildAtPoint(rectangle.Location + (forward ? rectangle.Size : Size.Empty));
            var nextControl = GetNextControl(control,forward);
            ScrollControlIntoView(nextControl);
        }

        /// <summary>Extend Windows Message Loop to receive MouseHWheel messages.</summary>
        protected override void WndProc(ref Message m) {
            if (!IsDisposed && m.HWnd == this.Handle) {
                switch ((WM)m.Msg) {
                    case WM.MouseHWheel:  OnMouseHWheel(m.CreateMouseEventArgs());
                                          m.Result = (IntPtr)0;
                                          break;
                    default:              break;
                }
            }
            base.WndProc(ref m);
        }
    }
}
