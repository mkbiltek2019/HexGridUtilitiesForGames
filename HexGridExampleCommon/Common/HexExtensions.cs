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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PGNapoleonics.HexUtilities.Common {
    /// <summary>Extension methods for <see Cref="Hex"/>.</summary>
    public static partial class HexExtensions {
        /// <summary>The <i>Manhattan</i> distance from this hex to that at <c>coords</c>.</summary>
        public static int Range(this IHex @this, IHex target)
        => @this.Coords.Range(target.Coords);

        /// <summary>TODO</summary>
        public static void Paint(this IHex @this, Graphics graphics, GraphicsPath path, Brush brush) {
            if (graphics==null) throw new ArgumentNullException("graphics");
            graphics.FillPath(brush, path);
        }
    }

    /// <summary>TODO</summary>
    public interface IPaintableHex<TSurface,TPath> {
        /// <summary>TODO</summary>
        void Paint(TSurface graphics, TPath path);
    }
}