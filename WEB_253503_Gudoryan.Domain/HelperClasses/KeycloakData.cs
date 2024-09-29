using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_Gudoryan.Domain.HelperClasses
{
    public class KeycloakData
    {

        public string Host { get; set; }

        public string Realm { get; set; } 
        
        public string ClientId { get; set; }   
        
        public string ClientSecret { get; set; }    

    }
}
