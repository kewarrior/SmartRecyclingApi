using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Enums;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels;
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
                var pedido = await _context.Pedido.FindAsync(id);
                if (pedido == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Pedido nao existe";
                    return resposta;
                }
                else if (pedido != null && pedido.Status_Pedido == EnumStatusPedido.Aceite)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Pedido já foi aceite";
                    return resposta;
                }

                var query = await (from p in _context.Pedido
                                   join u in _context.Utilizadores on p.ref_Utilizador equals u.Id
                                   where u.adesao == false
                                   && p.Status_Pedido == EnumStatusPedido.Pendente
                                   && p.Id == id
                                   select new PedidoDTO
                                   {
                                       Id = p.Id,
                                       ref_Utilizador = p.ref_Utilizador
                                   }).FirstOrDefaultAsync();

                if (query != null)
                {
                    var updateStatus = await _context.Utilizadores.Where(u => u.Id == query.ref_Utilizador).ExecuteUpdateAsync(set => set.SetProperty(p => p.adesao, true));

                    var atualizarPedido = await _context.Pedido.Where(u => u.Id == id).ExecuteUpdateAsync(set => set.SetProperty(s => s.Status_Pedido, EnumStatusPedido.Aceite));

                    if (updateStatus > 0 && atualizarPedido > 0)
                    {
                        resposta.Status = true;
                        resposta.Mensagem = "Pedido Aceite";
                        return resposta;
                    }
                    else
                    {
                        resposta.Status = false;
                        resposta.Mensagem = "Ocorreu um erro a aceitar pedido";
                        return resposta;
                    }
                }
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao aceitar pedido: {ex.Message}";
            }
            return resposta;
        }
        public async Task<ResponseModel<object>> AceitarVariosPedidos(List<long> ids)
        {
            var resposta = new ResponseModel<object>();
            var resultados = new List<object>();

            try
            {

                var verificarPedidos = await (from p in _context.Pedido
                                              join u in _context.Utilizadores on p.ref_Utilizador equals u.Id
                                              where ids.Contains(p.Id) && p.Status_Pedido == EnumStatusPedido.Pendente
                                              select new
                                              {
                                                  PedidoId = p.Id,
                                                  UtilizadorId = u.Id,
                                                  Status_Pedido = p.Status_Pedido,
                                                  Adesao = u.adesao,
                                                  NomeUtilizador = u.nome
                                              }).ToListAsync();
                var pedidosValidos = new List<long>();
                var utilizadoresValidos = new List<long>();


                foreach (var id in ids)
                {
                    var detalhe = verificarPedidos.FirstOrDefault(p => p.PedidoId == id);

                    if (detalhe == null)
                    {
                        resultados.Add(new { Id = id, Sucesso = false, Mensagem = $"Pedido {detalhe?.PedidoId} não existe ." });
                    }
                    else if (detalhe.Status_Pedido == EnumStatusPedido.Aceite)
                    {
                        resultados.Add(new { Id = id, Sucesso = false, Mensagem = $"Pedido {detalhe.PedidoId} já foi aceite." });
                    }
                    else if (detalhe.Adesao == true)
                    {
                        resultados.Add(new { Id = id, Sucesso = false, Mensagem = $"Utilizador {detalhe.NomeUtilizador} já tem adesão ativa." });
                    }
                    else
                    {
                        pedidosValidos.Add(detalhe.PedidoId);
                        utilizadoresValidos.Add(detalhe.UtilizadorId);
                    }
                }
                    if (pedidosValidos.Any())
                    {
                        var linhasUtilizadores = await _context.Utilizadores.Where(u => utilizadoresValidos.Contains(u.Id)).ExecuteUpdateAsync(set => set.SetProperty(s => s.adesao, true));

                        var linhasPedidos = await _context.Pedido.Where(p => pedidosValidos.Contains(p.Id)).ExecuteUpdateAsync(set => set.SetProperty(s => s.Status_Pedido, EnumStatusPedido.Aceite));

                        if(linhasUtilizadores> 0 && linhasPedidos > 0)
                        {
                            foreach(var idValido in pedidosValidos)
                            {
                                resultados.Add(new { Id = idValido, Sucesso = true, Mensagem = "Pedido aceite com sucesso." });
                            }
                        }
                    }
                
                resposta.Status = true;
                resposta.Dados = resultados;
                resposta.Mensagem = "Processamento Concluido. Verifique a lista de dados para detalhes";
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao aceitar pedido: {ex.Message}";
            }
            return resposta;
        }


        public async Task<ResponseModel<object>> ValidarPassword(LoginRequest verificarUtilizador)
        {
            ResponseModel<object> resposta = new ResponseModel<object>();

            try
            {

                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.email == verificarUtilizador.Email);

                if (utilizador == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Utilizador não encontrado";
                    return resposta;
                }

                var validarPassword = BCrypt.Net.BCrypt.EnhancedVerify(verificarUtilizador.Password, utilizador.password);

                if (!validarPassword)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Password de administrador incorreta";
                    return resposta;
                }

                if (utilizador.Role != "Administrador")
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
                        Role = u.Role,
                        adesao = u.adesao
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

        public async Task<ResponseModel<EditarUtilizadorAdminViewModel>> EditarUtilizador(EditarUtilizadorAdminViewModel utilizador)
        {
            ResponseModel<EditarUtilizadorAdminViewModel> resposta = new ResponseModel<EditarUtilizadorAdminViewModel>();

            try
            {
                if (utilizador.Id == 0 || utilizador.nome == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Dados inválidos: Id, nome, email sao obrigatórios";
                    return resposta;
                }


                var dadosutilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == utilizador.Id);

                var moradaExistente = await _context.Utilizadores.Where(u => u.morada == utilizador.morada).FirstOrDefaultAsync();

                if (moradaExistente != null && moradaExistente.Id != utilizador.Id)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Esta morada já esta associada a outro utilizador";
                    return resposta;
                }



                dadosutilizador.nome = utilizador.nome;
                dadosutilizador.morada = utilizador.morada;
                dadosutilizador.codigo_postal = utilizador.codigo_postal;
                dadosutilizador.telefone = utilizador.telefone;
                dadosutilizador.data_nascimento = utilizador.data_nascimento;

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

                if (adminUtilizador.email == null || adminUtilizador.nome == null || adminUtilizador.password == null)
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

                if (existeEmail != null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Email já existente";
                    return resposta;
                }

                var criptarPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(adminUtilizador.password, 13);

                var admin = new UtilizadorModel()
                {
                    nome = adminUtilizador.nome,
                    email = normalizarEmail,
                    password = criptarPassword,
                    morada = adminUtilizador.morada,
                    codigo_postal = adminUtilizador.codigo_postal,
                    telefone = adminUtilizador.telefone,
                    Role = "Administrador"

                };

                _context.Add(admin);
                await _context.SaveChangesAsync();

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

        public async Task<ResponseModel<UtilizadorModel>> CriarOperario(UtilizadorCriacaoDTO operarioUtilizador)
        {
            var resposta = new ResponseModel<UtilizadorModel>();
            try
            {

                if (operarioUtilizador.email == null || operarioUtilizador.nome == null || operarioUtilizador.password == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Dados obrigatorios nao preenchidos";
                    return resposta;
                }

                var emailRegex = @"^[^@\s]+@[^@\s]+\.(pt|com)$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(operarioUtilizador.email ?? string.Empty, emailRegex))
                {
                    resposta.Mensagem = "O e-mail informado não é válido. Deve terminar com .pt ou .com";
                    resposta.Status = false;
                    return resposta;
                }

                var normalizarEmail = operarioUtilizador.email.ToLower();

                var existeEmail = await _context.Utilizadores.Where(operarioBanco => operarioBanco.email == normalizarEmail).FirstOrDefaultAsync();

                if (existeEmail != null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Email ja existente";
                    return resposta;
                }

                var criptarPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(operarioUtilizador.password, 13);

                var operario = new UtilizadorModel()
                {
                    nome = operarioUtilizador.nome,
                    email = normalizarEmail,
                    password = criptarPassword,
                    morada = operarioUtilizador.morada,
                    codigo_postal = operarioUtilizador.codigo_postal,
                    telefone = operarioUtilizador.telefone,
                    Role = "Operador"
                };

                _context.Add(operario);
                await _context.SaveChangesAsync();

                resposta.Status = true;
                resposta.Mensagem = "Operador Criado";
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