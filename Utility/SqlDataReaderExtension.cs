using Microsoft.Data.SqlClient;

namespace SistemaGeneral.Utility {
    public static class SqlDataReaderExtension {

        public static bool SafeBool(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? false : reader.GetBoolean(index);
        }

        public static byte SafeByte(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? (byte)0 : reader.GetByte(index);
        }

        public static short SafeShort(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? (short)0 : reader.GetInt16(index);
        }

        public static int SafeInt(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? 0 : reader.GetInt32(index);
        }

        public static decimal SafeDecimal(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? 0 : reader.GetDecimal(index);
        }

        public static float SafeFloat(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? 0f : reader.GetFloat(index);
        }

        public static string SafeString(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? "" : reader.GetString(index);
        }

        public static DateTime SafeDateTime(this SqlDataReader reader, int index) {
            return reader.IsDBNull(index) ? default : reader.GetDateTime(index);
        }


    }


}
