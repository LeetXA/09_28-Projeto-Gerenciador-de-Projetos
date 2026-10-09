using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_28___Projeto_Gerenciador_de_Projetos
{
    internal class Tarefa
    {
        private int id;
        private string titulo;
        private string descricao;
        private int prioridade;
        private string status;
        private DateTime dataCriacao;
        private DateTime dataConclusao;

        public int Id
        {
            get { return id; }
        }

        public string Titulo
        {
            get { return titulo; }
        }

        public string Descricao
        {
            get { return descricao; }
        }

        public int Prioridade
        {
            get { return prioridade; }
            set
            {
                if (value < 1 || value > 3)
                    throw new ArgumentException(
                        "Prioridade deve ser 1, 2 ou 3.");

                prioridade = value;
            }
        }

        public string Status
        {
            get { return status; }
        }

        public DateTime DataCriacao
        {
            get { return dataCriacao; }
        }

        public DateTime DataConclusao
        {
            get { return dataConclusao; }
        }

        public Tarefa(int id, string titulo, string descricao,
                      int prioridade)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "O ID deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException(
                    "O título da tarefa é obrigatório.");

            if (prioridade < 1 || prioridade > 3)
                throw new ArgumentException(
                    "Prioridade deve ser 1, 2 ou 3.");

            this.id = id;
            this.titulo = titulo;
            this.descricao = descricao;
            this.prioridade = prioridade;
            status = "Aberta";
            dataCriacao = DateTime.Now;
            dataConclusao = DateTime.MinValue;
        }

        public void Alterar(string titulo, string descricao,
                            int prioridade)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException(
                    "O título da tarefa é obrigatório.");

            if (prioridade < 1 || prioridade > 3)
                throw new ArgumentException(
                    "Prioridade deve ser 1, 2 ou 3.");

            this.titulo = titulo;
            this.descricao = descricao;
            this.prioridade = prioridade;
        }

        public void concluir()
        {
            if (status == "Aberta")
            {
                status = "Fechada";
                dataConclusao = DateTime.Now;
            }
        }

        public void cancelar()
        {
            if (status == "Aberta")
            {
                status = "Cancelada";
                dataConclusao = DateTime.MinValue;
            }
        }

        public void reabrir()
        {
            if (status == "Fechada" || status == "Cancelada")
            {
                status = "Aberta";
                dataConclusao = DateTime.MinValue;
            }
        }

        public override string ToString()
        {
            string prioridadeTexto = prioridade switch
            {
                1 => "Alta",
                2 => "Média",
                3 => "Baixa",
                _ => "Indefinida"
            };

            string conclusao = dataConclusao == DateTime.MinValue
                ? "Não concluída"
                : dataConclusao.ToString("dd/MM/yyyy HH:mm");

            return $"ID: {id} | {titulo} | Prioridade: {prioridadeTexto} " +
                   $"| Status: {status} | Criada: {dataCriacao:dd/MM/yyyy} " +
                   $"| Conclusão: {conclusao}\nDescrição: {descricao}";
        }
    }
}
