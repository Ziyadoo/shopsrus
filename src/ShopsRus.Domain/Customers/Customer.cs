using System;
using ShopsRus.Domain.Core;

namespace ShopsRus.Domain.Customers
{
    public class Customer: Entity, IHaveCreationTime
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime CreationTime { get; set; }
        public int CustomerRoleId { get; set; }
        public CustomerRole CustomerRole { get; set; }
    }
}