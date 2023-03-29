using System.ComponentModel.DataAnnotations;

namespace Pokemon__App.Models
{
    public class Pokemon
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type1 { get; set; }
        public string? Type2 { get; set; }
        public int Total { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set;}
        public int Speed { get; set; }
        public int Generation { get; set; }
        public bool Legendary { get; set; }
    }
}
