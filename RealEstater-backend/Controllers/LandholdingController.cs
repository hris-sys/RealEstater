using Microsoft.AspNetCore.Mvc;
using RealEstater_backend.Helpers;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Services.Interfaces;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandholdingController : Controller
    {
        private readonly IFeatureRepository featureRepository;
        private readonly ILandholdingService landholdingService;
        private readonly ILandholdingTypeRepository landholdingTypeRepository;
        private readonly IConstructionTypeRepository constructionTypeRepository;
        private readonly IConstructionStageRepository constructionStageRepository;

        public LandholdingController(IFeatureRepository featureRepository, ILandholdingService landholdingService,
                                     IConstructionStageRepository constructionStageRepository, ILandholdingTypeRepository landholdingTypeRepository,
                                     IConstructionTypeRepository constructionTypeRepository)
        {
            this.featureRepository = featureRepository;
            this.landholdingService = landholdingService;
            this.landholdingTypeRepository = landholdingTypeRepository;
            this.constructionTypeRepository = constructionTypeRepository;
            this.constructionStageRepository = constructionStageRepository;
        }

        [HttpGet("getAllConstructionTypes")]
        public async Task<IActionResult> GetAllConstructionTypes()
        {
            var result = this.constructionTypeRepository.GetAll();
            return Ok(result.Select(x => x.Title));
        }

        [HttpGet("getAllConstructionStages")]
        public async Task<IActionResult> GetAllConstructionStages()
        {
            var result = this.constructionStageRepository.GetAll();
            return Ok(result.Select(x => x.Title));
        }

        [HttpGet("getAllLandholdingTypes")]
        public async Task<IActionResult> GetAllLandholdingTypes()
        {
            var result = this.landholdingTypeRepository.GetAll();
            return Ok(result.Select(x => x.Title));
        }

        [HttpGet("getAllFeatures")]
        public async Task<IActionResult> GetAllFeatures()
        {
            var result = this.featureRepository.GetAll();
            return Ok(result);
        }

        [HttpPost("createNewLandholding")]
        public async Task<IActionResult> CreateNewLandholing([FromBody] CreateLandholdingRequestDto createlandholding)
        {
            this.landholdingService.CreateLandholding(createlandholding.CreateLandholdingDto, createlandholding.Email);
            return Ok();
        }

        [HttpGet("getAllUserLandholdingsByEmail/{email}")]
        public async Task<IActionResult> GetAllUserLandholdingsByEmail(string email)
        {
            var res = this.landholdingService.GetLandholdingsForUser(email);
            return Ok(res);
        }

        [HttpGet("getAllUserLandholdingsById/{id}")]
        public async Task<IActionResult> GetAllUserLandholdingsById(int id)
        {
            var res = this.landholdingService.GetLandholdingsForUserById(id);
            return Ok(res);
        }

        [HttpPost("getLandholdingById")]
        public async Task<IActionResult> GetLandholdingById([FromBody] int id)
        {
            var res = this.landholdingService.GetLandholdingById(id);
            return Ok(res);
        }

        [HttpPut("updateLandholding")]
        public async Task<IActionResult> UpdateLandholding([FromBody] CreateLandholdingRequestDto updateLandholding)
        {
            this.landholdingService.UpdateLandholding(updateLandholding.CreateLandholdingDto, updateLandholding.Email);
            return Ok();
        }

        [HttpGet("searchLandholdings/{query}")]
        public async Task<IActionResult> SearchLandholdings(string query)
        {
            var res = this.landholdingService.SearchLandholdings(query);
            return Ok(res);
        }

        [HttpGet("getLatestLandholdings")]
        public async Task<IActionResult> GetLatestLandholdings()
        {
            var res = this.landholdingService.GetLatestLandholdings();
            return Ok(res);
        }

        [HttpPost("deleteLandholding")]
        public async Task<IActionResult> DeleteLandholding([FromBody] int landholdingId)
        {
            var res = this.landholdingService.DeleteLandholding(landholdingId);

            if(res)
                return Ok(res);
            else
                return BadRequest(res);
        }

        [HttpGet("getLatestDiscounterdLadholdings")]
        public async Task<IActionResult> GetLatestDiscounterdLadholdings()
        {
            var res = this.landholdingService.GetLatestDiscountedLandholdings();
            return Ok(res);
        }

        [HttpGet("generatePdf/{id}")]
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var pdf = PdfGenerator.GeneratePdf(this.landholdingService.GetLandholdingById(id));

            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = "landholding.pdf",
                Inline = false // Prompt the browser to download the file
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            return File(pdf.BinaryData, "application/pdf");
        }

    }
}
