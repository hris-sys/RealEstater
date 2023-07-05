using RealEstater_backend.Data.DTOs;

namespace RealEstater_backend.Helpers
{
    public static class PdfGenerator
    {
        private static string _htmlTemplate = "<!DOCTYPE html>\r\n<html>\r\n\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/css/bootstrap.min.css\"\r\n        integrity=\"sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T\" crossorigin=\"anonymous\">\r\n</head>\r\n\r\n<body>\r\n    <div class=\"card card-registration border-0\" id=\"card\">\r\n        <div class=\"container h-100\">\r\n                <div class=\"d-flex justify-content-center align-items-center\">\r\n                    <img src=\"{13}\" alt=\"...\" style=\"max-width: 75%; max-height: 45%;\">\r\n                </div>\r\n                <h3 class=\"text-center mt-2 mb-2\">{0}</h3>\r\n                <h6 class=\"text-center mt-2 mb-2\">{1}, \"{2}\"</h6>\r\n                <p class=\"text-center mt-2 mb-2\">\r\n                    Last updated on: {3}\r\n                </p>\r\n                <div class=\"row\">\r\n                    <div class=\"col-6 pt-4\">\r\n                        <div class=\"mt-2\"><b>Estate type: </b>{4}</div>\r\n                        <div class=\"mt-2\"><b>Built in: </b>{5}</div>\r\n                        <div class=\"mt-2\"><b>Main construction material: </b>{6}</div>\r\n                        <div class=\"mt-2\"><b>Current construction stage: </b>{7}</div>\r\n                        <div class=\"mt-2\"><b>Area: </b>{8} m<sup>2</sup></div>\r\n\t\t\t\t\t\t{20}\r\n                        <div class=\"mt-2\">\r\n                            <b>Price: </b><span class=\"text-success\">\r\n                                {9} BGN\r\n                            </span>\r\n                        </div>\r\n                        {11}{12}\r\n                        <div class=\"mt-2\"><b>Posted on: </b>{10}</div>\r\n                    </div>\r\n                    <div class=\"col\">\r\n                        <div class=\"d-flex justify-content-center align-items-center\">\r\n                            <div class=\"row py-4\">\r\n                                <p class=\"text-center\">{14}</p>\r\n                            </div>\r\n                        </div>\r\n\t\t\t\t\t\t<div class=\"mt-2 justify-content-center align-items-center\">\r\n                            <div class=\"d-flex flex-column align-items-center\">\r\n                                <h6 class=\"text-center\">{16} {17}</h6>\r\n                                <img src=\"{15}\" class=\"rounded\" style=\"max-width: 8rem; max-height: 8rem;\">\r\n                                <div><b>Number:</b> {18}</div>\r\n                                <div><b>Email:</b> {19}</div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    </div>\r\n</body>\r\n<script src=\"https://code.jquery.com/jquery-3.3.1.slim.min.js\"\r\n    integrity=\"sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo\"\r\n    crossorigin=\"anonymous\"></script>\r\n<script src=\"https://cdn.jsdelivr.net/npm/popper.js@1.14.7/dist/umd/popper.min.js\"\r\n    integrity=\"sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1\"\r\n    crossorigin=\"anonymous\"></script>\r\n<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js\"\r\n    integrity=\"sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM\"\r\n    crossorigin=\"anonymous\"></script>\r\n\r\n</html>";
        private static string _floorTemplate = "<div class=\"mt-2\">\r\n                            <b>Residing on floor: </b>{0}\r\n                        </div>";
        private static string _totalFloorTemplate = "<div class=\"mt-2\">\r\n                            <b>\r\n                                Total amount of floors:\r\n                            </b>{0}\r\n                        </div>";
        private static string _courtyardTemplate = "<div class=\"mt-2\"><b>Area: </b>{0} m<sup>2</sup></div>";

        public static PdfDocument GeneratePdf(DisplayLandholdingDto landholding)
        { 
            var renderer = new HtmlToPdf();
            renderer.PrintOptions.EnableJavaScript = true;
            renderer.PrintOptions.RenderDelay = 2000;
            renderer.PrintOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;

            var hasFloors = landholding.Floor > 0;
            var hasTotalFloors = landholding.NumberOfFloors > 0;
            var hasCourtyard = landholding.Courtyard > 0;

            var pdf = renderer.RenderHtmlAsPdf(string.Format(_htmlTemplate, landholding.Title.ToString(), landholding.City.ToString(), 
                landholding.Address.ToString(), landholding.LastUpdatedAt.Date.ToString(), 
                landholding.LandholdingType.ToString(), landholding.YearOfConstruction.ToString(), 
                landholding.ConstructionMaterial.ToString(), landholding.ConstructionStage.ToString(), 
                landholding.Area.ToString(), landholding.Price.ToString(), landholding.CreatedAt.Date.ToString(), 
                hasFloors ? string.Format(_floorTemplate, landholding.Floor) : "", hasTotalFloors ? string.Format(_totalFloorTemplate, landholding.NumberOfFloors) : "", 
                landholding.Pictures[0], landholding.Description, landholding.User.PictureURL, landholding.User.FirstName, landholding.User.LastName, landholding.User.PhoneNumber, landholding.User.Email,
                hasCourtyard ? string.Format(_courtyardTemplate, landholding.Courtyard) : ""));

            return pdf;
        }
    }
}
