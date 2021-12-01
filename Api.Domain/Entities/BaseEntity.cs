using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        private DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
