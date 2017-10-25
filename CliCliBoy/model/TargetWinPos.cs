using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public interface ITargetWinPosProp
    {
        TargetWinPos TargetWinPos
        {
            get;set;
        }
    }

    public class TargetWinPos
    {
        private Rectangle _position = new Rectangle();

        public TargetWinPos()
        {
        }
        public TargetWinPos(Rectangle pos)
        {
            _position = pos;
        }
        public TargetWinPos(TargetWinPos src)
        {
            _position = src.Position;
        }

        [System.Xml.Serialization.XmlIgnore]
        public bool hasValue
        {
            get { return !_position.IsEmpty; }
        }

        public Rectangle Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public void Clear()
        {
            _position = new Rectangle();
        }

        public override bool Equals(Object o)
        {
            var p = o as TargetWinPos;
            if (null == p)
            {
                return false;
            }
            if (hasValue)
            {
                return (p.hasValue && p.Position == Position);
            }
            else
            {
                return !p.hasValue;
            }
        }

        public bool Equals(TargetWinPos p)
        {
            if (null == p)
            {
                return false;
            }
            if (hasValue)
            {
                return (p.hasValue && p.Position == Position);
            }
            else
            {
                return !p.hasValue;
            }
        }


        public static bool operator ==(TargetWinPos a, TargetWinPos b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            else
            {
                return a.Equals(b);
            }
        }

        public static bool operator !=(TargetWinPos a, TargetWinPos b)
        {
            return !(a == b);
        }

        public TargetWinPos Clone()
        {
            return new TargetWinPos(this);
        }

        
    }
}
