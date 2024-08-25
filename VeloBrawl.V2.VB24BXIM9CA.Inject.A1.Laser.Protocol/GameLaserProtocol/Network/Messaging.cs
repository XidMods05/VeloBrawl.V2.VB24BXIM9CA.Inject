namespace VeloBrawl.V2.VB24BXIM9CA.Inject.A1.Laser.Protocol.GameLaserProtocol.Network;

public static class Messaging
{
    /// <summary>
    ///     Reads message header in tuple version.
    /// </summary>
    /// <param name="headerBuffer">byteArray of received packet.</param>
    /// <returns>(type, length, version)</returns>
    public static (int, int, int) ReadHeader(byte[] headerBuffer)
    {
        var v1 = (headerBuffer[0] << 8) | headerBuffer[1]; // messageType (int16)
        var v2 = (headerBuffer[2] << 16) | (headerBuffer[3] << 8) | headerBuffer[4]; // messageLength (int24)
        var v3 = (headerBuffer[5] << 8) | headerBuffer[6]; // messageVersion (int16)

        return (v1, v2, v3);
    }

    /// <summary>
    ///     Writes message header.
    /// </summary>
    /// <param name="payload">byteArray of message</param>
    /// <param name="t">messageType</param>
    /// <param name="v">messageVersion</param>
    /// <returns></returns>
    public static byte[] WriteHeader(byte[] payload, int t, int v)
    {
        var final = new byte[payload.Length + 7];
        {
            // int16
            final[0] = (byte)(t >> 8);
            final[1] = (byte)t;
            // messageType

            // int24
            final[2] = (byte)(payload.Length >> 16);
            final[3] = (byte)(payload.Length >> 8);
            final[4] = (byte)payload.Length;
            // messageLength

            // int16
            final[5] = (byte)(v >> 8);
            final[6] = (byte)v;
            // messageVersion
        }

        return final;
    }
}