﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seguridad.Interfaces
{
        public interface IEncryptionService
        {
            string Encrypt(string plainText);
            string Decrypt(string cipherText);
        }
    
}
