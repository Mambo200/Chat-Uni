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
        public int SenderID { get; private set; }
        public ECommands Command { get; private set; }
        public string[] Parameter { get; private set; }
        public byte[] Packet { get; private set; }

        private Message(short _length, string[] _parameters, byte[] _packet, int _senderID, ECommands _command)
        {
            Length = _length;
            Parameter = _parameters;
            Packet = _packet;
            SenderID = _senderID;
            Command = _command;
        }

        public static Message Encode(ECommands _command, int _senderID, params object[] _parameter)
        {
            byte[] toSend;
            string wholeDataBlock =
                ((int)_command) + " " + _senderID;

            foreach (object obj in _parameter)
            {
                wholeDataBlock += " " + obj;
            }

            toSend = System.Text.Encoding.UTF8.GetBytes(wholeDataBlock);

            if (toSend.Length > PACKET_SIZE + 2)
            {
#if DEBUG
                throw new ArgumentException();
#else
                return new Message(-1, null, null);
#endif
            }
            byte[] packet = new byte[PACKET_SIZE];
            packet[0] = (byte)toSend.Length;
            packet[1] = (byte)(toSend.Length >> 8);

            for (int i = 0; i < toSend.Length; i++)
            {
                packet[i + 2] = toSend[i];
            }

            return new Message((short)toSend.Length, toSend.Cast<string>().ToArray(), packet, _senderID, _command);
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

            string wholeDataBlock = Encoding.UTF8.GetString(_received, 2, Length);

            string[] split = wholeDataBlock.Split(' ');
            if (split.Length < 2)
            {
#if DEBUG
                throw new ArgumentException();
#else
                return new Message(-1, null, null);
#endif
            }

            ECommands command = (ECommands)int.Parse(split[0]);
            int senderID = int.Parse(split[1]);

            string[] parameters = new string[split.Length - 2];
            if (parameters.Length > 0)
            {
                split.CopyTo(parameters, 2);
            }

            return new Message(Length, parameters, _received, senderID, command);
        }
    }
}
