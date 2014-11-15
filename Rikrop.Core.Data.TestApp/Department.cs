using System;
using System.ComponentModel.DataAnnotations;
using Rikrop.Core.Data.Entities;
using Rikrop.Core.Data.Entities.Contracts;

namespace Rikrop.Core.Data.TestApp
{
    public class Department : Entity<Int32>, IRetrievableEntity<Department, Int32>
    {
        [Required]
        public string Name { get; set; }
    }
}
