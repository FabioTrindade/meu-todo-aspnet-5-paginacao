using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paginacao.ViewModels
{
    public class TodoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Total { get; set; }
    }
}
