using Insurance_Service_Database.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance_Service_Database
{
    class InsuranceServiceProgram
    {
        private static InsuranceServiceDatabaseEntities database;

        static void Main(string[] args)
        {
            //InsuranceCompany insuranceCompany = new InsuranceCompany();
            //insuranceCompany.Abbreviation = "VZP";
            //insuranceCompany.Name = "Všeobecná zdravotní pojišťovna";

            //Database.InsuranceCompanies.Add(insuranceCompany);
            //Database.SaveChanges();

            PrintInsuranceCompanies();

            Console.ReadKey();
        }

        public static void PrintInsuranceCompanies()
        {
            foreach (var o in Database.InsuranceCompanies)
            {
                Console.WriteLine("Name: {0}\nAbbreviation: {1}\nID: {2}\n", o.Name, o.Abbreviation, o.Id);
            }
        }

        public static InsuranceServiceDatabaseEntities Database
        {
            get
            {
                if (database == null)
                    database = new InsuranceServiceDatabaseEntities();
                return database;
            }
        }
    }
}
