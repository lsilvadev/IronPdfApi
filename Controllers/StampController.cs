using Microsoft.AspNetCore.Mvc;
using IronPdf.Editing;
using IronApi.Models;
using Newtonsoft.Json;

namespace IronApi.Controllers
{
    [Route("api/[controller]")]
    public class StampController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                ApiStamp? data = JsonConvert.DeserializeObject<ApiStamp>(System.IO.File.ReadAllText($"Files/stamp.json") ?? string.Empty);

                using var pdf = new IronPdf.PdfDocument(System.IO.File.ReadAllBytes("Files/file.pdf"));
                
                string GenerateContentFieldsToHtml(ApiStampField field)
                {
                    return $"<div style='position:absolute; font-family: \"Atkinson Hyperlegible\", sans-serif; font-size: {field.fontSize}pt; width: {field.width}pt; left: {field.x}pt; top: {field.y}pt; margin: 0; padding: 0;'>{field.value}</div>";
                }

                string content = string.Empty;

                var indexPdfPage = 0;


                for (int indexPayload = 1; indexPayload <= pdf.PageCount; indexPayload++)
                {
                    indexPdfPage = indexPayload - 1;
        
                    var fieldsToStamp = data.Fields.Where(_ => _.page == indexPayload);
            
                    foreach (var field in fieldsToStamp)
                        content = string.Concat(content, GenerateContentFieldsToHtml(field));

                    var html = $"<div style='position:absolute; font-size: 12px; width: 100vw; height: 100vh;'>{content}</div>";

                    if (pdf.PageCount <= 1)
                    {
                        pdf.StampHTML(new HtmlStamp(html));
                        break;
                    }
                    
                    using IronPdf.PdfDocument pdfPage = pdf.CopyPage(indexPdfPage);

                    pdfPage.StampHTML(new HtmlStamp(html));

                    content = String.Empty;
                    pdf.RemovePage(indexPdfPage).InsertPdf(pdfPage, indexPdfPage);

                }

                HttpContext.Response.ContentType = "application/pdf";
                FileContentResult result = new( pdf.BinaryData.ToArray(), "application/pdf")
                {
                    FileDownloadName = "response.pdf"
                };
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"(IronPdf) Erro ao estampar o texto no pdf. Erro: {e.Message}");
            }
        }
        
    }
}