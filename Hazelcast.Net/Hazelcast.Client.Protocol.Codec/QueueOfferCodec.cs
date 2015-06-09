using Hazelcast.Client.Protocol;
using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;
using Hazelcast.Net.Ext;

namespace Hazelcast.Client.Protocol.Codec
{
	internal sealed class QueueOfferCodec
	{
		public static readonly QueueMessageType RequestType = QueueMessageType.QueueOffer;

		public const int ResponseType = 101;

		public const bool Retryable = false;

		public class RequestParameters
		{
			public static readonly QueueMessageType Type = RequestType;

			public string name;

			public IData value;

			public long timeoutMillis;

			//************************ REQUEST *************************//
			public static int CalculateDataSize(string name, IData value, long timeoutMillis)
			{
				int dataSize = ClientMessage.HeaderSize;
				dataSize += ParameterUtil.CalculateStringDataSize(name);
				dataSize += ParameterUtil.CalculateDataSize(value);
				dataSize += Bits.LongSizeInBytes;
				return dataSize;
			}
		}

		public static ClientMessage EncodeRequest(string name, IData value, long timeoutMillis)
		{
			int requiredDataSize = QueueOfferCodec.RequestParameters.CalculateDataSize(name, value, timeoutMillis);
			ClientMessage clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
			clientMessage.SetMessageType(RequestType.Id());
			clientMessage.SetRetryable(Retryable);
			clientMessage.Set(name);
			clientMessage.Set(value);
			clientMessage.Set(timeoutMillis);
			clientMessage.UpdateFrameLength();
			return clientMessage;
		}

		public static QueueOfferCodec.RequestParameters DecodeRequest(ClientMessage clientMessage)
		{
			QueueOfferCodec.RequestParameters parameters = new QueueOfferCodec.RequestParameters();
			string name;
			name = null;
			name = clientMessage.GetStringUtf8();
			parameters.name = name;
			IData value;
			value = null;
			value = clientMessage.GetData();
			parameters.value = value;
			long timeoutMillis;
			timeoutMillis = clientMessage.GetLong();
			parameters.timeoutMillis = timeoutMillis;
			return parameters;
		}

		public class ResponseParameters
		{
			public bool response;

			//************************ RESPONSE *************************//
			public static int CalculateDataSize(bool response)
			{
				int dataSize = ClientMessage.HeaderSize;
				dataSize += Bits.BooleanSizeInBytes;
				return dataSize;
			}
		}

		public static ClientMessage EncodeResponse(bool response)
		{
			int requiredDataSize = QueueOfferCodec.ResponseParameters.CalculateDataSize(response);
			ClientMessage clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
			clientMessage.SetMessageType(ResponseType);
			clientMessage.Set(response);
			clientMessage.UpdateFrameLength();
			return clientMessage;
		}

		public static QueueOfferCodec.ResponseParameters DecodeResponse(ClientMessage clientMessage)
		{
			QueueOfferCodec.ResponseParameters parameters = new QueueOfferCodec.ResponseParameters();
			bool response;
			response = clientMessage.GetBoolean();
			parameters.response = response;
			return parameters;
		}
	}
}
