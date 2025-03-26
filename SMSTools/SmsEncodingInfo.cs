namespace Rezun.SmsTools;

/// <summary>
/// Provides information how a text will be encoded in the GSM 03.38 standard.
/// </summary>
/// <remarks>
/// Constructs with values.
/// </remarks>
public readonly record struct SmsEncodingInfo
{
    /// <summary>
    /// Gets how many SMS messages the text would need.
    /// </summary>
    public int PartsCount { get; init; }

    /// <summary>
    /// Gets how many total septets the text encodes to.
    /// This value includes header data.
    /// Only has a valid value when <see cref="Encoding" /> is <see cref="SmsEncodingType.Gsm7" />.
    /// </summary>
    /// <remarks>
    /// This value can be seen as "characters used".
    /// </remarks>
    public int SeptetsCount { get; init; }

    /// <summary>
    /// Gets how many total octets (bytes) the text would encode to.
    /// This value includes header data.
    /// </summary>
    public int OctetsCount { get; init; }

    /// <summary>
    /// Gets the type of encoding the text would need to use.
    /// </summary>
    public SmsEncodingType Encoding { get; init; }

    /// <summary>
    /// Gets how many chars would be left in the last SMS until it's full.
    /// </summary>
    /// <remarks>
    /// For <see cref="SmsEncodingType.Gsm7" /> this value only represents characters
    /// from the basic character set. Characters of the extended character set take up
    /// two regular character spaces.
    /// </remarks>
    public int CharsLeft { get; init; }
}
