namespace AuthenticationClientByAvoidKeypro
{
    using System;
    using System.Management;

    internal class DeviceInformation
    {
        public string biosId()
        {
            ManagementObjectCollection instances = new ManagementClass("Win32_BaseBoard").GetInstances();
            return this.identifier(instances, "SerialNumber");
        }

        public string cpuId()
        {
            ManagementObjectCollection instances = new ManagementClass("Win32_Processor").GetInstances();
            string str = this.identifier(instances, "UniqueId");
            if (str != "")
            {
                return str;
            }
            str = this.identifier(instances, "ProcessorId");
            if (str != "")
            {
                return str;
            }
            str = this.identifier(instances, "Name");
            if (str == "")
            {
                str = this.identifier(instances, "Manufacturer");
            }
            return (str + this.identifier(instances, "MaxClockSpeed"));
        }

        private string identifier(ManagementObjectCollection moc, string wmiProperty)
        {
            string str = "";
            foreach (ManagementObject obj2 in moc)
            {
                if (str == "")
                {
                    try
                    {
                        return obj2[wmiProperty].ToString();
                    }
                    catch
                    {
                    }
                }
            }
            return str;
        }

        private string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string str = "";
            ManagementClass class2 = new ManagementClass(wmiClass);
            foreach (ManagementObject obj2 in class2.GetInstances())
            {
                if ((obj2[wmiMustBeTrue].ToString() == "True") && (str == ""))
                {
                    try
                    {
                        return obj2[wmiProperty].ToString();
                    }
                    catch
                    {
                    }
                }
            }
            return str;
        }

        public string macId() => 
            this.identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");

        public string UUID()
        {
            ManagementObjectCollection instances = new ManagementClass("Win32_ComputerSystemProduct").GetInstances();
            return this.identifier(instances, "UUID");
        }
    }
}

