using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_28___Projeto_Gerenciador_de_Projetos
{
    internal class Projetos
    {
        private List<Projeto> itens;

        public Projetos()
        {
            itens = new List<Projeto>();
        }

        public bool adicionar(Projeto p)
        {
            if (p == null)
                return false;

            if (buscar(p) != null)
                return false;

            itens.Add(p);
            return true;
        }

        public bool remover(Projeto p)
        {
            if (p == null)
                return false;

            Projeto encontrado = buscar(p);

            if (encontrado == null)
                return false;

            if (encontrado.Tarefas.Count > 0)
                return false;

            return itens.Remove(encontrado);
        }

        public Projeto buscar(Projeto p)
        {
            if (p == null)
                return null;

            return itens.Find(x => x.Id == p.Id);
        }

        public Projeto buscarPorId(int id)
        {
            return itens.Find(p => p.Id == id);
        }

        public List<Projeto> listar()
        {
            return new List<Projeto>(itens);
        }
    }
}
