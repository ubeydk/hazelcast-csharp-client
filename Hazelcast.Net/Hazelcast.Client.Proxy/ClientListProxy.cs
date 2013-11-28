using System;
using System.Collections.Generic;
using Hazelcast.Client.Proxy;
using Hazelcast.Client.Request.Collection;
using Hazelcast.Client.Spi;
using Hazelcast.Core;
using Hazelcast.IO.Serialization;
using Hazelcast.Net.Ext;


namespace Hazelcast.Client.Proxy
{
    //.NET reviewed
    public class ClientListProxy<E> : AbstractClientCollectionProxy<E>, IHazelcastList<E>
    {
        public ClientListProxy(string serviceName, string objectName) : base(serviceName, objectName)
        {
        }

        public int IndexOf(E item)
        {
            return IndexOfInternal(item, false);
        }

        public void Insert(int index, E item)
        {
            Add(index,item);
        }

        public void RemoveAt(int index)
        {
            Remove(index);
        }

        public E this[int index]
        {
            get { return Get(index); }
            set { Set(index,value); }
        }

        public E Get(int index)
        {
            ListGetRequest request = new ListGetRequest(GetName(), index);
            return Invoke<E>(request);
        }

        public E Set(int index, E element)
        {
            ThrowExceptionIfNull(element);
            Data value = ToData(element);
            ListSetRequest request = new ListSetRequest(GetName(), index, value);
            return Invoke<E>(request);
        }

        public void Add(int index, E element)
        {
			ThrowExceptionIfNull(element);
			Data value = ToData(element);
			ListAddRequest request = new ListAddRequest(GetName(), value, index);
			Invoke<object>(request);
		}

        public E Remove(int index)
        {
            ListRemoveRequest request = new ListRemoveRequest(GetName(), index);
            return Invoke<E>(request);
        }

        public int LastIndexOf(E o)
        {
            return IndexOfInternal(o, true);
        }

        public bool AddAll<_T0>(int index, ICollection<_T0> c) where _T0 : E
        {
            ThrowExceptionIfNull(c);
            IList<Data> valueList = new List<Data>(c.Count);
            foreach (E e in c)
            {
                ThrowExceptionIfNull(e);
                valueList.Add(ToData(e));
            }
            ListAddAllRequest request = new ListAddAllRequest(GetName(), valueList, index);
            bool result = Invoke<bool>(request);
            return result;
        }
        private int IndexOfInternal(object o, bool last)
        {
            ThrowExceptionIfNull(o);
            Data value = ToData(o);
            ListIndexOfRequest request = new ListIndexOfRequest(GetName(), value, last);
            int result = Invoke<int>(request);
            return result;
        }

        //public virtual Hazelcast.Net.Ext.ListIterator<E> ListIterator()
        //{
        //    return ListIterator(0);
        //}

        //public virtual Hazelcast.Net.Ext.ListIterator<E> ListIterator(int index)
        //{
        //    return SubList(-1, -1).ListIterator(index);
        //}

        public virtual IList<E> SubList(int fromIndex, int toIndex)
        {
            ListSubRequest request = new ListSubRequest(GetName(), fromIndex, toIndex);
            SerializableCollection result = Invoke<SerializableCollection>(request);
            ICollection<Data> collection = result.GetCollection();
            IList<E> list = new List<E>(collection.Count);
            foreach (Data value in collection)
            {
                list.Add((E)ToObject(value));
            }
            return list;
        }
    }
	
}