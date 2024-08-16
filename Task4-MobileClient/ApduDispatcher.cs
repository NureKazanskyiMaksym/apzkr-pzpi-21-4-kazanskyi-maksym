using EquipmentWatcherMAUI;
using System.Buffers.Binary;
using System.Text;

namespace MauiNfcHceBootStrapExample
{
    // Application Status Word
    enum ApplicationSW : ushort
    {
        Success = 0x9000,
        EOF = 0x6282,
        CommandDataNotValid = 0x6A80,
        UnknownError = 0x6F00
    }

    internal class ApduDispatcher
    {
        public static (List<byte>, string) Process(byte[] input)
        {
            ushort answerSW = (ushort)ApplicationSW.Success;
            List<byte> answer = new List<byte>();
            List<byte> response = new List<byte>();
            string uiMessage = string.Empty;

            try
            {
                if (Util.ByteArrayStartsWith(input, [0x00, 0xA4, 0x04]))
                {
                    uiMessage = "Select Received..";
                }

                int LcBytes = 0, LcDataLength = 0, LeBytes, LeDataLength = 0;

                if (input.Length > 4)
                {
                    if (input[4] > 0)
                    {
                        LcBytes = 1;
                        LcDataLength = input[4];
                    }
                    else if (input[4] == 0 && input.Length >= 6)
                    {
                        LcBytes = 3;
                        LcDataLength = BinaryPrimitives.ReverseEndianness(BitConverter.ToInt16(input, 5));
                    }
                }
                else
                {
                    LcBytes = 0;
                }

                if (LcBytes == 0 || LcDataLength == 0)
                {
                    answerSW = (ushort)ApplicationSW.CommandDataNotValid;
                    answer.Add((byte)(answerSW >> 8));
                    answer.Add((byte)(answerSW));
                    uiMessage = "Incorrect parameters in the command data field";
                    return (answer, uiMessage);
                }

                var LeOffset = 4 + LcBytes + LcDataLength - 1;

                if (input.Length > LeOffset)
                {
                    if (input[LeOffset] == 0)
                    {
                        LeBytes = 1;
                        LeDataLength = input[LeOffset] == 0 ? 256 : input[LeOffset];
                    }
                    else if (LcBytes == 0 && input[LeOffset] == 0)
                    {
                        LeBytes = 3;
                        LeDataLength = BinaryPrimitives.ReadInt16BigEndian(input.AsSpan(LeOffset + 1, 2));
                    }
                    else if (LcBytes != 0)
                    {
                        LeBytes = 2;
                        LeDataLength = BinaryPrimitives.ReadInt16BigEndian(input.AsSpan(LeOffset, 2));
                    }
                    else
                    {
                        LeBytes = 0;
                    }
                }
                else
                {
                    LeBytes = 0;
                    LeDataLength = 0;
                }

                if (LeBytes == 0)
                {
                    answerSW = (ushort)ApplicationSW.EOF;
                    answer.Add((byte)(answerSW >> 8));
                    answer.Add((byte)(answerSW));
                    uiMessage = "End of file or record reached before reading Ne bytes";
                    return (answer, uiMessage);
                }

                response.Capacity = LeDataLength;

                var tokenBytes = Encoding.UTF8.GetBytes(StaticReference.CurrentAccessToken.Token);
                response.AddRange(tokenBytes);
                answerSW = (ushort)ApplicationSW.Success;

                uiMessage += "All ok";
            } 
            catch
            {
                response.Clear();
                answerSW = (ushort)ApplicationSW.UnknownError;
                uiMessage = "Undiagnosed Error for cApdu : " + Util.ByteArrayToHexString(input);
            }

            answer.AddRange(response);
            answer.Add((byte)(answerSW >> 8));
            answer.Add((byte)(answerSW));

            return (answer, uiMessage);
        }        
    }
}
