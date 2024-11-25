using Seguridad.Recursos;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Seguridad
{
    public class EncryptionService
    {


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
    }
}
