using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace bll
{
    public class ImageCreater
    {

        // 将text画到一个图标上，并返回图标对象
        public static Icon CreateTrayIcon(string text)
        {
            Icon icon = null;
            Bitmap bitmap = null;
            Graphics graph = null;

            Font imgfont = new Font("黑体", 11);
            StringFormat imgformat = new StringFormat();
            imgformat.FormatFlags = StringFormatFlags.NoWrap;

            try
            {
                bitmap = new Bitmap(16, 16);
                graph = Graphics.FromImage(bitmap);
                // 填充透明的背景
                graph.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Red)), new Rectangle(0, 0, 16, 16));
                graph.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                graph.DrawString(text, imgfont, new SolidBrush(SystemColors.ControlText),new PointF(-1, 0), imgformat);
                icon = Icon.FromHandle(bitmap.GetHicon());
            }
            catch
            {
            }
            finally
            {
                if (null != graph)
                    graph.Dispose();
                if (null != bitmap)
                    bitmap.Dispose();
            }

            return icon;
        }

        public static bool CreateTrayIcon(string text, string file)
        {
            bool result;
            Icon icon = null;
            Bitmap bitmap = null;
            Graphics graph = null;
            Stream s = null;

            Font imgfont = new Font("黑体", 11);
            StringFormat imgformat = new StringFormat();
            imgformat.FormatFlags = StringFormatFlags.NoWrap;

            try
            {
                bitmap = new Bitmap(16, 16);
                graph = Graphics.FromImage(bitmap);
                graph.FillRectangle(new SolidBrush(SystemColors.Control),
                                new Rectangle(0, 0, 16, 16));
                graph.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                graph.DrawString(text, imgfont, new SolidBrush(SystemColors.ControlText),
                                new PointF(-1, 0), imgformat);

                icon = Icon.FromHandle(bitmap.GetHicon());
                s = new FileStream(file, FileMode.Create);
                icon.Save(s);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (null != s)
                    s.Close();
                if (null != graph)
                    graph.Dispose();
                if (null != bitmap)
                    bitmap.Dispose();
                if (null != icon)
                    icon.Dispose();
            }

            return result;
        }

    }//end
}
