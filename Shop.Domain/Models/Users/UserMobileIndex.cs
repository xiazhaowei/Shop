using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Users
{
    public class UserMobileIndex
    {
        public string IndexId { get; private set; }
        public Guid UserId { get; private set; }
        public string Mobile { get; private set; }

        public UserMobileIndex(string indexId, Guid userId, string mobile)
        {
            IndexId = indexId;
            UserId = userId;
            Mobile = mobile;
        }
    }
}
