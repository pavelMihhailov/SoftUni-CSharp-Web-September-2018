using System.ComponentModel.DataAnnotations.Schema;

namespace IRunes.Models
{
    public abstract class BaseModel<T>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }
}