using System;
using System.IO;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;


/* http://stackoverflow.com/q/9039212/4366411
 * modifed from above to only get model and cpuid in a md5 hash
 */
namespace ToyPics
{
    class UIDGen
    {
        private string appdataFolder = String.Format(@"{0}\ShortCord\toypicsDL\", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        private string uid;

        public UIDGen()
        {
            try
            {
                while (true)
                {
                    string file = String.Concat(appdataFolder, "uid");

                    if (this.tempExists()) // if folder exists
                    {
                        if (this.tempExists(file)) // if file exists
                        {
                            this.uid = File.ReadAllText(file); // read file
                            break;
                            // this should break the while loop cause uid is nolonger null
                        }
                        else
                        {
                            using (StreamWriter writer = new StreamWriter(file)) // create file and write to it
                            {
                                writer.Write(this.HardwareProfile());
                                writer.Close();
                                break;
                            }
                        }
                    }
                    else // folder doesnt exist; so create it and make the uid file
                    {
                        Directory.CreateDirectory(appdataFolder);
                        File.Create(file);
                        Thread.Sleep(1000);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public string userUID() 
        {
            return this.uid;
        }

        private bool tempExists()
        {
            return Directory.Exists(appdataFolder);
        }

        private bool tempExists(string file)
        {
            return File.Exists(file);
        }

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

        private string GetComputerModel()
        {
            Console.WriteLine("GetComputerModel");
            var s1 = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (ManagementObject oReturn in s1.Get())
            {
                Debug.WriteLine(oReturn["Model"].ToString().Trim());
                return oReturn["Model"].ToString().Trim();
            }
            return string.Empty;
        }

        private string GetCpuId()
        {
            Console.WriteLine("GetCpuId");
            var managClass = new ManagementClass("win32_processor");
            var managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                //Get only the first CPU's ID
                Debug.WriteLine(managObj.Properties["processorID"].Value.ToString());
                return managObj.Properties["processorID"].Value.ToString();
            }
            return string.Empty;
        }
    }
}
