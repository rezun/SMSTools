namespace Rezun.SmsTools
{
    /// <summary>
    /// Provides informations how a text will be encoded in the GSM 03.38 standard.
    /// </summary>
    public readonly struct SmsEncodingInfo
    {
        /// <summary>
        /// Gets how many SMS messages the text would need.
        /// </summary>
        public int PartsCount { get; }

        /// <summary>
        /// Gets how many total septets the text encodes to.
        /// This value includes header data.
        /// Only has a valid value when <see cref="Encoding"/> is <see cref="SmsEncodingType.GSM7"/>.
        /// </summary>
        /// <remarks>
        /// This value can be seen as "characters used".
        /// </remarks>
        public int SeptetsCount { get; }

        /// <summary>
        /// Gets how many total octets (bytes) the text would encode to.
        /// This value includes header data.
        /// </summary>
        public int OctetsCount { get; }

        /// <summary>
        /// Gets the type of encoding the text would need to use.
        /// </summary>
        public SmsEncodingType Encoding { get; }

        /// <summary>
        /// Gets how many chars would be left in the last SMS untill it's full.
        /// </summary>
        /// <remarks>
        /// For <see cref="SmsEncodingType.GSM7"/> this value only represents characters
        /// from the basic character set. Characters of the extended character set take up
        /// two regular character spaces.
        /// </remarks>
        public int CharsLeft { get; }

        /// <summary>
        /// Constructs with values.
        /// </summary>
        public SmsEncodingInfo(
            int partsCount,
            int septetsCount,
            int octetsCount,
            int charsLeft,
            SmsEncodingType encoding)
        {
            PartsCount = partsCount;
            SeptetsCount = septetsCount;
            OctetsCount = octetsCount;
            CharsLeft = charsLeft;
            Encoding = encoding;
        }
    }
}
