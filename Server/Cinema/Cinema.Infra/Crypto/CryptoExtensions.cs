using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Cinema.Infra.Crypto
{
    public static class CryptoExtensions
    {
        /// <summary>
        /// Método de extensão para criptografar strings.
        /// </summary>
        /// <param name="passwordToHash"></param>
        /// <returns></returns>
        public static string GenerateHash(this string passwordToHash)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToHash,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
