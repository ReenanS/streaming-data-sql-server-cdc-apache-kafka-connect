using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Configuration
{
    public class KCertConfiguration
    {
        public static readonly string DEFAULT_CONFIG_SECTION = "KaaSCert";
        public string CaCertLocation { get; set; }
        public string P12Location { get; set; }
        public string Environment {  get; set; }
        public string AppName { get; set; }
        public string Community { get; set; }
        public string Sigla {  get; set; }
        public string? CertPemLocation { get; set; }
        public string? KeyPemLocation { get; set; }
    }
}
