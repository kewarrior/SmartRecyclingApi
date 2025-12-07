using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Utilizador;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartRecyclingApi.Services.Utilizador
{
    public class UtilizadorService : IUtilizadorInterface
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UtilizadorService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseModel<List<UtilizadorModel>>> GetUtilizadores()
        {
            ResponseModel<List<UtilizadorModel>> resposta = new ResponseModel<List<UtilizadorModel>>();
            try
            {
                var utilizadores = await _context.Utilizadores
                    .Select(u => new UtilizadorModel
                    {
                        Id = u.Id,
                        nome = u.nome,
                        email = u.email,
                        morada = u.morada,
                        codigo_postal = u.codigo_postal,
                        telefone = u.telefone,
                        pontos = u.pontos,
                        status = u.status,
                        adesao = u.adesao,
                        data_nascimento = u.data_nascimento,
                        Reciclagem = u.Reciclagem,
                        Pedido = u.Pedido
                    })
                    .ToListAsync();

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
                    resposta.Status = false;
                    return resposta;
                }

                var emailRegex = @"^[^@\s]+@[^@\s]+\.(pt|com)$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(utilizadorCriacaoDto.email ?? string.Empty, emailRegex))
                {
                    resposta.Mensagem = "O e-mail informado não é válido. Deve terminar com .pt ou .com";
                    resposta.Status = false;
                    return resposta;
                }

                var normalizaEmail = utilizadorCriacaoDto.email?.ToLower();
                var existeEmail = await _context.Utilizadores.FirstOrDefaultAsync(emailutilizador => emailutilizador.email == normalizaEmail);
                if (existeEmail != null)
                {
                    resposta.Mensagem = "O endereço de e-mail já está associado a outro utilizador ";
                    resposta.Status = false;
                    return resposta;
                }
                var criptarPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(utilizadorCriacaoDto.password, 10);

                var utilizador = new UtilizadorModel()
                {
                    nome = utilizadorCriacaoDto.nome,
                    email = normalizaEmail,
                    password = criptarPassword,
                    data_nascimento = utilizadorCriacaoDto.data_nascimento,
                    telefone = utilizadorCriacaoDto.telefone,
                    morada = utilizadorCriacaoDto.morada,
                    codigo_postal = utilizadorCriacaoDto.codigopostal,
                    Role = "Utilizador"
                };

                _context.Add(utilizador);
                await _context.SaveChangesAsync();

                resposta.Mensagem = "Utilizador Criado com Sucesso";
                resposta.Status = true;
                return resposta;

            }
            catch (Exception ex)
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

                if (utilizador == null)
                {
                    resposta.Mensagem = "Utilizador não encontrado";
                    resposta.Status = false;
                    return resposta;
                }
                var emailRegex = @"^[^@\s]+@[^@\s]+\.(pt|com)$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(editarUtilizadorDto.email ?? string.Empty, emailRegex))
                {
                    resposta.Mensagem = "O e-mail informado não é válido. Deve terminar com .pt ou .com";
                    resposta.Status = false;
                    return resposta;
                }

                var normalizaEmail = editarUtilizadorDto.email?.ToLower();
                var existeEmail = await _context.Utilizadores.FirstOrDefaultAsync(emailutilizador => emailutilizador.email == normalizaEmail);
                if (existeEmail != null)
                {
                    resposta.Mensagem = "O endereço de e-mail já está associado a outro utilizador ";
                    resposta.Status = false;
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
        public async Task<ResponseModel<LoginResponseModel>> Login(LoginRequest request)
        {
            ResponseModel<LoginResponseModel> resposta = new ResponseModel<LoginResponseModel>();
            try
            {

                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    resposta.Mensagem = "É necessário preencher os dados de Email e Palavra-Passe";
                    resposta.Status = false;
                    return resposta;
                }

                var emailNormalizado = request.Email.ToLower();
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(utilizadorlogin => utilizadorlogin.email == emailNormalizado);

                if (utilizador == null)
                {
                    resposta.Mensagem = "Dados Incorretos";
                    resposta.Status = false;
                    return resposta;
                }

                var passwordValida = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, utilizador.password);

                if (!passwordValida)
                {
                    resposta.Mensagem = "Dados Incorretos";
                    resposta.Status = false;
                    return resposta;
                }

                var issuer = _configuration["JwtConfig:Issuer"];
                var audience = _configuration["JwtConfig:Audience"];
                var key = _configuration["JwtConfig:Key"];
                var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
                var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Email, request.Email),
                        new Claim(ClaimTypes.Name, utilizador.nome),
                        new Claim(ClaimTypes.Role, utilizador.Role)

                    }),
                    Expires = tokenExpiryTimeStamp,
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),SecurityAlgorithms.HmacSha256Signature),
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var acessToken = tokenHandler.WriteToken(securityToken);

                resposta.Dados = new LoginResponseModel
                {
                    AcessoToken = acessToken,
                    Expira = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
                };
                resposta.Status = true;
                resposta.Mensagem = "Login com Sucesso";
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
