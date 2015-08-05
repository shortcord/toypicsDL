using System;
using System.Text;
using System.Management;
using System.Security.Cryptography;


/* http://stackoverflow.com/q/9039212/4366411
 * modified from above to only get model and cpuid in a md5 hash
 */
namespace ToyPics
{
    class UIDGen
    {
        private string uid;

        /// <summary>
        /// gen GUID
        /// </summary>
        public UIDGen()
        {
            try
            {
                //uid = this.HardwareProfile();
                //TODO fix this
                uid = "DEVBUILD v0.1";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Computers GUID from model and cpuid
        /// </summary>
        /// <returns>string</returns>
        public string userUID() 
        {
            return this.uid;
        }

        /// <summary>
        /// MD5 Hash of auto gen GUID
        /// </summary>
        /// <returns>string</returns>
        private string HardwareProfile()
        {

            try {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.ASCII.GetBytes(String.Format("{0}{1}", this.GetComputerModel(), this.GetCpuId()));
                    byte[] hashBytes = md5.ComputeHash(inputBytes);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }

                    return sb.ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// grab computers model
        /// </summary>
        /// <returns>string</returns>
        private string GetComputerModel()
        {
            Console.WriteLine("GetComputerModel");
            var s1 = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (ManagementObject oReturn in s1.Get())
            {
                Console.WriteLine(oReturn["Model"].ToString().Trim());
                return oReturn["Model"].ToString().Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// grabs computers cpuid
        /// </summary>
        /// <returns></returns>
        private string GetCpuId()
        {
            Console.WriteLine("GetCpuId");
            var managClass = new ManagementClass("win32_processor");
            var managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                //Get only the first CPU's ID
                Console.WriteLine(managObj.Properties["processorID"].Value.ToString());
                return managObj.Properties["processorID"].Value.ToString();
            }
            return string.Empty;
        }
    }
}
