using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public struct HSVColor
    {
        public float H { get; set; }
        public float S { get; set; }
        public float V { get; set; }

        public static HSVColor FromHSV(float h, float s, float v)
        {
            return new HSVColor() { H = h, S = s, V = v };
        }

        public static HSVColor FromColor(System.Drawing.Color c)
        {
            return FromColor(System.Windows.Media.Color.FromRgb(c.R, c.G, c.B));
        }

        public static HSVColor FromColor(System.Windows.Media.Color c)
        {
            float r = c.R / 255.0f;
            float g = c.G / 255.0f;
            float b = c.B / 255.0f;

            var list = new float[] { r, g, b };
            var max = list.Max();
            var min = list.Min();

            float h, s, v;
            if (max == min)
                h = 0;
            else if (max == r)
                h = (60 * (g - b) / (max - min) + 360) % 360;
            else if (max == g)
                h = 60 * (b - r) / (max - min) + 120;
            else
                h = 60 * (r - g) / (max - min) + 240;

            if (max == 0)
                s = 0;
            else
                s = (max - min) / max;

            v = max;

            return new HSVColor() { H = h, S = s, V = v };
        }
        public System.Windows.Media.Color ToMediaColor()
        {
            int Hi = ((int)(H / 60.0)) % 6;
            float f = H / 60.0f - (int)(H / 60.0);
            float p = V * (1 - S);
            float q = V * (1 - f * S);
            float t = V * (1 - (1 - f) * S);

            switch (Hi)
            {
                case 0:
                    return FromRGB(V, t, p);
                case 1:
                    return FromRGB(q, V, p);
                case 2:
                    return FromRGB(p, V, t);
                case 3:
                    return FromRGB(p, q, V);
                case 4:
                    return FromRGB(t, p, V);
                case 5:
                    return FromRGB(V, p, q);
            }

            // ここには来ない
            throw new InvalidOperationException();
        }

        public System.Drawing.Color ToDrawingColor()
        {
            var c = ToMediaColor();
            return System.Drawing.Color.FromArgb(c.R, c.G, c.B);
        }

        private static System.Windows.Media.Color FromRGB(float fr, float fg, float fb)
        {
            fr *= 255;
            fg *= 255;
            fb *= 255;
            byte r = (byte)((fr < 0) ? 0 : (fr > 255) ? 255 : fr);
            byte g = (byte)((fg < 0) ? 0 : (fg > 255) ? 255 : fg);
            byte b = (byte)((fb < 0) ? 0 : (fb > 255) ? 255 : fb);
            return System.Windows.Media.Color.FromRgb(r, g, b);
        }
    }

    public class HSVColorRange
    {
        public AngleRange H { get; set; }
        public FloatRange S { get; set; }
        public FloatRange V { get; set; }

        public HSVColorRange()
        {
            H = new AngleRange();
            S = new FloatRange();
            V = new FloatRange();
        }

        public void Clear()
        {
            H.Clear();
            S.Clear();
            V.Clear();
        }

        public void AddColor(System.Windows.Media.Color color)
        {
            var hsv = HSVColor.FromColor(color);
            H.AddValue(hsv.H);
            S.AddValue(hsv.S);
            V.AddValue(hsv.V);
        }

        public void AddColor(System.Drawing.Color color)
        {
            var hsv = HSVColor.FromColor(color);
            H.AddValue(hsv.H);
            S.AddValue(hsv.S);
            V.AddValue(hsv.V);
        }

        public bool IsInRange(System.Drawing.Color color, StringBuilder sb=null)
        {
            var hsv = HSVColor.FromColor(color);

#if DEBUG
            Debug.WriteLine("HSV range test...");
            Debug.WriteLine("    H: {3} : {0}  range({1} -- {2}", hsv.H, H.Min, H.Max, H.IsInRange(hsv.H) ? "TRUE " : "FALSE");
            Debug.WriteLine("    S: {3} : {0}  range({1} -- {2}", hsv.S, S.Min, S.Max, S.IsInRange(hsv.S) ? "TRUE " : "FALSE");
            Debug.WriteLine("    V: {3} : {0}  range({1} -- {2}", hsv.V, V.Min, V.Max, V.IsInRange(hsv.V) ? "TRUE " : "FALSE");
#endif
            if (sb != null)
            {
                bool h = H.IsInRange(hsv.H),
                     s = S.IsInRange(hsv.S),
                     v = V.IsInRange(hsv.V);
                bool result = h && s && v;
                if (result)
                {
                    sb.AppendLine("TRUE ----");
                }
                else
                {
                    sb.AppendLine("FALSE ---");
                }
                sb.AppendFormat(" {3} H: {0}  range({1} - {2})", hsv.H, H.Min, H.Max, H.IsInRange(hsv.H) ? "o" : "x").AppendLine();
                sb.AppendFormat(" {3} S: {0}  range({1} - {2})", hsv.S, S.Min, S.Max, S.IsInRange(hsv.S) ? "o" : "x").AppendLine();
                sb.AppendFormat(" {3} V: {0}  range({1} - {2})", hsv.V, V.Min, V.Max, V.IsInRange(hsv.V) ? "o" : "x").AppendLine();
                return result;
            }
            else
            {
                return H.IsInRange(hsv.H) && S.IsInRange(hsv.S) && V.IsInRange(hsv.V);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool IsValid
        {
            get
            {
                return H.IsValid && S.IsValid && V.IsValid;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public System.Windows.Media.Color this[int i]
        {
            get
            {
                if (!IsValid) return System.Windows.Media.Color.FromRgb(255, 255, 255);

                switch (i)
                {
                    default:
                        return System.Windows.Media.Color.FromRgb(0,0,0);
                    case 0:
                        return HSVColor.FromHSV(H.Min, S.Min, V.Min).ToMediaColor();
                    case 1:
                        return HSVColor.FromHSV(H.Min, S.Min, V.Max).ToMediaColor();
                    case 2:
                        return HSVColor.FromHSV(H.Min, S.Max, V.Min).ToMediaColor();
                    case 3:
                        return HSVColor.FromHSV(H.Min, S.Max, V.Max).ToMediaColor();
                    case 4:
                        return HSVColor.FromHSV(H.Max, S.Min, V.Min).ToMediaColor();
                    case 5:
                        return HSVColor.FromHSV(H.Max, S.Min, V.Max).ToMediaColor();
                    case 6:
                        return HSVColor.FromHSV(H.Max, S.Max, V.Min).ToMediaColor();
                    case 7:
                        return HSVColor.FromHSV(H.Max, S.Max, V.Max).ToMediaColor();
                    case 8:
                        return HSVColor.FromHSV(H.Median, S.Median, V.Median).ToMediaColor();
                }
            }
        }
        public bool Equals(HSVColorRange f)
        {
            if (null == f)
            {
                return false;
            }
            return H.Equals(f.H) && S.Equals(f.S) && V.Equals(f.V);
        }

        public void CopyFrom(HSVColorRange s)
        {
            if (null == s)
            {
                Clear();
                return;
            }

            H.CopyFrom(s.H);
            S.CopyFrom(s.S);
            V.CopyFrom(s.V);
        }

        public HSVColorRange Clone()
        {
            return HSVColorRange.Clone(this);
        }

        public static HSVColorRange Clone(HSVColorRange s)
        {
            if (null == s)
            {
                return null;
            }

            var r = new HSVColorRange();
            r.CopyFrom(s);
            return r;
        }
    }
}
