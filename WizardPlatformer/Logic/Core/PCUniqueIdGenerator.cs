using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WizardPlatformer {
	public static class PCUniqueIdGenerator {
		public static PhysicalAddress GetMacAddress() {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up) {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        public static string GetMacAddressHash() {
            PhysicalAddress mac = GetMacAddress();

            return mac.ToString().GetHashCode().ToString();
        }
	}
}
