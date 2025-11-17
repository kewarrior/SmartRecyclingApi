using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Services.Administrador
{
    public class AdministradorService : IAdminInterface
    {
        private readonly AppDbContext _context;

        public AdministradorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<object>> UpdatePedido(long id)
        {
            ResponseModel<object> resposta = new ResponseModel<object>();

            try
            {

                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == id && u.adesao == false);

                if (utilizador == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Utilizador não encontrado ou adesão já aceite";
                    return resposta;
                }

                var updateStatus = await _context.Utilizadores.Where(u => u.Id == id).ExecuteUpdateAsync(set => set.SetProperty(p => p.adesao, true));

                var apagarPedido = await _context.Pedido.Where(u => u.ref_Utilizador == id).ExecuteDeleteAsync();

                resposta.Dados = utilizador.nome;
                resposta.Status = true;
                resposta.Mensagem = $"Adesão do utilizador {utilizador.nome} aceite com sucesso e pedido removido";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao aceitar pedido: {ex.Message}";

            }
            return resposta;
        }


        public async Task<ResponseModel<object>> ValidarPassword(UtilizadorCriacaoDTO verificarUtilizador)
        {
            ResponseModel<object> resposta = new ResponseModel<object>();

            try
            {

                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == verificarUtilizador.Id);

                if (utilizador == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Utilizador não encontrado";
                    return resposta;
                }

                var validarPassword = BCrypt.Net.BCrypt.EnhancedVerify(verificarUtilizador.password, utilizador.password);

                if (!validarPassword)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Password de administrador incorreta";
                    return resposta;
                }

                if (utilizador.Role != "administrador")
                {
                    resposta.Status = false;
                    resposta.Mensagem = " Utilizador não tem permissoes de administrador";
                    return resposta;
                }

                resposta.Status = true;
                resposta.Mensagem = "Pode editar";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao aceitar pedido: {ex.Message}";
            }
            return resposta;
        }


        public async Task<ResponseModel<List<UtilizadorModel>>> Listar()
        {
            ResponseModel<List<UtilizadorModel>> resposta = new ResponseModel<List<UtilizadorModel>>();

            try
            {

                var utilizadores = await _context.Utilizadores.Select(
                    u => new UtilizadorModel
                    {
                        Id = u.Id,
                        nome = u.nome,
                        email = u.email,
                        Role = u.Role
                    }).ToListAsync();

                if (utilizadores.Count <= 0)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Nenhum utilizador encontrado";
                    return resposta;
                }

                resposta.Status = true;
                resposta.Mensagem = $"Foram encontrados {utilizadores.Count} utilizadores";
                resposta.Dados = utilizadores;


            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao procurar utilizadores {ex.Message}";
                return resposta;
            }
            return resposta;
        }

        public async Task<ResponseModel<UtilizadorModel>> EditarUtilizador(UtilizadorCriacaoDTO utilizador)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();

            try
            {
                if (utilizador.Id == 0 || utilizador.nome == null || utilizador.email == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Dados inválidos: Id, nome, email sao obrigatórios";
                    return resposta;
                }


                var dadosutilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == utilizador.Id);

                var emailExistente = await _context.Utilizadores.Where(u => u.email == utilizador.email).FirstOrDefaultAsync();

                if (emailExistente != null && emailExistente.Id != utilizador.Id)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Email já associado a outro utilizador";
                    return resposta;
                }



                dadosutilizador.nome = utilizador.nome;
                dadosutilizador.email = utilizador.email;
                dadosutilizador.Role = utilizador.role;

                _context.Update(dadosutilizador);
                await _context.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Utilizador atualizado com sucesso";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao editar Utilizador {ex.Message}";
                return resposta;
            }
        }

        public async Task<ResponseModel<UtilizadorModel>> ApagarUtilizador(long id)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();

            try
            {

                var autor = await _context.Utilizadores.FirstOrDefaultAsync(utbanco => utbanco.Id == id);

                if (autor == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Não foi encontrado nenhum utilizador.";
                    return resposta;
                }

                _context.Remove(autor);
                await _context.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Utilizador apagado";
                return resposta;


            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao apagar Utilizador {ex.Message}";
                return resposta;
            }
        }

        public async Task<ResponseModel<UtilizadorModel>> CriarADmin(UtilizadorCriacaoDTO adminUtilizador)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();

            try
            {

                if(adminUtilizador.email == null || adminUtilizador.nome == null || adminUtilizador.password == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Dados obrigatorios nao preenchidos";
                    return resposta;
                }

                var emailRegex = @"^[^@\s]+@[^@\s]+\.(pt|com)$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(adminUtilizador.email ?? string.Empty, emailRegex))
                {
                    resposta.Mensagem = "O e-mail informado não é válido. Deve terminar com .pt ou .com";
                    return resposta;
                }

                var normalizarEmail = adminUtilizador.email.ToLower();

                var existeEmail = await _context.Utilizadores.Where(emailbanco => emailbanco.email == normalizarEmail).FirstOrDefaultAsync();

                if(existeEmail != null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Email ja existente";
                    return resposta;
                }

                var criptarPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(adminUtilizador.password, 13);

                var admin = new UtilizadorModel()
                {
                    email = normalizarEmail,
                    password = criptarPassword,
                    Role = "Administrador"

                };

                _context.Add(admin);
                await _context.SaveChangesAsync();


                resposta.Dados = admin;
                resposta.Status = true;
                resposta.Mensagem = "Administrador Criado";
                return resposta;


            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao criar Administrador: {ex.Message}";
                return resposta;

            }
        }
    }
}