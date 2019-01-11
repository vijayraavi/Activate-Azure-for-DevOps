using SimpleCRM.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCRM.UI.Models
{
    public class SalesOrderViewModel : IValidatableObject
    {
        public SalesOrderHeader Order { get; set; }
        public List<SalesOrderDetail> LineItems { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Order.TotalDue > 2000)
            {
                yield return new ValidationResult("We can't process this order online because the total amount is greater than $2000, please call us.");
            }
        }
    }
}
