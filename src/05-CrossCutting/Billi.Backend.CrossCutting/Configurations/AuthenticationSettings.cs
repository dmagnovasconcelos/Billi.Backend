﻿using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Billi.Backend.CrossCutting.Configurations
{
    public class AuthenticationSettings
    {
        public string Audience { get; set; }
        public int Expiration { get; set; }
        public string Issuer { get; set; }
        public string SecurityKey { get; set; }

        public SigningCredentials SigningCredentials
        {
            get
            {
                return new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey)), SecurityAlgorithms.HmacSha256);
            }
        }

        public SymmetricSecurityKey SymmetricSecurityKey
        {
            get
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey));
            }
        }
    }
}