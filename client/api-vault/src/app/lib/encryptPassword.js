import CryptoJS from "crypto-js";

const AES_KEY = "12345678901234567890123456789012"; // Must match backend
const iv = CryptoJS.enc.Hex.parse("00000000000000000000000000000000");

export function encryptPassword(rawPassword) {
  const key = CryptoJS.enc.Utf8.parse(AES_KEY);

  const encrypted = CryptoJS.AES.encrypt(rawPassword, key, {
    iv: iv,
    mode: CryptoJS.mode.CBC,
    padding: CryptoJS.pad.Pkcs7,
  });

  return encrypted.toString(); // Base64 string
}
