using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_28___Projeto_Gerenciador_de_Projetos
{
    internal class Projeto
    {
        private int id;
        private string nome;
        private List<Tarefa> tarefas;

        public int Id
        {
            get { return id; }
        }

        public string Nome
        {
            get { return nome; }
        }

        public IReadOnlyList<Tarefa> Tarefas
        {
            get { return tarefas.AsReadOnly(); }
        }

        public Projeto(int id, string nome)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "O ID do projeto deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException(
                    "O nome do projeto é obrigatório.");

            this.id = id;
            this.nome = nome;
            tarefas = new List<Tarefa>();
        }

        public void adicionarTarefa(Tarefa t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));

            if (buscarTarefa(t) != null)
                throw new ArgumentException(
                    "Já existe uma tarefa com esse ID no projeto.");

            tarefas.Add(t);
        }

        public bool removerTarefa(Tarefa t)
        {
            if (t == null)
                return false;

            Tarefa encontrada = buscarTarefa(t);

            if (encontrada == null)
                return false;

            return tarefas.Remove(encontrada);
        }

        public Tarefa buscarTarefa(Tarefa t)
        {
            if (t == null)
                return null;

            return tarefas.Find(x => x.Id == t.Id);
        }

        public Tarefa buscarTarefaPorId(int idTarefa)
        {
            return tarefas.Find(t => t.Id == idTarefa);
        }

        public List<Tarefa> tarefasPorStatus(string s)
        {
            return tarefas
                .Where(t => t.Status.Equals(
                    s, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Tarefa> tarefasPorPrioridade(int p)
        {
            return tarefas
                .Where(t => t.Prioridade == p)
                .ToList();
        }

        public int totalAbertas()
        {
            return tarefas.Count(t => t.Status == "Aberta");
        }

        public int totalFechadas()
        {
            return tarefas.Count(t => t.Status == "Fechada");
        }

        public int totalCanceladas()
        {
            return tarefas.Count(t => t.Status == "Cancelada");
        }

        public override string ToString()
        {
            return $"ID: {id} | Projeto: {nome} | " +
                   $"Tarefas: {tarefas.Count}";
        }
    }
}
