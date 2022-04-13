namespace Rezun.SmsTools
{
    /// <summary>
    /// Represents a possible SMS message encoding type
    /// </summary>
    public enum SmsEncodingType : byte
    {
        /// <summary>
        /// Default GSM charset
        /// </summary>
        GSM7,

        /// <summary>
        /// UCS-2 Unicode
        /// </summary>
        UCS2
    }
}
