using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rezun.SmsTools.Test;

[TestClass]
public class SmsEncodingTests
{
    private const string SingleMessage = "Hello message with €-sign";

    private const string FullSingleMessage =
        "€Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor " +
        "invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At";

    private const string FullConcatenatedDoubleMessage =
        "€Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor " +
        "invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam " +
        "et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem " +
        "ipsum dolor sit amet. Lorem ip";

    private const string UnicodeMessage = "漢字";

    private const string FullUnicodeSingleMessage =
        "漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字" +
        "漢字漢字漢字漢字漢字漢字漢字漢字";

    [TestMethod]
    public void GsmEncodable_EncodableText()
    {
        var result = SmsEncodingHelper.IsGsmEncodable(SingleMessage);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GsmEncodable_NonEncodableText()
    {
        var result = SmsEncodingHelper.IsGsmEncodable(UnicodeMessage);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_SingleMessage()
    {
        var result = SmsEncodingHelper.GetEncodingInfo(SingleMessage);
        Assert.AreEqual(SmsEncodingType.Gsm7, result.Encoding);
        Assert.AreEqual(expected: 1, result.PartsCount);
        Assert.AreEqual(expected: 23, result.OctetsCount);
        Assert.AreEqual(expected: 26, result.SeptetsCount);
        Assert.AreEqual(expected: 134, result.CharsLeft);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_SingleMessageFull()
    {
        var result = SmsEncodingHelper.GetEncodingInfo(FullSingleMessage);
        Assert.AreEqual(SmsEncodingType.Gsm7, result.Encoding);
        Assert.AreEqual(expected: 1, result.PartsCount);
        Assert.AreEqual(expected: 140, result.OctetsCount);
        Assert.AreEqual(expected: 160, result.SeptetsCount);
        Assert.AreEqual(expected: 0, result.CharsLeft);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_DoubleMessage()
    {
        var result = SmsEncodingHelper.GetEncodingInfo(FullConcatenatedDoubleMessage);
        Assert.AreEqual(SmsEncodingType.Gsm7, result.Encoding);
        Assert.AreEqual(expected: 2, result.PartsCount);
        Assert.AreEqual(expected: 280, result.OctetsCount);
        Assert.AreEqual(expected: 320, result.SeptetsCount);
        Assert.AreEqual(expected: 0, result.CharsLeft);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_DoubleMessageNotConcatenated()
    {
        var doubleMessage = FullSingleMessage + FullSingleMessage;
        var result = SmsEncodingHelper.GetEncodingInfo(doubleMessage, concatenated: false);
        Assert.AreEqual(SmsEncodingType.Gsm7, result.Encoding);
        Assert.AreEqual(expected: 2, result.PartsCount);
        Assert.AreEqual(expected: 280, result.OctetsCount);
        Assert.AreEqual(expected: 320, result.SeptetsCount);
        Assert.AreEqual(expected: 0, result.CharsLeft);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_UnicodeSingle()
    {
        var result = SmsEncodingHelper.GetEncodingInfo(UnicodeMessage);
        Assert.AreEqual(SmsEncodingType.Ucs2, result.Encoding);
        Assert.AreEqual(expected: 1, result.PartsCount);
        Assert.AreEqual(expected: 4, result.OctetsCount);
        Assert.AreEqual(expected: -1, result.SeptetsCount);
        Assert.AreEqual(expected: 68, result.CharsLeft);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_UnicodeSingleFull()
    {
        var result = SmsEncodingHelper.GetEncodingInfo(FullUnicodeSingleMessage);
        Assert.AreEqual(SmsEncodingType.Ucs2, result.Encoding);
        Assert.AreEqual(expected: 1, result.PartsCount);
        Assert.AreEqual(expected: 140, result.OctetsCount);
        Assert.AreEqual(expected: -1, result.SeptetsCount);
        Assert.AreEqual(expected: 0, result.CharsLeft);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_UnicodeMulti()
    {
        var message = FullUnicodeSingleMessage + FullUnicodeSingleMessage;
        var result = SmsEncodingHelper.GetEncodingInfo(message);
        Assert.AreEqual(SmsEncodingType.Ucs2, result.Encoding);
        Assert.AreEqual(expected: 3, result.PartsCount);
        Assert.AreEqual(expected: 298, result.OctetsCount);
        Assert.AreEqual(expected: -1, result.SeptetsCount);
        Assert.AreEqual(expected: 61, result.CharsLeft);
    }

    [TestMethod]
    public void GetSmsEncodingInfo_UnicodeDoubleNotConcatenated()
    {
        var message = FullUnicodeSingleMessage + FullUnicodeSingleMessage;
        var result = SmsEncodingHelper.GetEncodingInfo(message, concatenated: false);
        Assert.AreEqual(SmsEncodingType.Ucs2, result.Encoding);
        Assert.AreEqual(expected: 2, result.PartsCount);
        Assert.AreEqual(expected: 280, result.OctetsCount);
        Assert.AreEqual(expected: -1, result.SeptetsCount);
        Assert.AreEqual(expected: 0, result.CharsLeft);
    }
}
