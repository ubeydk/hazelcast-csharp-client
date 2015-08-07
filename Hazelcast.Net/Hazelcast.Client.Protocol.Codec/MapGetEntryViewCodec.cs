using Hazelcast.Client.Protocol;
using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;
using System.Collections.Generic;

namespace Hazelcast.Client.Protocol.Codec
{
    internal sealed class MapGetEntryViewCodec
    {

        public static readonly MapMessageType RequestType = MapMessageType.MapGetEntryView;
        public const int ResponseType = 111;
        public const bool Retryable = true;

        //************************ REQUEST *************************//

        public class RequestParameters
        {
            public static readonly MapMessageType TYPE = RequestType;
            public string name;
            public IData key;
            public long threadId;

            public static int CalculateDataSize(string name, IData key, long threadId)
            {
                int dataSize = ClientMessage.HeaderSize;
                dataSize += ParameterUtil.CalculateDataSize(name);
                dataSize += ParameterUtil.CalculateDataSize(key);
                dataSize += Bits.LongSizeInBytes;
                return dataSize;
            }
        }

        public static ClientMessage EncodeRequest(string name, IData key, long threadId)
        {
            int requiredDataSize = RequestParameters.CalculateDataSize(name, key, threadId);
            ClientMessage clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
            clientMessage.SetMessageType((int)RequestType);
            clientMessage.SetRetryable(Retryable);
            clientMessage.Set(name);
            clientMessage.Set(key);
            clientMessage.Set(threadId);
            clientMessage.UpdateFrameLength();
            return clientMessage;
        }

        //************************ RESPONSE *************************//


        public class ResponseParameters
        {
            public Hazelcast.Map.SimpleEntryView<IData,IData> dataEntryView;
        }

        public static ResponseParameters DecodeResponse(IClientMessage clientMessage)
        {
            ResponseParameters parameters = new ResponseParameters();
            Hazelcast.Map.SimpleEntryView<IData,IData> dataEntryView = null;
            bool dataEntryView_isNull = clientMessage.GetBoolean();
            if (!dataEntryView_isNull)
            {
            dataEntryView = com.hazelcast.client.impl.protocol.codec.EntryViewCodec.Decode(clientMessage);
            parameters.dataEntryView = dataEntryView;
            }
            return parameters;
        }

    }
}
