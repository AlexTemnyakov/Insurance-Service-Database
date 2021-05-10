using Insurance_Service_Database.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Insurance_Service_Database
{
    class InsuranceServiceProgram
    {
        private InsuranceServiceDatabaseEntities database;

        static void Main(string[] args)
        {          
            InsuranceServiceProgram insuranceServiceProgram = new InsuranceServiceProgram();
            insuranceServiceProgram.PrintDatabaseContent();

            //insuranceServiceProgram.RemoveAllInsuranceCompanies();
            //insuranceServiceProgram.RemoveAllMedicalServiceProviderTypes();

            //insuranceServiceProgram.AddInsuranceCompanies();
            //insuranceServiceProgram.AddMedicalServiceProviderTypes();
            //insuranceServiceProgram.AddInsuranceContractTypes();

            Console.ReadKey();
        }

        private void RemoveAllInsuranceCompanies()
        {
            foreach (var entity in Database.InsuranceCompanies)
                Database.InsuranceCompanies.Remove(entity);
            Database.SaveChanges();
        }

        private void RemoveAllMedicalServiceProviderTypes()
        {
            foreach (var entity in Database.MedicalServiceProviderTypes)
                Database.MedicalServiceProviderTypes.Remove(entity);
            Database.SaveChanges();
        }

        public void PrintDatabaseContent()
        {
            PrintInsuranceCompanies();
            Console.WriteLine("\n\n");
            PrintMedicalServiceProviderTypes();
            Console.WriteLine("\n\n");
            PrintInsuranceContractTypes();
            Console.WriteLine("\n\n");
        }        

        public IEnumerable<InsuranceCompany> FindInsuranceCompaniesById(int id)
        {
            return from o in Database.InsuranceCompanies
                        where o.Id == id
                        select o;
        }

        public IEnumerable<MedicalServiceProviderType> FindMedicalServiceProviderTypeById(int id)
        {
            return from o in Database.MedicalServiceProviderTypes
                        where o.Id == id
                        select o;
        }

        public bool AddInsuranceCompany(InsuranceCompany insuranceCompany)
        {
            Database.InsuranceCompanies.Add(insuranceCompany);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddMedicalServiceProviderType(string code, string name, DateTime validFrom, DateTime? validUntil)
        {
            MedicalServiceProviderType medicalServiceProviderType = new MedicalServiceProviderType();
            medicalServiceProviderType.Code = code;
            medicalServiceProviderType.Name = name;
            medicalServiceProviderType.ValidFrom = validFrom;
            medicalServiceProviderType.ValidUntil = validUntil;
            Database.MedicalServiceProviderTypes.Add(medicalServiceProviderType);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddMedicalServiceProviderType(MedicalServiceProviderType medicalServiceProviderType)
        {
            Database.MedicalServiceProviderTypes.Add(medicalServiceProviderType);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddInsuranceContractType(InsuranceContractType insuranceContractType)
        {
            Database.InsuranceContractTypes.Add(insuranceContractType);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        private bool DoCommonStepsAfterAdditionToDatabase()
        {
            try
            {
                Database.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                database = null;
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                return false;
            }
        }

        public void PrintInsuranceCompanies()
        {
            Console.WriteLine("========= INSURANCE COMPANIES =========");
            Console.WriteLine();
            foreach (var o in Database.InsuranceCompanies)
            {
                Console.WriteLine(EntityToStringConverter.InsuranceCompanyToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("=======================================");
        }

        public void PrintMedicalServiceProviderTypes()
        {
            Console.WriteLine("========= MEDICAL SERVICE PROVIDERS =========");
            Console.WriteLine();
            foreach (var o in Database.MedicalServiceProviderTypes)
            {
                Console.WriteLine(EntityToStringConverter.MedicalServiceProviderTypeToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("=============================================");
        }

        public void PrintInsuranceContractTypes()
        {
            Console.WriteLine("========= INSURANCE CONTRACT TYPES =========");
            Console.WriteLine();
            foreach (var o in Database.InsuranceContractTypes)
            {
                Console.WriteLine(EntityToStringConverter.InsuranceContractTypeToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("============================================");
        }

        private void AddInsuranceCompanies()
        {
            try
            {
                AddInsuranceCompany(
                    new InsuranceCompany()
                    {
                        Abbreviation = "VZP",
                        Name = "Všeobecná zdravotní pojišťovna"
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void AddMedicalServiceProviderTypes()
        {
            try
            {
                AddMedicalServiceProviderType(
                    "P1",
                    "Zubař",
                    new DateTime(2010, 1, 1),
                    new DateTime(2015, 1, 1));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {

                AddMedicalServiceProviderType(
                    "P2",
                    "Pediatr",
                    new DateTime(2010, 1, 1),
                    null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {

                AddMedicalServiceProviderType(
                    "P3",
                    "Chirurg",
                    new DateTime(2010, 1, 1),
                    null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {

                AddMedicalServiceProviderType(
                    "P1",
                    "Dentista",
                    new DateTime(2015, 2, 1),
                    null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void AddInsuranceContractTypes()
        {
            //Id Kod Nazev DatumOd DatumDo
            //1   A001 Smlouva o převozu   1 / 1 / 2010    1 / 1 / 2012
            //2   A002 Smlouva o péči  1 / 1 / 2010    1 / 1 / 2012
            //3   A001 Smlouva o převozu 2012  1 / 2 / 2012    1 / 1 / 2014
            //4   A002 Smlouva o péči 2012 1 / 2 / 2012    1 / 1 / 2014
            //5   A001 Smlouva o převozu aktuální  1 / 2 / 2014
            //6   A002 Smlouva o péči aktuální 1 / 2 / 2014

            try
            {
                AddInsuranceContractType(new InsuranceContractType()
                {
                    Code = "A001",
                    Name = "Smlouva o převozu",
                    ValidFrom = new DateTime(2010, 1, 1),
                    ValidUntil = new DateTime(2012, 1, 1)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContractType(new InsuranceContractType()
                {
                    Code = "A002",
                    Name = "Smlouva o péči",
                    ValidFrom = new DateTime(2010, 1, 1),
                    ValidUntil = new DateTime(2012, 1, 1)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContractType(new InsuranceContractType()
                {
                    Code = "A001",
                    Name = "Smlouva o převozu 2012",
                    ValidFrom = new DateTime(2012, 2, 1),
                    ValidUntil = new DateTime(2014, 1, 1)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContractType(new InsuranceContractType()
                {
                    Code = "A002",
                    Name = "Smlouva o péči 2012",
                    ValidFrom = new DateTime(2012, 2, 1),
                    ValidUntil = new DateTime(2014, 1, 1)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContractType(new InsuranceContractType()
                {
                    Code = "A001",
                    Name = "Smlouva o převozu aktuální",
                    ValidFrom = new DateTime(2014, 2, 1),
                    ValidUntil = null
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContractType(new InsuranceContractType()
                {
                    Code = "A002",
                    Name = "Smlouva o péči aktuální",
                    ValidFrom = new DateTime(2014, 2, 1),
                    ValidUntil = null
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public InsuranceServiceDatabaseEntities Database
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
