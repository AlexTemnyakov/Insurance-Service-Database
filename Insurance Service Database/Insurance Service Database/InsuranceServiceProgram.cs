using Insurance_Service_Database.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private static readonly string commandToResetDatabase = "r";
        private static readonly string commandToConfirm = "+";
        private static readonly string commandToFillWithDefaultData = "d";
        private static readonly string commandToPrint = "p";
        private static readonly string commandToSave = "s";
        private static readonly string commandToFind = "f";
        private static readonly string commandToFindById = "i";
        private static readonly string commandToFindByDate = "d";
        private static readonly string commandToLoad = "l";

        private static readonly string insuranceCompanyCode = "1";
        private static readonly string medicalServiceProviderTypeCode = "2";
        private static readonly string medicalServiceProviderCode = "3";
        private static readonly string insuranceContractTypeCode = "4";
        private static readonly string insuranceContractsCode = "5";

        private static readonly string commandToPrintAllInsuranceCompanies = commandToPrint + insuranceCompanyCode;
        private static readonly string commandToPrintAllMedicalServiceProviderTypes = commandToPrint + medicalServiceProviderTypeCode;
        private static readonly string commandToPrintAllMedicalServiceProviders = commandToPrint + medicalServiceProviderCode;
        private static readonly string commandToPrintAllInsuranceContractTypes = commandToPrint + insuranceContractTypeCode;
        private static readonly string commandToPrintAllInsuranceContracts = commandToPrint + insuranceContractsCode;

        private static readonly string commandToSaveAllInsuranceCompanies = commandToSave + insuranceCompanyCode;
        private static readonly string commandToSaveAllMedicalServiceProviderTypes = commandToSave + medicalServiceProviderTypeCode;
        private static readonly string commandToSaveAllMedicalServiceProviders = commandToSave + medicalServiceProviderCode;
        private static readonly string commandToSaveAllInsuranceContractTypes = commandToSave + insuranceContractTypeCode;
        private static readonly string commandToSaveAllInsuranceContracts = commandToSave + insuranceContractsCode;

        private static readonly string commandToFindInsuranceCompanyById = commandToFind + insuranceCompanyCode + commandToFindById;
        private static readonly string commandToFindMedicalServiceProviderTypeById = commandToFind + medicalServiceProviderTypeCode + commandToFindById;
        private static readonly string commandToFindMedicalServiceProviderById = commandToFind + medicalServiceProviderCode + commandToFindById;
        private static readonly string commandToFindInsuranceContractTypeById = commandToFind + insuranceContractTypeCode + commandToFindById;
        private static readonly string commandToFindInsuranceContractsById = commandToFind + insuranceContractsCode + commandToFindById;
        private static readonly string commandToFindInsuranceContractsByDate = commandToFind + insuranceContractsCode + commandToFindByDate;

        private static readonly string commandToLoadNewInsuranceCompanies = commandToLoad + insuranceCompanyCode;
        private static readonly string commandToLoadNewMedicalServiceProviders = commandToLoad + medicalServiceProviderCode;
        private static readonly string commandToLoadNewInsuranceContracts = commandToLoad + insuranceContractsCode;

        private static readonly string notFoundText = "Not found.";
        private static readonly string queryResultText = "Your query result:";
        private static readonly string loadedEntitiesText = "Loaded entities:";
        private static readonly string noEntitiesLoaded = "No entities loaded.";
        private static readonly string validText = "Valid";
        private static readonly string invalidText = "Invalid";
        private static readonly string savedInText = "Saved in";
        private static readonly string readingFromText = "Reading from ";
        private static readonly string exitText = "Exiting.";
        private static readonly string greetingsText = "Hello!";
        private static readonly string goodbyeText = "Goodbye!";
        private static readonly string pressAnyButtonToClose = "Press any button to close.";
        private static readonly string spaceDelimiter = "\n\n\n";
        private static readonly string majorDelimiter
            = "\n\n\n"
            + "=========================================================================================="
            + "\n\n\n";
        private static readonly string minorDelimiter
            = "\n"
            + "--------------------------------------"
            + "\n";

        private static readonly string dataFolder = "Data";

        private static readonly ConsoleColor normalBackgroundColor1 = ConsoleColor.Yellow;
        private static readonly ConsoleColor normalForegroundColor1 = ConsoleColor.Black;
        private static readonly ConsoleColor exceptionBackgroundColor1 = ConsoleColor.Red;
        private static readonly ConsoleColor exceptionForegroundColor1 = ConsoleColor.White;

        private InsuranceServiceDatabaseEntities databaseContext;

        static void Main(string[] args)
        {          
            InsuranceServiceProgram insuranceServiceProgram = new InsuranceServiceProgram();

            insuranceServiceProgram.Run();
        }

        public void Run()
        {
            Console.WriteLine(greetingsText);
            Console.WriteLine("\n");

            PrintHelp();

            string input;
            while ((input = Console.ReadLine()) != commandToExit)
            {
                try
                {
                    if (input.StartsWith(commandToFindInsuranceCompanyById))
                    {
                        int id = RetrieveIdFromInput(input, commandToFindInsuranceCompanyById);

                        var result = FindInsuranceCompanyById(id);

                        Console.WriteLine();
                        if (result == null)
                            Console.WriteLine(notFoundText);
                        else
                        {
                            Console.WriteLine(queryResultText);
                            Console.WriteLine(EntityToStringConverter.EntityToString(result));
                        }
                        Console.WriteLine();
                    }
                    if (input.StartsWith(commandToFindMedicalServiceProviderTypeById))
                    {
                        int id = RetrieveIdFromInput(input, commandToFindMedicalServiceProviderTypeById);

                        var result = FindMedicalServiceProviderTypeById(id);

                        Console.WriteLine();
                        if (result == null)
                            Console.WriteLine(notFoundText);
                        else
                        {
                            Console.WriteLine(queryResultText);
                            Console.WriteLine(EntityToStringConverter.EntityToString(result));
                        }
                    }
                    if (input.StartsWith(commandToFindMedicalServiceProviderById))
                    {
                        int id = RetrieveIdFromInput(input, commandToFindMedicalServiceProviderById);

                        var result = FindMedicalServiceProviderById(id);

                        Console.WriteLine();
                        if (result == null)
                            Console.WriteLine(notFoundText);
                        else
                        {
                            Console.WriteLine(queryResultText);
                            Console.WriteLine(EntityToStringConverter.EntityToString(result));
                        }
                    }
                    if (input.StartsWith(commandToFindInsuranceContractTypeById))
                    {
                        int id = RetrieveIdFromInput(input, commandToFindInsuranceContractTypeById);

                        var result = FindInsuranceContractTypeById(id);

                        Console.WriteLine();
                        if (result == null)
                            Console.WriteLine(notFoundText);
                        else
                        {
                            Console.WriteLine(queryResultText);
                            Console.WriteLine(EntityToStringConverter.EntityToString(result));
                        }
                    }
                    if (input.StartsWith(commandToFindInsuranceContractsById))
                    {
                        int id = RetrieveIdFromInput(input, commandToFindInsuranceContractsById);

                        var result = FindInsuranceContractById(id);

                        Console.WriteLine();
                        if (result == null)
                            Console.WriteLine(notFoundText);
                        else
                        {
                            Console.WriteLine(queryResultText);
                            Console.WriteLine(EntityToStringConverter.EntityToString(result));
                        }
                    }
                    if (input.StartsWith(commandToFindInsuranceContractsByDate))
                    {
                        string dateString = input.Replace(commandToFindInsuranceContractsByDate + " ", "");

                        DateTime date = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        var result = FindInsuranceContracts(date);

                        Console.WriteLine();
                        if (result == null)
                            Console.WriteLine(notFoundText);
                        else
                        {
                            Console.WriteLine(queryResultText);
                            foreach (var o in result)
                            {
                                Console.WriteLine(EntityToStringConverter.EntityToString(o));
                            }
                        }
                    }
                    else if (input.StartsWith(commandToSaveAllInsuranceCompanies))
                    {
                        string path = RetrieveStringFromInput(input, commandToSaveAllInsuranceCompanies);

                        SaveInsuranceCompaniesToXml(path);
                    }
                    else if (input.StartsWith(commandToSaveAllMedicalServiceProviderTypes))
                    {
                        string path = RetrieveStringFromInput(input, commandToSaveAllMedicalServiceProviderTypes);

                        SaveMedicalServiceProviderTypesToXml(path);
                    }
                    else if (input.StartsWith(commandToSaveAllMedicalServiceProviders))
                    {
                        string path = RetrieveStringFromInput(input, commandToSaveAllMedicalServiceProviders);

                        SaveMedicalServiceProvidersToXml(path);
                    }
                    else if (input.StartsWith(commandToSaveAllInsuranceContractTypes))
                    {
                        string path = RetrieveStringFromInput(input, commandToSaveAllInsuranceContractTypes);

                        SaveInsuranceContractTypesToXml(path);
                    }
                    else if (input.StartsWith(commandToSaveAllInsuranceContracts))
                    {
                        string path = RetrieveStringFromInput(input, commandToSaveAllInsuranceContracts);

                        SaveInsuranceContractsToXml(path);
                    }
                    else if (input.StartsWith(commandToLoadNewInsuranceCompanies))
                    {
                        string path = RetrieveStringFromInput(input, commandToLoadNewInsuranceCompanies);

                        var loadedEntities = ReadEntities<InsuranceCompany>(path);

                        Console.WriteLine(majorDelimiter);
                        if (loadedEntities != null && loadedEntities.Count > 0)
                        {
                            Console.WriteLine(loadedEntitiesText);
                            Console.WriteLine(minorDelimiter);

                            for (int i = 0; i < loadedEntities.Count; i++)
                            {
                                var company = loadedEntities[i];

                                Console.WriteLine(i);
                                Console.WriteLine(EntityToStringConverter.EntityToString(company));
                                Console.WriteLine();

                                AddInsuranceCompany(company);
                                Console.WriteLine(minorDelimiter);
                            }
                        }
                        else
                        {
                            Console.WriteLine(noEntitiesLoaded);
                        }

                        Console.WriteLine(majorDelimiter);
                    }
                    else if (input.StartsWith(commandToLoadNewInsuranceContracts))
                    {
                        string path = RetrieveStringFromInput(input, commandToLoadNewInsuranceContracts);

                        var loadedEntities = ReadEntities<InsuranceContract>(path);

                        Console.WriteLine(majorDelimiter);
                        if (loadedEntities != null && loadedEntities.Count > 0)
                        {
                            Console.WriteLine(loadedEntitiesText);
                            Console.WriteLine(minorDelimiter);

                            for (int i = 0; i < loadedEntities.Count; i++)
                            {
                                var contract = loadedEntities[i];

                                Console.WriteLine(i);
                                Console.WriteLine(EntityToStringConverter.EntityToString(contract));
                                Console.WriteLine();

                                string log;
                                if (ValidateNewInsuranceContract(contract, out log))
                                {
                                    Console.WriteLine(validText);
                                    CloseContractAndAddNew(contract);
                                }
                                else
                                {
                                    Console.WriteLine(invalidText);
                                    Console.WriteLine(log);
                                }
                                Console.WriteLine(minorDelimiter);
                            }
                        }
                        else
                        {
                            Console.WriteLine(noEntitiesLoaded);
                        }

                        Console.WriteLine(majorDelimiter);
                    }
                    else if (input.StartsWith(commandToLoadNewMedicalServiceProviders))
                    {
                        string path = RetrieveStringFromInput(input, commandToLoadNewMedicalServiceProviders);

                        var loadedEntities = ReadEntities<MedicalServiceProvider>(path);

                        Console.WriteLine(majorDelimiter);
                        if (loadedEntities != null && loadedEntities.Count > 0)
                        {
                            Console.WriteLine(loadedEntitiesText);
                            Console.WriteLine(minorDelimiter);

                            for (int i = 0; i < loadedEntities.Count; i++)
                            {
                                var contract = loadedEntities[i];

                                Console.WriteLine(i);
                                Console.WriteLine(EntityToStringConverter.EntityToString(contract));
                                Console.WriteLine();

                                AddMedicalServiceProvider(contract);

                                Console.WriteLine(minorDelimiter);
                            }
                        }
                        else
                        {
                            Console.WriteLine(noEntitiesLoaded);
                        }

                        Console.WriteLine(majorDelimiter);
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
                    else if (input == commandToPrintAllMedicalServiceProviderTypes)
                    {
                        PrintMedicalServiceProviderTypes();
                    }
                    else if (input == commandToPrintAllMedicalServiceProviders)
                    {
                        PrintMedicalServiceProviders();
                    }
                    else if (input == commandToPrintAllInsuranceContractTypes)
                    {
                        PrintInsuranceContractTypes();
                    }
                    else if (input == commandToPrintAllInsuranceContracts)
                    {
                        PrintInsuranceContracts();
                    }
                    else if (input == commandToResetDatabase)
                    {
                        Console.WriteLine("Type " + commandToConfirm + " to confirm");
                        if (Console.ReadLine() == commandToConfirm)
                        {
                            RemoveAll();
                        }
                    }
                    else if (input == commandToFillWithDefaultData)
                    {
                        Console.WriteLine("Type " + commandToConfirm + " to confirm");
                        if (Console.ReadLine() == commandToConfirm)
                        {
                            RemoveAll();
                            AddDefaultData();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unknown command.");
                    }
                }
                catch (Exception e)
                {
                    Console.BackgroundColor = exceptionBackgroundColor1;
                    Console.ForegroundColor = exceptionForegroundColor1;

                    Console.WriteLine(majorDelimiter);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(majorDelimiter);

                    Console.ResetColor();
                }
            }

            Console.BackgroundColor = normalBackgroundColor1;
            Console.ForegroundColor = normalForegroundColor1;
            Console.WriteLine(majorDelimiter);
            Console.WriteLine(exitText);
            Console.WriteLine(goodbyeText);
            Console.ResetColor();
            Console.WriteLine(pressAnyButtonToClose);
            Console.ReadKey();
        }

        private static string RetrieveStringFromInput(string input, string command)
        {
            input = input.Remove(0, command.Length + 1);
            input = input.Remove(0, 1);
            input = input.Remove(input.Length - 1, 1);
            return input;
        }

        private static int RetrieveIdFromInput(string input, string command)
        {
            return int.Parse(input.Remove(0, command.Length + 1));
        }     

        // -----------------------------------------------------------------------------------------------------------------------1
        private EntityCollectionWrapper<T> DeserializeObject<T>(string path)
        {
            Console.BackgroundColor = normalBackgroundColor1;
            Console.ForegroundColor = normalForegroundColor1;
            Console.WriteLine(readingFromText + path);
            Console.ResetColor();

            path = dataFolder + "\\" + path;

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
            DatabaseContext.Configuration.ProxyCreationEnabled = false;
            SaveString(EntitiesToXml<T>(collectionOfEntities), path);
        }

        public void SaveInsuranceCompaniesToXml(string path)
        {
            SaveEntitiesToXml(DatabaseContext.InsuranceCompanies, path);
        }

        public void SaveMedicalServiceProviderTypesToXml(string path)
        {
            SaveEntitiesToXml(DatabaseContext.MedicalServiceProviderTypes, path);
        }

        public void SaveMedicalServiceProvidersToXml(string path)
        {
            SaveEntitiesToXml(DatabaseContext.MedicalServiceProviders, path);
        }

        public void SaveInsuranceContractTypesToXml(string path)
        {
            SaveEntitiesToXml(DatabaseContext.InsuranceContractTypes, path);
        }

        public void SaveInsuranceContractsToXml(string path)
        {
            SaveEntitiesToXml(DatabaseContext.InsuranceContracts, path);
        }

        public static void SaveString(string stringToSave, string path)
        {
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            path = dataFolder + "\\" + path;

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(stringToSave);
            }

            Console.WriteLine();
            Console.WriteLine(savedInText + " \"" + path + "\"");
            Console.WriteLine();
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
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { Indent = true };
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
        private void RemoveAll()
        {
            var tableNames = DatabaseContext.Database.SqlQuery<string>(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME NOT LIKE '%Migration%'").ToList();
            foreach (var tableName in tableNames)
            {
                Console.WriteLine("Removing table " + tableName);

                DatabaseContext.Database.ExecuteSqlCommand(string.Format("ALTER TABLE {0} NOCHECK CONSTRAINT ALL", tableName));
                DatabaseContext.Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
                DatabaseContext.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, 0)", tableName));
                DatabaseContext.Database.ExecuteSqlCommand(string.Format("ALTER TABLE {0} CHECK CONSTRAINT ALL", tableName));
            }

            DatabaseContext.SaveChanges();
            ResetContext();
            Console.WriteLine("Database has been successfully reseted.");
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

        public InsuranceContractType FindInsuranceContractTypeById(int id)
        {
            var query = from o in DatabaseContext.InsuranceContractTypes
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

        public InsuranceContract FindInsuranceContractById(int id)
        {
            var query = from o in DatabaseContext.InsuranceContracts
                        where o.Id == id
                        select o;
            return query.FirstOrDefault();
        }

        public IEnumerable<InsuranceContract> FindInsuranceContracts(int medicalServiceProvider, int insuranceCompany, int type)
        {
            return DatabaseContext.InsuranceContracts
                .Where(o => o.MedicalServiceProviderId == medicalServiceProvider)
                .Where(o => o.InsuranceCompanyId == insuranceCompany)
                .Where(o => o.TypeId == type);
        }

        public IEnumerable<InsuranceContract> FindInsuranceContracts(DateTime date)
        {
            return from o in DatabaseContext.InsuranceContracts
                   where o.ValidUntil == null || DateTime.Compare(o.ValidUntil.Value, date) >= 0
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
                databaseContext = null;
                Console.WriteLine(e.Message);
                //Console.WriteLine(e.InnerException);
                return false;
            }
        }

        private void CloseContractAndAddNew(InsuranceContract newContract)
        {
            var similarContracts = FindInsuranceContracts(newContract.MedicalServiceProviderId, newContract.InsuranceCompanyId, newContract.TypeId);

            foreach (var contract in similarContracts)
            {
                if (contract.ValidUntil == null)
                {
                    contract.ValidUntil = newContract.ValidFrom.AddDays(-1);
                    break;
                }
            }

            AddInsuranceContract(newContract);
        }

        private bool ValidateNewInsuranceContract(InsuranceContract newContract, out string log)
        {
            if (newContract.ValidFrom == null)
            {
                log = "The validity start date is not set.";
                return false;
            }

            if (newContract.ValidUntil != null && DateTime.Compare(newContract.ValidUntil.Value, newContract.ValidFrom) <= 0)
            {
                log = "The new contract is valid from the date which is earlier than the last day of validity.";
                return false;
            }

            var similarContracts = FindInsuranceContracts(newContract.MedicalServiceProviderId, newContract.InsuranceCompanyId, newContract.TypeId);

            foreach (var contract in similarContracts)
            {
                if (contract.ValidUntil != null && DateTime.Compare(newContract.ValidFrom, contract.ValidUntil.Value) <= 0)
                {
                    log = "There is a similar contract which is valid when the new contract starts to be valid.";
                    return false;
                }

                if (DateTime.Compare(newContract.ValidFrom, contract.ValidFrom) <= 0)
                {
                    log = "There is a similar contract the validity date of which starts later.";
                    return false;
                }
            }

            if (FindMedicalServiceProviderById(newContract.MedicalServiceProviderId) == null)
            {
                log = "Couldn't find the medical service provider with id " + newContract.MedicalServiceProviderId + ".";
                return false;
            }

            if (FindInsuranceCompanyById(newContract.InsuranceCompanyId) == null)
            {
                log = "Couldn't find the medical insurance company with id " + newContract.InsuranceCompanyId + ".";
                return false;
            }

            if (FindInsuranceContractTypeById(newContract.TypeId) == null)
            {
                log = "Couldn't find the medical insurance contract type with id " + newContract.TypeId + ".";
                return false;
            }

            log = "The new contract is valid.";

            return true;
        }
        // -----------------------------------------------------------------------------------------------------------------------   




        // -----------------------------------------------------------------------------------------------------------------------   
        public void PrintHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
                @"{0}                  - exit{1}",
                commandToExit, Environment.NewLine);
            sb.AppendFormat(
                @"{0}                  - print help{1}",
                commandToPrintHelp, Environment.NewLine);
            sb.AppendFormat(
                @"{0}                  - reset database content{1}",
                commandToResetDatabase, Environment.NewLine);
            sb.AppendFormat(
                @"{0}                  - fill database with default data (database will be reseted){1}",
                commandToFillWithDefaultData, Environment.NewLine);
            sb.AppendFormat(
                @"{0}                  - print database content{1}",
                commandToPrint, Environment.NewLine);
            sb.AppendFormat(
                @"{0}                 - print insurance companies{1}",
                commandToPrintAllInsuranceCompanies, Environment.NewLine);
            sb.AppendFormat(
               @"{0}                 - print medical service provider types{1}",
               commandToPrintAllMedicalServiceProviderTypes, Environment.NewLine);
            sb.AppendFormat(
               @"{0}                 - print medical service providers{1}",
               commandToPrintAllMedicalServiceProviders, Environment.NewLine);
            sb.AppendFormat(
                @"{0}                 - print insurance contract types{1}",
                commandToPrintAllInsuranceContractTypes, Environment.NewLine);
            sb.AppendFormat(
                @"{0}                 - print insurance contracts{1}",
                commandToPrintAllInsuranceContracts, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - save insurance companies{1}",
                commandToSaveAllInsuranceCompanies, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - save medical service provider types{1}",
                commandToSaveAllMedicalServiceProviderTypes, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - save medical service providers{1}",
                commandToSaveAllMedicalServiceProviders, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - save insurance contract types{1}",
                commandToSaveAllInsuranceContractTypes, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - save insurance contracts{1}",
                commandToSaveAllInsuranceContracts, Environment.NewLine);
            sb.AppendFormat(
                @"{0} ID             - find insurance company by ID{1}",
                commandToFindInsuranceCompanyById, Environment.NewLine);
            sb.AppendFormat(
                @"{0} ID             - find medical service provider type by ID{1}",
                commandToFindMedicalServiceProviderTypeById, Environment.NewLine);
            sb.AppendFormat(
                @"{0} ID             - find medical service provider by ID{1}",
                commandToFindMedicalServiceProviderById, Environment.NewLine);
            sb.AppendFormat(
                @"{0} ID             - find insurance contract type by ID{1}",
                commandToFindInsuranceContractTypeById, Environment.NewLine);
            sb.AppendFormat(
                @"{0} ID             - find insurance contracts by ID{1}",
                commandToFindInsuranceContractsById, Environment.NewLine);
            sb.AppendFormat(
                @"{0} dd/MM/yyyy     - find insurance contracts by date{1}",
                commandToFindInsuranceContractsByDate, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - load insurance companies and add to database{1}",
                commandToLoadNewInsuranceCompanies, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - load medical service providers and add to database{1}",
                commandToLoadNewMedicalServiceProviders, Environment.NewLine);
            sb.AppendFormat(
                "{0} \"NAME\"          - load insurance contracts and add to database{1}",
                commandToLoadNewInsuranceContracts, Environment.NewLine);

            Console.BackgroundColor = normalBackgroundColor1;
            Console.ForegroundColor = normalForegroundColor1;
            Console.WriteLine(majorDelimiter);
            Console.Write(sb.ToString());
            Console.WriteLine(majorDelimiter);
            Console.ResetColor();
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
            Console.WriteLine(spaceDelimiter);
            Console.WriteLine("========= INSURANCE COMPANIES =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.InsuranceCompanies)
            {
                Console.WriteLine(EntityToStringConverter.EntityToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("=======================================");
            Console.WriteLine(spaceDelimiter);
        }

        public void PrintMedicalServiceProviderTypes()
        {
            Console.WriteLine(spaceDelimiter);
            Console.WriteLine("========= MEDICAL SERVICE PROVIDER TYPES =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.MedicalServiceProviderTypes)
            {
                Console.WriteLine(EntityToStringConverter.EntityToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("=============================================");
            Console.WriteLine(spaceDelimiter);
        }

        public void PrintInsuranceContractTypes()
        {
            Console.WriteLine(spaceDelimiter);
            Console.WriteLine("========= INSURANCE CONTRACT TYPES =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.InsuranceContractTypes)
            {
                Console.WriteLine(EntityToStringConverter.EntityToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("============================================");
            Console.WriteLine(spaceDelimiter);
        }

        public void PrintMedicalServiceProviders()
        {
            Console.WriteLine(spaceDelimiter);
            Console.WriteLine("========= MEDICAL SERVICE PROVIDERS =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.MedicalServiceProviders)
            {
                Console.WriteLine(EntityToStringConverter.EntityToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("=============================================");
            Console.WriteLine(spaceDelimiter);
        }

        public void PrintInsuranceContracts()
        {
            Console.WriteLine(spaceDelimiter);
            Console.WriteLine("========= INSURANCE CONTRACTS =========");
            Console.WriteLine();
            foreach (var o in DatabaseContext.InsuranceContracts)
            {
                Console.WriteLine(EntityToStringConverter.EntityToString(o));
                Console.WriteLine("\n");
            }
            Console.WriteLine("=======================================");
            Console.WriteLine(spaceDelimiter);
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

        private void AddDefaultData()
        {
            AddInsuranceCompanies();
            AddMedicalServiceProviderTypes();
            AddInsuranceContractTypes();
            AddMedicalServiceProviders();
            AddInsuranceContracts();
        }
        // -----------------------------------------------------------------------------------------------------------------------   




        // ----------------------------------------------------------------------------------------------------------------------- 
        private void ResetContext()
        {
            databaseContext = null;
        }
        // ----------------------------------------------------------------------------------------------------------------------- 




        // -----------------------------------------------------------------------------------------------------------------------   
        public InsuranceServiceDatabaseEntities DatabaseContext
        {
            get
            {
                if (databaseContext == null)
                    databaseContext = new InsuranceServiceDatabaseEntities();
                //database.Configuration.ProxyCreationEnabled = false;
                return databaseContext;
            }
        }
    }

    public class EntityCollectionWrapper<T>
    {
        public List<T> EntityList { get; set; }
    }
}
