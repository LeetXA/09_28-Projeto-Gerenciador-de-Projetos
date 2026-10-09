
using _09_28___Projeto_Gerenciador_de_Projetos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _09_28___Projeto_Gerenciador_de_Projetos
{
    internal class Program
    {
        private static Projetos projetos = new Projetos();

        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine("===== GERENCIADOR DE PROJETOS =====");
                Console.WriteLine("0 - Sair");
                Console.WriteLine("1 - Adicionar projeto");
                Console.WriteLine("2 - Pesquisar projeto");
                Console.WriteLine("3 - Remover projeto");
                Console.WriteLine("4 - Adicionar tarefa em projeto");
                Console.WriteLine("5 - Concluir tarefa");
                Console.WriteLine("6 - Cancelar tarefa");
                Console.WriteLine("7 - Reabrir tarefa");
                Console.WriteLine("8 - Listar tarefas de um projeto");
                Console.WriteLine("9 - Filtrar tarefas em um projeto");
                Console.WriteLine("10 - Filtrar tarefas em todos os projetos");
                Console.WriteLine("11 - Resumo geral");
                Console.WriteLine("12 - Alterar tarefa");
                Console.Write("\nEscolha: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                    opcao = -1;

                Console.Clear();

                try
                {
                    switch (opcao)
                    {
                        case 0:
                            Console.WriteLine("Programa encerrado.");
                            break;
                        case 1:
                            AdicionarProjeto();
                            break;
                        case 2:
                            PesquisarProjeto();
                            break;
                        case 3:
                            RemoverProjeto();
                            break;
                        case 4:
                            AdicionarTarefa();
                            break;
                        case 5:
                            AlterarStatusTarefa("concluir");
                            break;
                        case 6:
                            AlterarStatusTarefa("cancelar");
                            break;
                        case 7:
                            AlterarStatusTarefa("reabrir");
                            break;
                        case 8:
                            ListarTarefasProjeto();
                            break;
                        case 9:
                            FiltrarTarefasProjeto();
                            break;
                        case 10:
                            FiltrarTarefasTodos();
                            break;
                        case 11:
                            ResumoGeral();
                            break;
                        case 12:
                            AlterarTarefa();
                            break;
                        default:
                            Console.WriteLine("Opção inválida.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }

                if (opcao != 0)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }

            } while (opcao != 0);
        }

        private static int LerInteiro(string mensagem)
        {
            int valor;

            while (true)
            {
                Console.Write(mensagem);

                if (int.TryParse(Console.ReadLine(), out valor))
                    return valor;

                Console.WriteLine("Digite um número inteiro válido.");
            }
        }

        private static string LerTexto(string mensagem)
        {
            Console.Write(mensagem);
            return Console.ReadLine() ?? "";
        }

        private static Projeto SelecionarProjeto()
        {
            if (projetos.listar().Count == 0)
            {
                Console.WriteLine("Não há projetos cadastrados.");
                return null;
            }

            Console.WriteLine("Projetos cadastrados:");

            foreach (Projeto p in projetos.listar())
                Console.WriteLine(p);

            int id = LerInteiro("\nInforme o ID do projeto: ");
            Projeto projeto = projetos.buscarPorId(id);

            if (projeto == null)
                Console.WriteLine("Projeto não encontrado.");

            return projeto;
        }

        private static Tarefa SelecionarTarefa(Projeto projeto)
        {
            if (projeto.Tarefas.Count == 0)
            {
                Console.WriteLine("Esse projeto não possui tarefas.");
                return null;
            }

            foreach (Tarefa t in projeto.Tarefas)
                Console.WriteLine(t + "\n");

            int id = LerInteiro("Informe o ID da tarefa: ");
            Tarefa tarefa = projeto.buscarTarefaPorId(id);

            if (tarefa == null)
                Console.WriteLine("Tarefa não encontrada.");

            return tarefa;
        }

        private static void AdicionarProjeto()
        {
            Console.WriteLine("=== ADICIONAR PROJETO ===");

            int id = LerInteiro("ID do projeto: ");
            string nome = LerTexto("Nome do projeto: ");

            Projeto projeto = new Projeto(id, nome);

            if (projetos.adicionar(projeto))
                Console.WriteLine("Projeto cadastrado com sucesso!");
            else
                Console.WriteLine("Já existe um projeto com esse ID.");
        }

        private static void PesquisarProjeto()
        {
            Console.WriteLine("=== PESQUISAR PROJETO ===");

            Projeto projeto = SelecionarProjeto();

            if (projeto == null)
                return;

            Console.WriteLine("\n" + projeto);
            Console.WriteLine("\n--- TAREFAS ABERTAS ---");
            ExibirTarefas(projeto.tarefasPorStatus("Aberta"));

            Console.WriteLine("\n--- TAREFAS FECHADAS ---");
            ExibirTarefas(projeto.tarefasPorStatus("Fechada"));

            Console.WriteLine("\n--- TAREFAS CANCELADAS ---");
            ExibirTarefas(projeto.tarefasPorStatus("Cancelada"));

            Console.WriteLine($"\nTotal abertas: {projeto.totalAbertas()}");
            Console.WriteLine($"Total fechadas: {projeto.totalFechadas()}");
            Console.WriteLine($"Total canceladas: {projeto.totalCanceladas()}");
        }

        private static void RemoverProjeto()
        {
            Console.WriteLine("=== REMOVER PROJETO ===");

            Projeto projeto = SelecionarProjeto();

            if (projeto == null)
                return;

            if (projetos.remover(projeto))
                Console.WriteLine("Projeto removido com sucesso!");
            else
                Console.WriteLine(
                    "Não foi possível remover. O projeto pode ter tarefas cadastradas.");
        }

        private static void AdicionarTarefa()
        {
            Console.WriteLine("=== ADICIONAR TAREFA ===");

            Projeto projeto = SelecionarProjeto();

            if (projeto == null)
                return;

            int id = LerInteiro("ID da tarefa: ");
            string titulo = LerTexto("Título: ");
            string descricao = LerTexto("Descrição: ");

            Console.WriteLine("Prioridade: 1-Alta | 2-Média | 3-Baixa");
            int prioridade = LerInteiro("Prioridade: ");

            Tarefa tarefa = new Tarefa(id, titulo, descricao, prioridade);
            projeto.adicionarTarefa(tarefa);

            Console.WriteLine("Tarefa adicionada com sucesso!");
        }

        private static void AlterarStatusTarefa(string acao)
        {
            Console.WriteLine("=== " + acao.ToUpper() + " TAREFA ===");

            Projeto projeto = SelecionarProjeto();

            if (projeto == null)
                return;

            Tarefa tarefa = SelecionarTarefa(projeto);

            if (tarefa == null)
                return;

            switch (acao)
            {
                case "concluir":
                    if (tarefa.Status != "Aberta")
                    {
                        Console.WriteLine("Somente tarefas abertas podem ser concluídas.");
                        return;
                    }

                    tarefa.concluir();
                    Console.WriteLine("Tarefa concluída.");
                    break;

                case "cancelar":
                    if (tarefa.Status != "Aberta")
                    {
                        Console.WriteLine("Somente tarefas abertas podem ser canceladas.");
                        return;
                    }

                    tarefa.cancelar();
                    Console.WriteLine("Tarefa cancelada.");
                    break;

                case "reabrir":
                    if (tarefa.Status == "Aberta")
                    {
                        Console.WriteLine("A tarefa já está aberta.");
                        return;
                    }

                    tarefa.reabrir();
                    Console.WriteLine("Tarefa reaberta.");
                    break;
            }
        }


        private static void AlterarTarefa()
        {
            Console.WriteLine("=== ALTERAR TAREFA ===");

            Projeto projeto = SelecionarProjeto();

            if (projeto == null)
                return;

            Tarefa tarefa = SelecionarTarefa(projeto);

            if (tarefa == null)
                return;

            Console.WriteLine("\nDeixe os dados atuais como referência para a alteração.");
            Console.WriteLine($"Título atual: {tarefa.Titulo}");
            string titulo = LerTexto("Novo título: ");

            Console.WriteLine($"Descrição atual: {tarefa.Descricao}");
            string descricao = LerTexto("Nova descrição: ");

            Console.WriteLine($"Prioridade atual: {tarefa.Prioridade}");
            Console.WriteLine("1 - Alta | 2 - Média | 3 - Baixa");
            int prioridade = LerInteiro("Nova prioridade: ");

            tarefa.Alterar(titulo, descricao, prioridade);

            Console.WriteLine("Tarefa alterada com sucesso!");
        }


        private static void ListarTarefasProjeto()
        {
            Console.WriteLine("=== LISTAR TAREFAS ===");

            Projeto projeto = SelecionarProjeto();

            if (projeto == null)
                return;

            ExibirTarefas(projeto.Tarefas);
        }

        private static void FiltrarTarefasProjeto()
        {
            Console.WriteLine("=== FILTRAR TAREFAS DO PROJETO ===");

            Projeto projeto = SelecionarProjeto();

            if (projeto == null)
                return;

            FiltrarEExibir(projeto.Tarefas);
        }

        private static void FiltrarTarefasTodos()
        {
            Console.WriteLine("=== FILTRAR TAREFAS DE TODOS OS PROJETOS ===");

            List<Tarefa> todas = projetos.listar()
                .SelectMany(p => p.Tarefas)
                .ToList();

            if (todas.Count == 0)
            {
                Console.WriteLine("Não há tarefas cadastradas.");
                return;
            }

            FiltrarEExibir(todas);
        }

        private static void FiltrarEExibir(
            IEnumerable<Tarefa> tarefas)
        {
            Console.WriteLine("1 - Filtrar por status");
            Console.WriteLine("2 - Filtrar por prioridade");

            int tipo = LerInteiro("Escolha: ");

            if (tipo == 1)
            {
                Console.WriteLine("Status: Aberta, Fechada ou Cancelada");
                string status = LerTexto("Informe o status: ");

                var resultado = tarefas
                    .Where(t => t.Status.Equals(
                        status, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                ExibirTarefas(resultado);
            }
            else if (tipo == 2)
            {
                Console.WriteLine("Prioridade: 1-Alta | 2-Média | 3-Baixa");
                int prioridade = LerInteiro("Informe a prioridade: ");

                if (prioridade < 1 || prioridade > 3)
                {
                    Console.WriteLine("Prioridade inválida.");
                    return;
                }

                var resultado = tarefas
                    .Where(t => t.Prioridade == prioridade)
                    .ToList();

                ExibirTarefas(resultado);
            }
            else
            {
                Console.WriteLine("Filtro inválido.");
            }
        }

        private static void ExibirTarefas(
            IEnumerable<Tarefa> tarefas)
        {
            List<Tarefa> lista = tarefas.ToList();

            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa encontrada.");
                return;
            }

            foreach (Tarefa t in lista)
                Console.WriteLine(t + "\n");
        }

        private static void ResumoGeral()
        {
            Console.WriteLine("=== RESUMO GERAL ===");

            List<Projeto> listaProjetos = projetos.listar();
            List<Tarefa> todas = listaProjetos
                .SelectMany(p => p.Tarefas)
                .ToList();

            int abertas = todas.Count(t => t.Status == "Aberta");
            int fechadas = todas.Count(t => t.Status == "Fechada");
            int canceladas = todas.Count(t => t.Status == "Cancelada");

            // Percentual concluído entre tarefas abertas e fechadas.
            int consideradas = abertas + fechadas;
            double percentual = consideradas == 0
                ? 0
                : (double)fechadas / consideradas * 100;

            Console.WriteLine($"Quantidade de projetos: {listaProjetos.Count}");
            Console.WriteLine($"Tarefas abertas: {abertas}");
            Console.WriteLine($"Tarefas fechadas: {fechadas}");
            Console.WriteLine($"Tarefas canceladas: {canceladas}");
            Console.WriteLine($"Total de tarefas: {todas.Count}");
            Console.WriteLine(
                $"Percentual concluído (fechadas / abertas + fechadas): {percentual:F2}%");
        }
    }
}
