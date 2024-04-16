using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebAPI.Domains;
using WebAPI.Interfaces;
using WebAPI.Utils;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExameController : ControllerBase
    {
        private readonly IExameRepository _exameRepository;
        private readonly OCRService _ocrService;

        public ExameController(IExameRepository exameRepository, OCRService ocrService)
        {
            _exameRepository = exameRepository;
            _ocrService = ocrService;
        }

        [HttpPost("Cadastrar")]
        public async Task<IActionResult> Post([FromForm] ExameViewModel exameViewModel)
        {
            try
            {
                if (exameViewModel == null || exameViewModel.Imagem == null)
                {
                    return BadRequest("Nenhuma imagem fornecida.");
                }

                using (var stream = exameViewModel.Imagem.OpenReadStream())
                {
                    var recognizedText = await _ocrService.RecognizeTextAsync(stream);

                    // Atribuir o texto reconhecido à propriedade Descricao do exameViewModel
                    exameViewModel.Descricao = recognizedText;

                    // Criar um novo objeto Exame e preenchê-lo com os dados do exameViewModel
                    Exame exame = new Exame
                    {
                        Descricao = exameViewModel.Descricao,
                        ConsultaId = exameViewModel.ConsultaId
                    };

                    // Chamar o método Cadastrar do repositório para salvar o exame no banco de dados
                    await _exameRepository.Cadastrar(exame);

                    return Ok(new { RecognizedText = recognizedText });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BuscarPeloIdConsulta")]
        public IActionResult GetByIDConsult(Guid idConsulta)
        {
            try
            {
                List<Exame> lista = _exameRepository.BuscarPorIdConsulta(idConsulta);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
