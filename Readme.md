# SMSTools

A library to calculate SMS message sizes and characters (left)

Available as nuget package [Rezun.SmsTools](https://www.nuget.org/packages/Rezun.SmsTools/)

## Features

* Check if text is GSM encodable
* Calculate SMS encoding info (parts, size, chars left)
* Support for GSM-7 and UCS-2 encoding
* Support for concatenated messages
* Support for extended GSM characters

## Usage

### Check if text is GSM encodable

```csharp
string message = "Hello message with €-sign";
bool isGsmEncodable = SmsEncodingHelper.IsGsmEncodable(message); // true

string unicodeMessage = "漢字";
bool isUnicodeEncodable = SmsEncodingHelper.IsGsmEncodable(unicodeMessage); // false
```

### Get SMS encoding information

```csharp
string message = "Hello message with €-sign";
var info = SmsEncodingHelper.GetEncodingInfo(message);

Console.WriteLine($"Encoding: {info.Encoding}");        // GSM7
Console.WriteLine($"Parts: {info.PartsCount}");         // 1
Console.WriteLine($"Octets: {info.OctetsCount}");       // 23
Console.WriteLine($"Septets: {info.SeptetsCount}");     // 26
Console.WriteLine($"Chars left: {info.CharsLeft}");     // 134
```
