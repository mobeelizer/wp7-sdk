using System;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal class MobeelizerNotificationTokenConverter : IMobeelizerNotificationTokenConverter
    {
        public string Convert(string token)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(token);
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(String.Format("{0:X}", b));
            }

            return builder.ToString();
        }
    }
}
