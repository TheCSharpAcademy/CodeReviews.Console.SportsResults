using SportsResults.ukpagrace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsResults.ukpagrace.Interfaces
{
    public interface IEmailInterface
    {
        public void SendMail();
        public string EmailTemplate(List<Game> games);
    }
}
