using System;

namespace DisqusOAuthExample.Services.Disqus.Helpers
{
	public static class DisqusSSO
	{
		/// <summary>
        /// Gets the Disqus SSO payload to authenticate users
        /// </summary>
		/// <param name="secretKey">The API secret key associated with the SSO application.</param>
        /// <param name="userId">The unique ID to associate with the user.</param>
        /// <param name="username">Non-unique name shown next to comments.</param>
        /// <param name="userEmail">User's email address, defined by RFC 5322.</param>
        /// <param name="avatarUrl">URL of the avatar image.</param>
        /// <param name="website_url">Website, blog or custom profile URL for the user, defined by RFC 3986.</param>
        /// <returns>A signed payload representing the user authentication.</returns>
        public static string GetPayload(string secretKey, string userId, string username, string userEmail, string avatarUrl = "", string websiteUrl = "")
        {
            var userdata = new
            {
                id = userId,
                username = username,
                email = userEmail,
                avatar = avatarUrl,
                url = websiteUrl
            };

            string serializedUserData = JsonConvert.SerializeObject(userdata);

            return GeneratePayload(secretKey, serializedUserData);
        }

        /// <summary>
        /// Method to get a payload to log out a user
        /// </summary>
        /// <returns>A signed, empty payload string</returns>
        public static string GetEmptyPayload()
        {
            var userdata = new { };
            string serializedUserData = JsonConvert.SerializeObject(userdata);

            return GeneratePayload(serializedUserData);
        }

        private static string GeneratePayload(string secretKey, string serializedUserData)
        {
            byte[] userDataAsBytes = StringToAscii(serializedUserData);

            // Base64 Encode the message
            string Message = System.Convert.ToBase64String(userDataAsBytes);

            // Get the proper timestamp
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            string Timestamp = Convert.ToInt32(ts.TotalSeconds).ToString();

            // Convert the message + timestamp to bytes
            byte[] messageAndTimestampBytes = StringToAscii(Message + " " + Timestamp);

            // Convert Disqus API key to HMAC-SHA1 signature
            byte[] apiBytes = StringToAscii(secretKey);
            HMACSHA1 hmac = new HMACSHA1(apiBytes);
            byte[] hashedMessage = hmac.ComputeHash(messageAndTimestampBytes);

            // Put it all together into the final payload
            return Message + " " + ByteToString(hashedMessage) + " " + Timestamp;
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        private static byte[] StringToAscii(string s) {
            byte[] retval = new byte[s.Length];
            for (int ix = 0; ix < s.Length; ++ix) {
                char ch = s[ix];
                if (ch <= 0x7f) retval[ix] = (byte)ch;
                else retval[ix] = (byte)'?';
            }
            return retval;
        }
	}
}
