using Insurance_Service_Database.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance_Service_Database
{
    class EntityToStringConverter
    {
        public static string EntityToString(InsuranceCompany insuranceCompany)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Abbreviation: {0}\nName: {1}\nID: {2}", insuranceCompany.Abbreviation, insuranceCompany.Name, insuranceCompany.Id);
            return sb.ToString();
        }

        public static string EntityToString(MedicalServiceProviderType medicalServiceProviderType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Code: {0}\nName: {1}\nID: {2}", medicalServiceProviderType.Code, medicalServiceProviderType.Name, medicalServiceProviderType.Id);
            return sb.ToString();
        }

        public static string EntityToString(MedicalServiceProvider medicalServiceProvider)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Name: {0}\nID: {1}\nType: {2}", medicalServiceProvider.Name, medicalServiceProvider.Id, medicalServiceProvider.Type);
            return sb.ToString();
        }

        public static string EntityToString(InsuranceContractType insuranceContractType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Code: {0}\nName: {1}\nID: {2}\nValid from: {3}\nValid until: {4}", 
                insuranceContractType.Code, 
                insuranceContractType.Name, 
                insuranceContractType.Id,
                insuranceContractType.ValidFrom,
                insuranceContractType.ValidUntil);
            return sb.ToString();
        }

        public static string EntityToString(InsuranceContract insuranceContract)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("ID: {0}\nMedical service provider ID: {1}\nInsurance company ID: {2}\nType ID: {3}\nValid from: {4}\nValid until: {5}",
                insuranceContract.Id,
                insuranceContract.MedicalServiceProviderId,
                insuranceContract.InsuranceCompanyId,
                insuranceContract.TypeId,
                insuranceContract.ValidFrom,
                insuranceContract.ValidUntil);
            return sb.ToString();
        }
    }
}
