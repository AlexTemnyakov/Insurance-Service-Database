using Insurance_Service_Database.EntityDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace Insurance_Service_Database
{   
    class InsuranceServiceProgram
    {
        private static readonly string commandToExit = "e";
        private static readonly string commandToPrintHelp = "h";
        private static readonly string commandToPrint = "p";
        private static readonly string commandToSave = "s";
        private static readonly string commandToFind = "f";
        private static readonly string commandToFindById = "i";
        private static readonly string insuranceCompanyCommandCode = "1";
        private static readonly string commandToPrintAllInsuranceCompanies = commandToPrint + insuranceCompanyCommandCode;
        private static readonly string commandToSaveAllInsuranceCompanies = commandToSave + insuranceCompanyCommandCode;
        private static readonly string commandToFindInsuranceCompanyById = commandToFind + insuranceCompanyCommandCode + commandToFindById;

        private InsuranceServiceDatabaseEntities database;

        static void Main(string[] args)
        {          
            InsuranceServiceProgram insuranceServiceProgram = new InsuranceServiceProgram();

            insuranceServiceProgram.Run();

            //insuranceServiceProgram.RemoveAll();
            //insuranceServiceProgram.PrintDatabaseContent();

            ////insuranceServiceProgram.AddInsuranceCompanies();
            ////insuranceServiceProgram.AddMedicalServiceProviderTypes();
            ////insuranceServiceProgram.AddInsuranceContractTypes();
            ////insuranceServiceProgram.AddMedicalServiceProviders();
            ////insuranceServiceProgram.AddInsuranceContracts();

            //insuranceServiceProgram.SaveAllToXml();

            ////foreach (var o in insuranceServiceProgram.ReadEntities<InsuranceCompany>("insurance_companies.xml"))
            ////{
            ////    Console.WriteLine(EntityToStringConverter.InsuranceCompanyToString(o));
            ////}


        }

        public void Run()
        {
            PrintHelp();

            string input;
            while ((input = Console.ReadLine()) != commandToExit)
            {
                if (input.StartsWith(commandToFindInsuranceCompanyById))
                {
                    int id = int.Parse(input.Remove(0, commandToFindInsuranceCompanyById.Length + 1));
                    Console.WriteLine(EntityToStringConverter.InsuranceCompanyToString(FindInsuranceCompanyById(id)));
                }
                else if (input.StartsWith(commandToSaveAllInsuranceCompanies))
                {
                    string path = input.Remove(0, commandToSaveAllInsuranceCompanies.Length + 1);
                    SaveInsuranceCompaniesToXml(path);
                }
                else if (input == commandToExit)
                {
                    break;
                }
                else if (input == commandToPrintHelp)
                {
                    PrintHelp();
                }
                else if (input == commandToPrint)
                {
                    PrintDatabaseContent();
                }                
                else if (input == commandToPrintAllInsuranceCompanies)
                {
                    PrintInsuranceCompanies();
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }
            }
        }


        // -----------------------------------------------------------------------------------------------------------------------1
        private EntityCollectionWrapper<T> DeserializeObject<T>(string path)
        {
            XmlSerializer serializer =
            new XmlSerializer(typeof(EntityCollectionWrapper<T>));

            EntityCollectionWrapper<T> i;

            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                i = serializer.Deserialize(reader) as EntityCollectionWrapper<T>;
            }

            return i;
        }

        public List<T> ReadEntities<T>(string path)
        {
            EntityCollectionWrapper<T> entityCollectionWrapper = DeserializeObject<T>(path);

            return entityCollectionWrapper.EntityList;
        }
        // -----------------------------------------------------------------------------------------------------------------------




        // -----------------------------------------------------------------------------------------------------------------------
        public void SaveAllToXml()
        {
            SaveInsuranceCompaniesToXml("insurance_companies.xml");
            SaveMedicalServiceProviderTypesToXml("medical_service_provider_types.xml");
            SaveMedicalServiceProvidersToXml("medical_service_providers.xml");
            SaveInsuranceContractTypesToXml("insurance_contract_types.xml");
            SaveInsuranceContractsToXml("insurance_contracts.xml");
        }

        public void SaveEntitiesToXml<T>(IEnumerable<T> collectionOfEntities, string path)
        {
            SaveString(EntitiesToXml<T>(collectionOfEntities), path);
        }

        public void SaveInsuranceCompaniesToXml(string path)
        {
            SaveString(EntitiesToXml(DatabaseContext.InsuranceCompanies), path);
        }

        public void SaveMedicalServiceProviderTypesToXml(string path)
        {
            SaveString(EntitiesToXml(DatabaseContext.MedicalServiceProviderTypes), path);
        }

        public void SaveMedicalServiceProvidersToXml(string path)
        {
            SaveString(EntitiesToXml(DatabaseContext.MedicalServiceProviders), path);
        }

        public void SaveInsuranceContractTypesToXml(string path)
        {
            SaveString(EntitiesToXml(DatabaseContext.InsuranceContractTypes), path);
        }

        public void SaveInsuranceContractsToXml(string path)
        {
            SaveString(EntitiesToXml(DatabaseContext.InsuranceContracts), path);
        }

        public static void SaveString(string stringToSave, string path)
        {
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(stringToSave);
            }
        }

        public string EntitiesToXml<T>(IEnumerable<T> entities)
        {
            DatabaseContext.Configuration.ProxyCreationEnabled = false;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EntityCollectionWrapper<T>));

            string xml = "";

            EntityCollectionWrapper<T> collectionWrapper = new EntityCollectionWrapper<T>();
            collectionWrapper.EntityList = new List<T>(entities);

            using (var sww = new StringWriter())
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.OmitXmlDeclaration = true;
                using (XmlWriter writer = XmlWriter.Create(sww, xmlWriterSettings))
                {
                    xmlSerializer.Serialize(writer, collectionWrapper);
                    xml += sww.ToString(); // Your XML
                }
            }

            return xml;
        }
        // -----------------------------------------------------------------------------------------------------------------------




        // -----------------------------------------------------------------------------------------------------------------------
        private void RemoveAllInsuranceCompanies()
        {
            foreach (var entity in DatabaseContext.InsuranceCompanies)
                DatabaseContext.InsuranceCompanies.Remove(entity);
            DatabaseContext.SaveChanges();
        }

        private void RemoveAllMedicalServiceProviderTypes()
        {
            foreach (var entity in DatabaseContext.MedicalServiceProviderTypes)
                DatabaseContext.MedicalServiceProviderTypes.Remove(entity);
            DatabaseContext.SaveChanges();
        }

        private void RemoveAll()
        {
            var tableNames = DatabaseContext.Database.SqlQuery<string>(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME NOT LIKE '%Migration%'").ToList();
            foreach (var tableName in tableNames)
            {
                DatabaseContext.Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
                DatabaseContext.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, 0)", tableName));
            }
            DatabaseContext.SaveChanges();
        }
        // -----------------------------------------------------------------------------------------------------------------------




        // -----------------------------------------------------------------------------------------------------------------------          
        public InsuranceCompany FindInsuranceCompanyById(int id)
        {
            var query = from o in DatabaseContext.InsuranceCompanies
                        where o.Id == id
                        select o;
            return query.FirstOrDefault();
        }

        public MedicalServiceProviderType FindMedicalServiceProviderTypeById(int id)
        {
            var query = from o in DatabaseContext.MedicalServiceProviderTypes
                        where o.Id == id
                        select o;
            return query.FirstOrDefault();
        }

        public IEnumerable<MedicalServiceProviderType> FindMedicalServiceProviderTypesByCode(string code)
        {
            return from o in DatabaseContext.MedicalServiceProviderTypes
                   where o.Code == code
                   select o;
        }

        public MedicalServiceProvider FindMedicalServiceProviderById(int id)
        {
            var query = from o in DatabaseContext.MedicalServiceProviders
                        where o.Id == id
                        select o;
            return query.FirstOrDefault();
        }

        public IEnumerable<InsuranceContractType> FindInsuranceContractTypesByCode(string code)
        {
            return from o in DatabaseContext.InsuranceContractTypes
                   where o.Code == code
                   select o;
        }
        // -----------------------------------------------------------------------------------------------------------------------   




        // -----------------------------------------------------------------------------------------------------------------------   
        public bool AddInsuranceCompany(InsuranceCompany insuranceCompany)
        {
            DatabaseContext.InsuranceCompanies.Add(insuranceCompany);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddMedicalServiceProviderType(string code, string name, DateTime validFrom, DateTime? validUntil)
        {
            MedicalServiceProviderType medicalServiceProviderType = new MedicalServiceProviderType();
            medicalServiceProviderType.Code = code;
            medicalServiceProviderType.Name = name;
            medicalServiceProviderType.ValidFrom = validFrom;
            medicalServiceProviderType.ValidUntil = validUntil;
            DatabaseContext.MedicalServiceProviderTypes.Add(medicalServiceProviderType);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddMedicalServiceProviderType(MedicalServiceProviderType medicalServiceProviderType)
        {
            DatabaseContext.MedicalServiceProviderTypes.Add(medicalServiceProviderType);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddInsuranceContractType(InsuranceContractType insuranceContractType)
        {
            DatabaseContext.InsuranceContractTypes.Add(insuranceContractType);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddMedicalServiceProvider(MedicalServiceProvider medicalServiceProvider)
        {
            DatabaseContext.MedicalServiceProviders.Add(medicalServiceProvider);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        public bool AddInsuranceContract(InsuranceContract insuranceContract)
        {
            DatabaseContext.InsuranceContracts.Add(insuranceContract);
            return DoCommonStepsAfterAdditionToDatabase();
        }

        private bool DoCommonStepsAfterAdditionToDatabase()
        {
            try
            {
                DatabaseContext.SaveChanges();

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
        // -----------------------------------------------------------------------------------------------------------------------   




        // -----------------------------------------------------------------------------------------------------------------------   
        public void PrintHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
                @"{0}        - exit{1}",
                commandToExit, Environment.NewLine);
            sb.AppendFormat(
                @"{0}        - print help{1}",
                commandToPrintHelp, Environment.NewLine);
            sb.AppendFormat(
                @"{0}        - print database content{1}",
                commandToPrint, Environment.NewLine);
            sb.AppendFormat(
                @"{0}       - print insurance companies{1}",
                commandToPrintAllInsuranceCompanies, Environment.NewLine);
            sb.AppendFormat(
                @"{0} PATH  - save insurance companies{1}",
                commandToSaveAllInsuranceCompanies, Environment.NewLine);
            sb.AppendFormat(
                @"{0} ID   - find insurance company by ID{1}",
                commandToFindInsuranceCompanyById, Environment.NewLine);

            Console.WriteLine(sb.ToString());
        }

        public void PrintDatabaseContent()
        {
            PrintInsuranceCompanies();
            Console.WriteLine("\n\n");
            PrintMedicalServiceProviderTypes();
            Console.WriteLine("\n\n");
            PrintInsuranceContractTypes();
            Console.WriteLine("\n\n");
            PrintMedicalServiceProviders();
            Console.WriteLine("\n\n");
            PrintInsuranceContracts();
            Console.WriteLine("\n\n");
        }

        public void PrintInsuranceCompanies()
        {
            Console.WriteLine("========= INSURANCE COMPANIES =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.InsuranceCompanies)
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
            foreach (var o in DatabaseContext.MedicalServiceProviderTypes)
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
            foreach (var o in DatabaseContext.InsuranceContractTypes)
            {
                Console.WriteLine(EntityToStringConverter.InsuranceContractTypeToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("============================================");
        }

        public void PrintMedicalServiceProviders()
        {
            Console.WriteLine("========= MEDICAL SERVICE PROVIDERS =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.MedicalServiceProviders)
            {
                Console.WriteLine(EntityToStringConverter.MedicalServiceProviderToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("=============================================");
        }

        public void PrintInsuranceContracts()
        {
            Console.WriteLine("========= INSURANCE CONTRACTS =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.InsuranceContracts)
            {
                Console.WriteLine(o);
                Console.WriteLine("\n");
            }
            Console.WriteLine("=======================================");
        }
        // -----------------------------------------------------------------------------------------------------------------------   




        // -----------------------------------------------------------------------------------------------------------------------   
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

        private void AddMedicalServiceProviders()
        {

            //Id Nazev   TypPZS
            //1   MUDr.Lucie Nováková    P2
            //2   Fakultní nemocnice Praha P3
            //3   MDDr.Pavel Zoubek  P1

            try
            {
                AddMedicalServiceProvider(new MedicalServiceProvider()
                {
                    Name = "MUDr.Lucie Nováková",
                    Type = (new List<MedicalServiceProviderType>(FindMedicalServiceProviderTypesByCode("P2")))[0].Id
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddMedicalServiceProvider(new MedicalServiceProvider()
                {
                    Name = "Fakultní nemocnice Praha",
                    Type = (new List<MedicalServiceProviderType>(FindMedicalServiceProviderTypesByCode("P3")))[0].Id
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddMedicalServiceProvider(new MedicalServiceProvider()
                {
                    Name = "MDDr.Pavel Zoubek",
                    Type = (new List<MedicalServiceProviderType>(FindMedicalServiceProviderTypesByCode("P1")))[0].Id
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void AddInsuranceContracts()
        {

            //Id PoskytovatelZdravotnichSluzebId PojistovnaId TypSmlouvy  DatumOd DatumDo
            //1   1   1   A001    6 / 5 / 2010    11 / 10 / 2010
            //2   1   1   A001    6 / 5 / 2013    11 / 10 / 2013
            //3   1   1   A001    6 / 5 / 2016
            //4   2   1   A002    6 / 5 / 2016
            //5   3   1   A001    6 / 5 / 2010    6 / 4 / 2016
            //6   3   1   A001    6 / 5 / 2016


            try
            {
                AddInsuranceContract(new InsuranceContract()
                {
                    MedicalServiceProviderId = FindMedicalServiceProviderById(1).Id,
                    InsuranceCompanyId = FindInsuranceCompanyById(1).Id,
                    TypeId = (new List<InsuranceContractType>(FindInsuranceContractTypesByCode("A001")))[0].Id,
                    ValidFrom = new DateTime(2010, 5, 6),
                    ValidUntil = new DateTime(2010, 10, 11)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContract(new InsuranceContract()
                {
                    MedicalServiceProviderId = FindMedicalServiceProviderById(1).Id,
                    InsuranceCompanyId = FindInsuranceCompanyById(1).Id,
                    TypeId = (new List<InsuranceContractType>(FindInsuranceContractTypesByCode("A001")))[0].Id,
                    ValidFrom = new DateTime(2013, 5, 6),
                    ValidUntil = new DateTime(2010, 10, 11)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContract(new InsuranceContract()
                {
                    MedicalServiceProviderId = FindMedicalServiceProviderById(1).Id,
                    InsuranceCompanyId = FindInsuranceCompanyById(1).Id,
                    TypeId = (new List<InsuranceContractType>(FindInsuranceContractTypesByCode("A001")))[0].Id,
                    ValidFrom = new DateTime(2016, 5, 6),
                    ValidUntil = null
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContract(new InsuranceContract()
                {
                    MedicalServiceProviderId = FindMedicalServiceProviderById(2).Id,
                    InsuranceCompanyId = FindInsuranceCompanyById(1).Id,
                    TypeId = (new List<InsuranceContractType>(FindInsuranceContractTypesByCode("A002")))[0].Id,
                    ValidFrom = new DateTime(2016, 5, 6),
                    ValidUntil = null
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContract(new InsuranceContract()
                {
                    MedicalServiceProviderId = FindMedicalServiceProviderById(3).Id,
                    InsuranceCompanyId = FindInsuranceCompanyById(1).Id,
                    TypeId = (new List<InsuranceContractType>(FindInsuranceContractTypesByCode("A001")))[0].Id,
                    ValidFrom = new DateTime(2010, 5, 6),
                    ValidUntil = new DateTime(2016, 4, 6)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                AddInsuranceContract(new InsuranceContract()
                {
                    MedicalServiceProviderId = FindMedicalServiceProviderById(3).Id,
                    InsuranceCompanyId = FindInsuranceCompanyById(1).Id,
                    TypeId = (new List<InsuranceContractType>(FindInsuranceContractTypesByCode("A001")))[0].Id,
                    ValidFrom = new DateTime(2016, 5, 6),
                    ValidUntil = null
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // -----------------------------------------------------------------------------------------------------------------------   




        // -----------------------------------------------------------------------------------------------------------------------   
        public InsuranceServiceDatabaseEntities DatabaseContext
        {
            get
            {
                if (database == null)
                    database = new InsuranceServiceDatabaseEntities();
                database.Configuration.ProxyCreationEnabled = false;
                return database;
            }
        }
    }

    public class EntityCollectionWrapper<T>
    {
        public List<T> EntityList { get; set; }
    }
}
