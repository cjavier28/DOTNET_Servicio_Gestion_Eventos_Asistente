using Seguridad.Interfaces;
using Seguridad.Recursos;
using System;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Modelos.Models;

namespace Seguridad
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IConfiguration _configuration;

        public EncryptionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método para convertir una cadena hexadecimal a bytes
        private static byte[] HexStringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        // Cifrado AES-256
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("El texto a cifrar no puede ser vacío.");

            using (Aes aesAlg = Aes.Create())
            {
                // Convertir la clave hexadecimal a bytes
                aesAlg.Key = HexStringToByteArray(RecursoSeguridad.KeySeguridad);
                aesAlg.IV = Encoding.UTF8.GetBytes(RecursoSeguridad._IV);  // Convertir el IV a bytes
                aesAlg.Mode = CipherMode.CBC;  // Establecer el modo de operación
                aesAlg.Padding = PaddingMode.PKCS7; // Establecer el relleno (Padding)

                // Cifrar el texto
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                    swEncrypt.Close();  // Escribir el texto en el flujo de cifrado

                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);  // Retornar el texto cifrado en Base64
                }
            }
        }

        // Descifrado AES-256
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("El texto cifrado no puede ser vacío.");

            using (Aes aesAlg = Aes.Create())
            {
                // Convertir la clave hexadecimal a bytes
                aesAlg.Key = HexStringToByteArray(RecursoSeguridad.KeySeguridad);
                aesAlg.IV = Encoding.UTF8.GetBytes(RecursoSeguridad._IV);  // Convertir el IV a bytes
                aesAlg.Mode = CipherMode.CBC;  // Establecer el modo de operación
                aesAlg.Padding = PaddingMode.PKCS7; // Establecer el relleno (Padding)

                // Descifrar el texto
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();  // Leer y devolver el texto descifrado
                }
            }
        }


        public string EncriptarContrasena(string pContrasena)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesContrasena = sha256.ComputeHash(Encoding.UTF8.GetBytes(pContrasena));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytesContrasena.Length; i++)
                {
                    sb.Append(bytesContrasena[i].ToString("x2"));
                }

                //Contraseña encriptada
                return sb.ToString();
            }
        }
        
        public string GenerarJWT(UsuarioConsulta modelo)
        {
            var userClaims = new[]
            {  
                new Claim(ClaimTypes.Name, modelo.Correo_Usuario),
                new Claim(ClaimTypes.Email, modelo.Correo_Usuario)
            };

            //creación de la Llave de Seguridad
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));

            //creación de las Credenciales de seguridad
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Parametrización del Token
            var configurationJWT = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
            );

            //Token generado
            var token = new JwtSecurityTokenHandler().WriteToken(configurationJWT);

            return token;
        }

        public bool ValidarToken(string token)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false, //valida q las apps externas puedan usar la URL donde se encuentra nuestra Api
                ValidateAudience = false, //quienes pueden acceder a nuestra Api
                ValidateLifetime = true, //valida el tiempo de vida del Token
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!))
            };

            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return claimsPrincipal.Identity.IsAuthenticated;
            }
            catch (Exception)
            {
                return false;
            }
        }

       
    }
}
