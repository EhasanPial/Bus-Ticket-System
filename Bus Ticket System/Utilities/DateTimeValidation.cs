using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Bus_Ticket_System.Utilities
{
    public class DateTimeValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null) return false;


            DateTime date;

            try
            {
                date = Convert.ToDateTime(value.ToString());
            }
            catch
            {
                return false;
            }

            DateTime dateNow = DateTime.Now;
            if (dateNow < date) return false;


            return true;
        }
    }
}
