using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace idx3_ubyte_parser
{
    internal class BinReader : BinaryReader //stupid, BigEnd
    {
        public BinReader(System.IO.Stream stream) : base(stream)
        {
        }

        public override int ReadInt32()
        {
            if (BitConverter.IsLittleEndian)
            {
                var arr = base.ReadBytes(4);
                Array.Reverse(arr);
                return BitConverter.ToInt32(arr, 0);
            }
            return base.ReadInt32();
        }

        public override Int16 ReadInt16()
        {
            if (BitConverter.IsLittleEndian)
            {
                var arr = base.ReadBytes(2);
                Array.Reverse(arr);
                return BitConverter.ToInt16(arr, 0);
            }
            return base.ReadInt16();
        }

        public override Int64 ReadInt64()
        {
            if (BitConverter.IsLittleEndian)
            {
                var arr = base.ReadBytes(8);
                Array.Reverse(arr);
                return BitConverter.ToInt64(arr, 0);
            }
            return base.ReadInt64();
        }

        public override UInt32 ReadUInt32()
        {
            if (BitConverter.IsLittleEndian)
            {
                var arr = base.ReadBytes(4);
                Array.Reverse(arr);
                return BitConverter.ToUInt32(arr, 0);
            }
            return base.ReadUInt32();
        }
    }
}