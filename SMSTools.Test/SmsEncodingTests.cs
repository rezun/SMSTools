using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rezun.SmsTools.Tests
{
    [TestClass]
    public class SmsEncodingTests
    {
        private static readonly string SingleMessage = "Hello message with €-sign";

        private static readonly string FullSingleMessage =
            "€Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor " +
            "invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At";

        private static readonly string FullConcatenatedDoubleMessage =
            "€Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor " +
            "invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam " +
            "et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem " +
            "ipsum dolor sit amet. Lorem ip";

        private static readonly string UnicodeMessage = "漢字";

        private static readonly string FullUnicodeSingleMessage =
            "漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字漢字" +
            "漢字漢字漢字漢字漢字漢字漢字漢字";

        [TestMethod]
        public void GsmEncodable_EncodableText()
        {
            var result = SmsEncodingHelper.Instance.IsGsmEncodable(SingleMessage);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GsmEncodable_NonEncodableText()
        {
            var result = SmsEncodingHelper.Instance.IsGsmEncodable(UnicodeMessage);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_SingleMessage()
        {
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(SingleMessage);
            Assert.AreEqual(result.Encoding, SmsEncodingType.GSM7);
            Assert.AreEqual(result.PartsCount, 1);
            Assert.AreEqual(result.OctetsCount, 23);
            Assert.AreEqual(result.SeptetsCount, 26);
            Assert.AreEqual(result.CharsLeft, 134);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_SingleMessageFull()
        {
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(FullSingleMessage);
            Assert.AreEqual(result.Encoding, SmsEncodingType.GSM7);
            Assert.AreEqual(result.PartsCount, 1);
            Assert.AreEqual(result.OctetsCount, 140);
            Assert.AreEqual(result.SeptetsCount, 160);
            Assert.AreEqual(result.CharsLeft, 0);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_DoubleMessage()
        {
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(FullConcatenatedDoubleMessage);
            Assert.AreEqual(result.Encoding, SmsEncodingType.GSM7);
            Assert.AreEqual(result.PartsCount, 2);
            Assert.AreEqual(result.OctetsCount, 280);
            Assert.AreEqual(result.SeptetsCount, 320);
            Assert.AreEqual(result.CharsLeft, 0);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_DoubleMessageNotConcatinated()
        {
            var doubleMessage = FullSingleMessage + FullSingleMessage;
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(doubleMessage, concatenated: false);
            Assert.AreEqual(result.Encoding, SmsEncodingType.GSM7);
            Assert.AreEqual(result.PartsCount, 2);
            Assert.AreEqual(result.OctetsCount, 280);
            Assert.AreEqual(result.SeptetsCount, 320);
            Assert.AreEqual(result.CharsLeft, 0);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_UnicodeSingle()
        {
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(UnicodeMessage);
            Assert.AreEqual(result.Encoding, SmsEncodingType.UCS2);
            Assert.AreEqual(result.PartsCount, 1);
            Assert.AreEqual(result.OctetsCount, 4);
            Assert.AreEqual(result.SeptetsCount, -1);
            Assert.AreEqual(result.CharsLeft, 68);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_UnicodeSingleFull()
        {
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(FullUnicodeSingleMessage);
            Assert.AreEqual(result.Encoding, SmsEncodingType.UCS2);
            Assert.AreEqual(result.PartsCount, 1);
            Assert.AreEqual(result.OctetsCount, 140);
            Assert.AreEqual(result.SeptetsCount, -1);
            Assert.AreEqual(result.CharsLeft, 0);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_UnicodeMulti()
        {
            var message = FullUnicodeSingleMessage + FullUnicodeSingleMessage;
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(message);
            Assert.AreEqual(result.Encoding, SmsEncodingType.UCS2);
            Assert.AreEqual(result.PartsCount, 3);
            Assert.AreEqual(result.OctetsCount, 298);
            Assert.AreEqual(result.SeptetsCount, -1);
            Assert.AreEqual(result.CharsLeft, 61);
        }

        [TestMethod]
        public void GetSmsEncodingInfo_UnicodeDoubleNotConcatinated()
        {
            var message = FullUnicodeSingleMessage + FullUnicodeSingleMessage;
            var result = SmsEncodingHelper.Instance.GetEncodingInfo(message, concatenated: false);
            Assert.AreEqual(result.Encoding, SmsEncodingType.UCS2);
            Assert.AreEqual(result.PartsCount, 2);
            Assert.AreEqual(result.OctetsCount, 280);
            Assert.AreEqual(result.SeptetsCount, -1);
            Assert.AreEqual(result.CharsLeft, 0);
        }
    }
}
