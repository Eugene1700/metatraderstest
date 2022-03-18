using System;
using System.Net;
using IpGeoInformer.Domain;

namespace IpGeoInformer.Helpers
{
    public static class IpHelpers
    {
        public static uint StrIpToUInt(this string ipAddress)
        {
            if (!IPAddress.TryParse(ipAddress, out var ip))
                throw new DomainException("Incorrect ip format. Use: x.x.x.x");
            return (uint) IPAddress.NetworkToHostOrder(
                (int) BitConverter.ToUInt32(ip.GetAddressBytes(), 0));
        }
    }
}