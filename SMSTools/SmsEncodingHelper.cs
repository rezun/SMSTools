using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Rezun.SmsTools;

/// <summary>
/// Provides GSM 03.38 encoding infos and functionality.
/// </summary>
public static class SmsEncodingHelper
{
    /// <summary>
    /// The encoded size of an SMS message in septets.
    /// </summary>
    public const int MessageOctetSize = 140;

    /// <summary>
    /// The encoded size of an SMS message in octets (bytes).
    /// </summary>
    public const int MessageSeptetSize = 160;

    /// <summary>
    /// The header octet size of each SMS message when its part of a concatenated message.
    /// </summary>
    public const int MessageConcatHeaderOctetSize = 6;

    /// <summary>
    /// The header septet size of each SMS message when its part of a concatenated message.
    /// </summary>
    public const int MessageConcatHeaderSeptetSize = 7;

    /// <summary>
    /// The size of a single character from the basic character set encoded in GSM.
    /// </summary>
    public const int GsmBasicCharBitSize = 7;

    /// <summary>
    /// Returns true if the provided text is encodable in GSM 7 bit.
    /// </summary>
    public static bool IsGsmEncodable(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return IsGsmEncodable(text.AsSpan());
    }

    /// <summary>
    /// Returns true if the provided text is encodable in GSM 7 bit.
    /// </summary>
    public static bool IsGsmEncodable(ReadOnlySpan<char> text)
    {
        foreach (var currentChar in text)
        {
            if (!Gsm7CharactersSet.Contains(currentChar)
                && !Gsm7ExtCharactersSet.Contains(currentChar))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Calculates information how the text is going to be encoded in an SMS message.
    /// </summary>
    /// <param name="text">Message text.</param>
    /// <param name="concatenated">Weather this should be calculated for a concatenated SMS.</param>
    public static SmsEncodingInfo GetEncodingInfo(string text, bool concatenated = true)
    {
        ArgumentNullException.ThrowIfNull(text);

        return GetEncodingInfo(text.AsSpan(), concatenated);
    }

    /// <summary>
    /// Calculates information how the text is going to be encoded in an SMS message.
    /// </summary>
    /// <param name="text">Message text.</param>
    /// <param name="concatenated">
    /// Weather this should be calculated for a concatenated SMS message
    /// or individual single messages.
    /// </param>
    public static SmsEncodingInfo GetEncodingInfo(ReadOnlySpan<char> text, bool concatenated = true)
    {
        if (text.IsEmpty)
        {
            return new SmsEncodingInfo
            {
                PartsCount = 1,
                SeptetsCount = 0,
                OctetsCount = 0,
                CharsLeft = 0,
                Encoding = SmsEncodingType.Gsm7,
            };
        }

        var octets = 0;
        var septets = 0;
        var parts = 1;
        var charsLeft = 0;
        var encoding = SmsEncodingType.Gsm7;

        foreach (var currentChar in text)
        {
            if (Gsm7CharactersSet.Contains(currentChar))
            {
                septets++;
            }
            else if (Gsm7ExtCharactersSet.Contains(currentChar))
            {
                septets += 2;
            }
            else
            {
                septets = -1;
                encoding = SmsEncodingType.Ucs2;
                break;
            }
        }

        octets = encoding switch
        {
            SmsEncodingType.Gsm7 => (int)Math.Ceiling(septets * (7m / 8m)),
            SmsEncodingType.Ucs2 => text.Length * 2,
            _ => octets,
        };

        if (octets > MessageOctetSize)
        {
            if (concatenated)
            {
                parts = DivideCeiling(octets, MessageOctetSize - MessageConcatHeaderOctetSize);
                octets += parts * MessageConcatHeaderOctetSize;

                if (encoding == SmsEncodingType.Gsm7)
                {
                    septets += parts * MessageConcatHeaderSeptetSize;
                }
            }
            else
            {
                parts = DivideCeiling(octets, MessageOctetSize);
            }
        }

        charsLeft = encoding switch
        {
            SmsEncodingType.Gsm7 => parts * MessageSeptetSize - septets,
            SmsEncodingType.Ucs2 => (parts * MessageOctetSize - octets) / 2,
            _ => charsLeft,
        };

        return new SmsEncodingInfo
        {
            PartsCount = parts,
            SeptetsCount = septets,
            OctetsCount = octets,
            CharsLeft = charsLeft,
            Encoding = encoding,
        };
    }

    /// <summary>
    /// Divides two integer values and gives the ceiling result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int DivideCeiling(int dividend, int divisor)
    {
        return (checked(dividend + divisor) - 1) / divisor;
    }

    #region character sets

    private static readonly HashSet<char> Gsm7CharactersSet =
    [
        '@', '£', '$', '¥', 'è', 'é', 'ù', 'ì', 'ò', 'Ç', '\r', 'Ø', 'ø', '\n', 'Å', 'å',
        'Δ', '_', 'Φ', 'Γ', 'Λ', 'Ω', 'Π', 'Ψ', 'Σ', 'Θ', 'Ξ', 'Æ', 'æ', 'ß', 'É', ' ',
        '!', '"', '#', '¤', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
        '¡', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
        'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Ä', 'Ö', 'Ñ', 'Ü', '§',
        '¿', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o',
        'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'ä', 'ö', 'ñ', 'ü', 'à',
    ];

    private static readonly HashSet<char> Gsm7ExtCharactersSet =
    [
        '€', '\f', '[', '\\', ']', '^', '{', '|', '}', '~',
    ];

    /// <summary>
    /// Gets the characters from the GSM 7 basic character set.
    /// </summary>
    public static IReadOnlySet<char> Gsm7Characters => Gsm7CharactersSet;

    /// <summary>
    /// Gets the characters from the GSM 7 extended character set.
    /// </summary>
    public static IReadOnlySet<char> Gsm7ExtCharacters => Gsm7ExtCharactersSet;

    #endregion

}
