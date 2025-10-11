using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books
{
    public class PresentableEmployee : Employee
    {
        public string Position { get; set; }
        public PresentableEmployee(Employee employee)
        {
            Id = employee.Id;
            FullName = employee.FullName;
            PhoneNumber = employee.PhoneNumber;
            PassportSerialNumber = employee.PassportSerialNumber;
            Password = employee.Password;
            Login = employee.Login;
            Sales = employee.Sales;
            Shipments = employee.Shipments;
            AccessLevel = employee.AccessLevel;
            switch (AccessLevel)
            {
                case 1:
                    Position = "Продавец-кассир";
                    break;
                case 2:
                    Position = "Старший продавец";
                    break;
                default:
                    Position = "Администратор";
                    break;
            }
        }
    }
}
