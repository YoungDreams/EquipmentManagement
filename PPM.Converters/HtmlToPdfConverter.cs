using System;
using System.IO;
using PdfSharp.Fonts;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PPM.Converters
{
    public class HtmlToPdfConverter
    {
        public void ToPdf(string html, string saveAsPath)
        {
            var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.RA4);
            pdf.Save(saveAsPath);
        }
    }

    public class CustomFontResolver: IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo(familyName);
        }

        public byte[] GetFont(string faceName)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),"simfang.ttf");
            return File.ReadAllBytes(path);
        }
    }
}
