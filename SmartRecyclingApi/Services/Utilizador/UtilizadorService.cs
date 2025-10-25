using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Services.Utilizador
{
    public class UtilizadorService : IUtilizadorInterface
    {
        private readonly AppDbContext _context;
        public UtilizadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<UtilizadorModel>>> GetUtilizadores()
        {
            ResponseModel<List<UtilizadorModel>> resposta = new ResponseModel<List<UtilizadorModel>>();
            try
            {
                var utilizadores = await _context.Utilizadores.ToListAsync();

                resposta.Dados = utilizadores;
                resposta.Mensagem = "Todos os utilizadores foram carregados!";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }

        }

        public async Task<ResponseModel<UtilizadorModel>> GetUtilizarbyId(int idUtilizador)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();

            try
            {
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(utilizadorBanco => utilizadorBanco.Id == idUtilizador);

                if (utilizador == null)
                {
                    resposta.Mensagem = "Não existe nenhum utilizador com esse ID";
                    return resposta;
                }

                resposta.Dados = utilizador;
                resposta.Mensagem = "Foi encontrado o utilizador";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }

        }

        public async Task<ResponseModel<UtilizadorModel>> CriarUtilizador(UtilizadorCriacaoDTO utilizadorCriacaoDto)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();
            try
            {
                if (new[] { utilizadorCriacaoDto.nome, utilizadorCriacaoDto.email, utilizadorCriacaoDto.password }
                    .Any(string.IsNullOrEmpty))
                {
                    resposta.Mensagem = "Nome, e-mail e palavra-passe são obrigatórios.";
                    return resposta;
                }

                var emailRegex = @"^[^@\s]+@[^@\s]+\.(pt|com)$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(utilizadorCriacaoDto.email ?? string.Empty, emailRegex))
                {
                    resposta.Mensagem = "O e-mail informado não é válido. Deve terminar com .pt ou .com";
                    return resposta;
                }

                var normalizaEmail = utilizadorCriacaoDto.email?.ToLower();
                var existeEmail = await _context.Utilizadores.FirstOrDefaultAsync(emailutilizador => emailutilizador.email == normalizaEmail);
                if (existeEmail != null)
                {
                    resposta.Mensagem = "O endereço de e-mail já está associado a outro utilizador ";
                    return resposta;
                }

                var criptarPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(utilizadorCriacaoDto.password, 13);

                var utilizador = new UtilizadorModel()
                {
                    nome = utilizadorCriacaoDto.nome,
                    email = normalizaEmail,
                    password = criptarPassword
                };

                _context.Add(utilizador);
                await _context.SaveChangesAsync();

                resposta.Dados = utilizador;
                resposta.Mensagem = "Utilizador Criado com Sucesso";
                return resposta;

            }catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }

        public async Task<ResponseModel<UtilizadorModel>> EditarUtilizador(EditarUtilizadorDto editarUtilizadorDto)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();
            try
            {
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(utilizadorBanco => utilizadorBanco.Id == editarUtilizadorDto.Id);

                if(utilizador == null)
                {
                    resposta.Mensagem = "Utilizador não encontrado";
                    return resposta;
                }
                var emailRegex = @"^[^@\s]+@[^@\s]+\.(pt|com)$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(editarUtilizadorDto.email ?? string.Empty, emailRegex))
                {
                    resposta.Mensagem = "O e-mail informado não é válido. Deve terminar com .pt ou .com";
                    return resposta;
                }

                var normalizaEmail = editarUtilizadorDto.email?.ToLower();
                var existeEmail = await _context.Utilizadores.FirstOrDefaultAsync(emailutilizador => emailutilizador.email == normalizaEmail);
                if (existeEmail != null)
                {
                    resposta.Mensagem = "O endereço de e-mail já está associado a outro utilizador ";
                    return resposta;
                }

                var criptarPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(editarUtilizadorDto.password, 13);

                utilizador.nome = editarUtilizadorDto.nome;
                utilizador.email = normalizaEmail;
                utilizador.password = criptarPassword;
                utilizador.morada = editarUtilizadorDto.morada;
                utilizador.codigo_postal = editarUtilizadorDto.codigo_postal;
                utilizador.telefone = editarUtilizadorDto.telefone;

                _context.Update(utilizador);
                await _context.SaveChangesAsync();

                resposta.Mensagem = "Utilizador editado com Sucesso";
                return resposta;
            }
            catch (Exception ex) 
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
        }
    }
}
