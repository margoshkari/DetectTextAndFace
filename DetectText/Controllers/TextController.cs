using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetectText.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextController : ControllerBase
    {
        string info;
        Image image;

        private readonly ILogger<TextController> _logger;

        public TextController(ILogger<TextController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetText")]
        public ActionResult GetText()
        {
            info = string.Empty;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "info.json");
            image = Image.FromFile("image.jpg");

            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            IReadOnlyList<EntityAnnotation> textAnnotations = client.DetectText(image);
            foreach (EntityAnnotation text in textAnnotations)
            {
                info += $"Description: {text.Description}";
            }
            return Content(info);
        }

        [HttpGet("GetFace")]
        public ActionResult GetFace()
        {
            info = string.Empty;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "info.json");
            image = Image.FromFile("woman.jpeg");

            ImageAnnotatorClient client = ImageAnnotatorClient.Create();
            IReadOnlyList<FaceAnnotation> result = client.DetectFaces(image);
            foreach (FaceAnnotation face in result)
            {
                string poly = string.Join(" - ", face.BoundingPoly.Vertices.Select(v => $"({v.X}, {v.Y})"));
                info +=  $"Confidence: {(int)(face.DetectionConfidence * 100)}%; BoundingPoly: {poly}";
            }
           
            return Content(info);
        }
    }
}
