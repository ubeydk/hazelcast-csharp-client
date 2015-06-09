using System.Collections.Generic;
using Hazelcast.Client.Protocol;
using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;
using Hazelcast.Net.Ext;

namespace Hazelcast.Client.Protocol.Codec
{
	internal sealed class ListIteratorCodec
	{
		public static readonly ListMessageType RequestType = ListMessageType.ListIterator;

		public const int ResponseType = 106;

		public const bool Retryable = true;

		public class RequestParameters
		{
			public static readonly ListMessageType Type = RequestType;

			public string name;

			//************************ REQUEST *************************//
			public static int CalculateDataSize(string name)
			{
				int dataSize = ClientMessage.HeaderSize;
				dataSize += ParameterUtil.CalculateStringDataSize(name);
				return dataSize;
			}
		}

		public static ClientMessage EncodeRequest(string name)
		{
			int requiredDataSize = ListIteratorCodec.RequestParameters.CalculateDataSize(name);
			ClientMessage clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
			clientMessage.SetMessageType(RequestType.Id());
			clientMessage.SetRetryable(Retryable);
			clientMessage.Set(name);
			clientMessage.UpdateFrameLength();
			return clientMessage;
		}

		public static ListIteratorCodec.RequestParameters DecodeRequest(ClientMessage clientMessage)
		{
			ListIteratorCodec.RequestParameters parameters = new ListIteratorCodec.RequestParameters();
			string name;
			name = null;
			name = clientMessage.GetStringUtf8();
			parameters.name = name;
			return parameters;
		}

		public class ResponseParameters
		{
			public ICollection<IData> list;

			//************************ RESPONSE *************************//
			public static int CalculateDataSize(ICollection<IData> list)
			{
				int dataSize = ClientMessage.HeaderSize;
				dataSize += Bits.IntSizeInBytes;
				foreach (IData list_item in list)
				{
					dataSize += ParameterUtil.CalculateDataSize(list_item);
				}
				return dataSize;
			}
		}

		public static ClientMessage EncodeResponse(ICollection<IData> list)
		{
			int requiredDataSize = ListIteratorCodec.ResponseParameters.CalculateDataSize(list);
			ClientMessage clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
			clientMessage.SetMessageType(ResponseType);
			clientMessage.Set(list.Count);
			foreach (IData list_item in list)
			{
				clientMessage.Set(list_item);
			}
			clientMessage.UpdateFrameLength();
			return clientMessage;
		}

		public static ListIteratorCodec.ResponseParameters DecodeResponse(ClientMessage clientMessage)
		{
			ListIteratorCodec.ResponseParameters parameters = new ListIteratorCodec.ResponseParameters();
			IList<IData> list;
			list = null;
			int list_size = clientMessage.GetInt();
			list = new AList<IData>(list_size);
			for (int list_index = 0; list_index < list_size; list_index++)
			{
				IData list_item;
				list_item = clientMessage.GetData();
				list.AddItem(list_item);
			}
			parameters.list = list;
			return parameters;
		}
	}
}
