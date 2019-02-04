using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE_Chat
{
    public struct Message
    {
        public static readonly int PACKET_SIZE = 1024*10;
        public short Length { get; private set; }
        public byte[] UsefulData { get; private set; }
        public byte[] Packet { get; private set; }

        private Message(short _length, byte[] _usefulData, byte[] _packet)
        {
            Length = _length;
            UsefulData = _usefulData;
            Packet = _packet;
        }

        public static Message Encode(byte[] _toSend)
        {
            if (_toSend.Length > PACKET_SIZE + 2)
            {
#if DEBUG
                throw new ArgumentException();
#else
                return new Message(-1, null, null);
#endif
            }
            byte[] packet = new byte[PACKET_SIZE];
            packet[0] = (byte)_toSend.Length;
            packet[1] = (byte)(_toSend.Length >> 8);

            for (int i = 0; i < _toSend.Length; i++)
            {
                packet[i + 2] = _toSend[i];
            }

            return new Message((short)_toSend.Length, _toSend, packet);
        }

        public static Message Decode(byte[] _received)
        {
            if (_received == null || _received.Length < 2)
            {
#if DEBUG
                throw new ArgumentException();
#else
                return new Message(-1, null, null);
#endif
            }

            short Length = (short)(_received[0] + (_received[1] << 8));
            byte[] UsefulData = new byte[Length];
            for (int i = 2; i < Length; i++)
            {
                UsefulData[i - 2] = _received[i];
            }
            return new Message(Length, UsefulData, _received);
        }
    }
}
