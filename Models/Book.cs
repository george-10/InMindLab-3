using System.ComponentModel.DataAnnotations;

namespace LinQ_Lab.Models;

public class Book
{ 
        [Required]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public DateTime PublishedYear { get; set; }
}