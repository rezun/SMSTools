using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Rezun.SmsTools
{
    /// <summary>
    /// Provides GSM 03.38 encoding infos and functionality.
    /// </summary>
    public class SmsEncodingHelper
    {
        private static readonly Lazy<SmsEncodingHelper> instance = new Lazy<SmsEncodingHelper>();

        #region character sets

        /// <summary>
        /// Gets the characters from the GSM 7 basic character set.
        /// </summary>
        public static readonly IReadOnlyDictionary<char, byte> Gsm7Characters =
            new Dictionary<char, byte>(128)
            {
                { '@', 0x00},
                { '£', 0x01},
                { '$', 0x02},
                { '¥', 0x03},
                { 'è', 0x04},
                { 'é', 0x05},
                { 'ù', 0x06},
                { 'ì', 0x07},
                { 'ò', 0x08},
                { 'Ç', 0x09},
                { '\r', 0x0A},
                { 'Ø', 0x0B},
                { 'ø', 0x0C},
                { '\n', 0x0D},
                { 'Å', 0x0E},
                { 'å', 0x0F},
                { 'Δ', 0x10},
                { '_', 0x11},
                { 'Φ', 0x12},
                { 'Γ', 0x13},
                { 'Λ', 0x14},
                { 'Ω', 0x15},
                { 'Π', 0x16},
                { 'Ψ', 0x17},
                { 'Σ', 0x18},
                { 'Θ', 0x19},
                { 'Ξ', 0x1A},
                { 'Æ', 0x1C},
                { 'æ', 0x1D},
                { 'ß', 0x1E},
                { 'É', 0x1F},
                { ' ', 0x20},
                { '!', 0x21},
                { '"', 0x22},
                { '#', 0x23},
                { '¤', 0x24},
                { '%', 0x25},
                { '&', 0x26},
                { '\'', 0x27},
                { '(', 0x28},
                { ')', 0x29},
                { '*', 0x2A},
                { '+', 0x2B},
                { ',', 0x2C},
                { '-', 0x2D},
                { '.', 0x2E},
                { '/', 0x2F},
                { '0', 0x30},
                { '1', 0x31},
                { '2', 0x32},
                { '3', 0x33},
                { '4', 0x34},
                { '5', 0x35},
                { '6', 0x36},
                { '7', 0x37},
                { '8', 0x38},
                { '9', 0x39},
                { ':', 0x3A},
                { ';', 0x3B},
                { '<', 0x3C},
                { '=', 0x3D},
                { '>', 0x3E},
                { '?', 0x3F},
                { '¡', 0x40},
                { 'A', 0x41},
                { 'B', 0x42},
                { 'C', 0x43},
                { 'D', 0x44},
                { 'E', 0x45},
                { 'F', 0x46},
                { 'G', 0x47},
                { 'H', 0x48},
                { 'I', 0x49},
                { 'J', 0x4A},
                { 'K', 0x4B},
                { 'L', 0x4C},
                { 'M', 0x4D},
                { 'N', 0x4E},
                { 'O', 0x4F},
                { 'P', 0x50},
                { 'Q', 0x51},
                { 'R', 0x52},
                { 'S', 0x53},
                { 'T', 0x54},
                { 'U', 0x55},
                { 'V', 0x56},
                { 'W', 0x57},
                { 'X', 0x58},
                { 'Y', 0x59},
                { 'Z', 0x5A},
                { 'Ä', 0x5B},
                { 'Ö', 0x5C},
                { 'Ñ', 0x5D},
                { 'Ü', 0x5E},
                { '§', 0x5F},
                { '¿', 0x60},
                { 'a', 0x61},
                { 'b', 0x62},
                { 'c', 0x63},
                { 'd', 0x64},
                { 'e', 0x65},
                { 'f', 0x66},
                { 'g', 0x67},
                { 'h', 0x68},
                { 'i', 0x69},
                { 'j', 0x6A},
                { 'k', 0x6B},
                { 'l', 0x6C},
                { 'm', 0x6D},
                { 'n', 0x6E},
                { 'o', 0x6F},
                { 'p', 0x70},
                { 'q', 0x71},
                { 'r', 0x72},
                { 's', 0x73},
                { 't', 0x74},
                { 'u', 0x75},
                { 'v', 0x76},
                { 'w', 0x77},
                { 'x', 0x78},
                { 'y', 0x79},
                { 'z', 0x7A},
                { 'ä', 0x7B},
                { 'ö', 0x7C},
                { 'ñ', 0x7D},
                { 'ü', 0x7E},
                { 'à', 0x7F},
            };

        /// <summary>
        /// Gets the characters from the GSM 7 extended character set.
        /// </summary>
        public static readonly IReadOnlyDictionary<char, byte> Gsm7ExtCharacters = 
            new Dictionary<char, byte>(10)
            {
                { '€', 0x65},
                { '\f', 0x0A},
                { '[', 0x3C},
                { '\\', 0x2F},
                { ']', 0x3E},
                { '^', 0x14},
                { '{', 0x28},
                { '|', 0x40},
                { '}', 0x29},
                { '~', 0x3D},
            };

        #endregion

        /// <summary>
        /// The encoded size of an SMS message in septets.
        /// </summary>
        public const int MESSAGE_OCTET_SIZE = 140;

        /// <summary>
        /// The encoded size of an SMS message in octets (bytes).
        /// </summary>
        public const int MESSAGE_SEPTET_SIZE = 160;

        /// <summary>
        /// The header octet size of each SMS message when its part of a concatenated message.
        /// </summary>
        public const int MESSAGE_CONCAT_HEADER_OCTET_SIZE = 6;

        /// <summary>
        /// The header septet size of each SMS message when its part of a concatenated message.
        /// </summary>
        public const int MESSAGE_CONCAT_HEADER_SEPTET_SIZE = 7;

        /// <summary>
        /// The size of a single character from the basic character set encoded in GSM.
        /// </summary>
        public const int GSM_BASIC_CHAR_BIT_SIZE = 7;

        /// <summary>
        /// A singleton instance of this class for easy use.
        /// </summary>
        public static SmsEncodingHelper Instance => instance.Value;

        /// <summary>
        /// Returns true if the provided text is encodable in GSM 7 bit.
        /// </summary>
        public bool IsGsmEncodable(string text)
        {
            if (text == null)
                throw new ArgumentNullException(text);

            return IsGsmEncodable(text.AsSpan());
        }

        /// <summary>
        /// Returns true if the provided text is encodable in GSM 7 bit.
        /// </summary>
        public bool IsGsmEncodable(ReadOnlySpan<char> text)
        {
            foreach (var currentChar in text)
            {
                if (!Gsm7Characters.ContainsKey(currentChar)
                    && !Gsm7ExtCharacters.ContainsKey(currentChar))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Calculates informations how the text is going to be encoded in an SMS message.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="concatenated">Wether this should be calculated for a concatenated SMS.</param>
        public SmsEncodingInfo GetEncodingInfo(string text, bool concatenated = true)
        {
            if (text == null)
                throw new ArgumentNullException(text);

            return GetEncodingInfo(text.AsSpan(), concatenated);
        }

        /// <summary>
        /// Calculates informations how the text is going to be encoded in an SMS message.
        /// </summary>
        /// <param name="text">Message text.</param>
        /// <param name="concatenated">
        /// Wether this should be calculated for a concatenated SMS message
        /// or individual single messages.
        /// </param>
        public SmsEncodingInfo GetEncodingInfo(ReadOnlySpan<char> text, bool concatenated = true)
        {
            int octets = 0;
            int septets = 0;
            int parts = 1;
            int charsLeft = 0;
            var encoding = SmsEncodingType.GSM7;

            foreach (var currentChar in text)
            {
                if (Gsm7Characters.ContainsKey(currentChar))
                {
                    septets++;
                }
                else if (Gsm7ExtCharacters.ContainsKey(currentChar))
                {
                    septets += 2;
                }
                else
                {
                    septets = -1;
                    encoding = SmsEncodingType.UCS2;
                    break;
                }
            }

            switch (encoding)
            {
                case SmsEncodingType.GSM7:
                    octets = (int)Math.Ceiling(septets * (7m / 8m));
                    break;
                case SmsEncodingType.UCS2:
                    octets = text.Length * 2;
                    break;
            }

            if (octets > MESSAGE_OCTET_SIZE)
            {
                if (concatenated)
                {
                    parts = DivideCeiling(octets, MESSAGE_OCTET_SIZE - MESSAGE_CONCAT_HEADER_OCTET_SIZE);
                    octets += parts * MESSAGE_CONCAT_HEADER_OCTET_SIZE;

                    if (encoding == SmsEncodingType.GSM7)
                        septets += parts * MESSAGE_CONCAT_HEADER_SEPTET_SIZE;
                }
                else
                {
                    parts = DivideCeiling(octets, MESSAGE_OCTET_SIZE);
                }
            }

            switch (encoding)
            {
                case SmsEncodingType.GSM7:
                    charsLeft = parts * MESSAGE_SEPTET_SIZE - septets;
                    break;
                case SmsEncodingType.UCS2:
                    charsLeft = (parts * MESSAGE_OCTET_SIZE - octets) / 2;
                    break;
            }

            return new SmsEncodingInfo(
                parts,
                septets,
                octets,
                charsLeft,
                encoding
                );
        }

        /// <summary>
        /// Divides two integer values and gives the ceiling result.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DivideCeiling(int dividend, int divisor)
        {
            return (checked(dividend + divisor) - 1) / divisor;
        }
    }
}
