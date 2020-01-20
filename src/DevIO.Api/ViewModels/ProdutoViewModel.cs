using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.ViewModels
{
    public class ProdutoViewModel
    {
         [Key]
        public Guid Id { get; set; }
        
         public Guid FornecedorId { get; set; }

        public string Nome { get; set; }
        
        public string Descricao { get; set; }        
        public string ImagemUpload { get; set; }
        public string Imagem { get; set; }
        public int Valor {  get; set; }
        
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        
        [ScaffoldColumn(false)]
        public string NomeFornecedor { get; set; }
       // public IEnumerable<ProdutoViewModel> Produtos  { get; set; }
    }
}