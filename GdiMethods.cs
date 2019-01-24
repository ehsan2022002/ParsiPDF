
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;


namespace PDFFarsi
{

        [SuppressUnmanagedCodeSecurity]
        class GdiMethods
        {
            [DllImport("GDI32.dll")]
            public static extern bool DeleteObject(IntPtr hgdiobj);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern uint GetCharacterPlacement(IntPtr hdc, string lpString, int nCount, int nMaxExtent, [In, Out] ref GcpResults lpResults, uint dwFlags);

            [DllImport("GDI32.dll")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct GcpResults
        {
            public uint lStructSize;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpOutString;
            public IntPtr lpOrder;
            public IntPtr lpDx;
            public IntPtr lpCaretPos;
            public IntPtr lpClass;
            public IntPtr lpGlyphs;
            public uint nGlyphs;
            public int nMaxFit;
        }

        public class UnicodeCharacterPlacement
        {
            const int GcpReorder = 0x0002;
            GCHandle _caretPosHandle;
            GCHandle _classHandle;
            GCHandle _dxHandle;
            GCHandle _glyphsHandle;
            GCHandle _orderHandle;

            public Font Font { set; get; }

            public string Apply(string lines)
            {
                if (string.IsNullOrWhiteSpace(lines))
                    return string.Empty;

                return Apply(lines.Split('\n')).Aggregate((s1, s2) => s1 + s2);
            }

            public IEnumerable<string> Apply(IEnumerable<string> lines)
            {
                if (Font == null)
                    throw new ArgumentNullException("Font is null.");

                if (!hasUnicodeText(lines))
                    return lines;

                var graphics = Graphics.FromHwnd(IntPtr.Zero);
                var hdc = graphics.GetHdc();
                try
                {
                    var font = (System.Drawing.Font)Font.Clone();
                    var hFont = font.ToHfont();
                    var fontObject = GdiMethods.SelectObject(hdc, hFont);
                    try
                    {
                        var results = new List<string>();
                        foreach (var line in lines)
                            results.Add(modifyCharactersPlacement(line, hdc));
                        return results;
                    }
                    finally
                    {
                        GdiMethods.DeleteObject(fontObject);
                        GdiMethods.DeleteObject(hFont);
                        font.Dispose();
                    }
                }
                finally
                {
                    graphics.ReleaseHdc(hdc);
                    graphics.Dispose();
                }
            }

            void freeResources()
            {
                _orderHandle.Free();
                _dxHandle.Free();
                _caretPosHandle.Free();
                _classHandle.Free();
                _glyphsHandle.Free();
            }

            static bool hasUnicodeText(IEnumerable<string> lines)
            {
                return lines.Any(line => line.Any(chr => chr >= '\u00FF'));
            }

            void initializeResources(int textLength)
            {
                _orderHandle = GCHandle.Alloc(new int[textLength], GCHandleType.Pinned);
                _dxHandle = GCHandle.Alloc(new int[textLength], GCHandleType.Pinned);
                _caretPosHandle = GCHandle.Alloc(new int[textLength], GCHandleType.Pinned);
                _classHandle = GCHandle.Alloc(new byte[textLength], GCHandleType.Pinned);
                _glyphsHandle = GCHandle.Alloc(new short[textLength], GCHandleType.Pinned);
            }

            string modifyCharactersPlacement(string text, IntPtr hdc)
            {
                var textLength = text.Length;
                initializeResources(textLength);
                try
                {
                    var gcpResult = new GcpResults
                    {
                        lStructSize = (uint)Marshal.SizeOf(typeof(GcpResults)),
                        lpOutString = new String('\0', textLength),
                        lpOrder = _orderHandle.AddrOfPinnedObject(),
                        lpDx = _dxHandle.AddrOfPinnedObject(),
                        lpCaretPos = _caretPosHandle.AddrOfPinnedObject(),
                        lpClass = _classHandle.AddrOfPinnedObject(),
                        lpGlyphs = _glyphsHandle.AddrOfPinnedObject(),
                        nGlyphs = (uint)textLength,
                        nMaxFit = 0
                    };
                    var result = GdiMethods.GetCharacterPlacement(hdc, text, textLength, 0, ref gcpResult, GcpReorder);
                    return result != 0 ? gcpResult.lpOutString : text;
                }
                finally
                {
                    freeResources();
                }
            }
        }

}
