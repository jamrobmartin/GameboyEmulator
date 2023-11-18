using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyEmulator
{
    public class Byte : CustomValueType<Byte, byte>
    {
        private Byte(byte value) : base(value) { }
        private Byte(int value) : base((byte)value) { }

        public static implicit operator Byte(int value) { return new Byte(value); }
        public static implicit operator Byte(byte value) { return new Byte(value); }
        public static implicit operator int(Byte custom) { return custom._value; }

        public static implicit operator bool(Byte custom) { return custom._value > 0; }
        public static implicit operator Byte(bool value) { return value ? new Byte(1) : new Byte(0); }


        public string ToHexString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:X2}", _value);
            return sb.ToString();
        }
    }

    public class Word : CustomValueType<Word, UInt16>
    {
        private Word(int value) : base((UInt16)value) { }
        private Word(Byte value) : base((UInt16)value) { }

        public static implicit operator Word(int value) { return new Word(value); }
        public static implicit operator Word(Byte value) { return new Word(value); }
        public static implicit operator int(Word custom) { return custom._value; }


        public string ToHexString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:X4}", _value);
            return sb.ToString();
        }
    }

    public class CustomValueType<TCustom, TValue>
    {
        protected readonly TValue _value;

        public CustomValueType(TValue value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public static bool operator <(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return Comparer<TValue>.Default.Compare(a._value, b._value) < 0;
        }

        public static bool operator >(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return !(a < b);
        }

        public static bool operator <=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (a < b) || (a == b);
        }

        public static bool operator >=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (a > b) || (a == b);
        }

        public static bool operator ==(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return a.Equals((object)b);
        }

        public static bool operator !=(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return !(a == b);
        }

        public static TCustom operator +(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (dynamic)a._value + b._value;
        }

        public static TCustom operator -(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return ((dynamic)a._value - b._value);
        }

        public static TCustom operator &(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (dynamic)a._value & b._value;
        }

        public static TCustom operator |(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (dynamic)a._value | b._value;
        }

        public static TCustom operator ^(CustomValueType<TCustom, TValue> a, CustomValueType<TCustom, TValue> b)
        {
            return (dynamic)a._value ^ b._value;
        }

        protected bool Equals(CustomValueType<TCustom, TValue> other)
        {
            return EqualityComparer<TValue>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CustomValueType<TCustom, TValue>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(_value);
        }
    }
}
