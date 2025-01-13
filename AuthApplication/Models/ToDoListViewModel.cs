using System.ComponentModel.DataAnnotations;

namespace AuthApplication.Models
{
    public class ToDoListViewModel
    {
        [Required]       
        public int Id { get; set; }

        public string Name { get; set; }        

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateofCreation { get; set; }

        public bool IsCompleted { get; set; }
    }
}
