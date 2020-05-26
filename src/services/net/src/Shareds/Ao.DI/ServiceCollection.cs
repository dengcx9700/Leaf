using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ao.DI
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly List<ServiceDescriptor> services;
        public ServiceDescriptor this[int index]
        {
            get => services[index];
            set => services[index] = value;
        }

        public int Count => services.Count;

        bool ICollection<ServiceDescriptor>.IsReadOnly => false;
        public ServiceCollection()
        {
            services = new List<ServiceDescriptor>();
        }
        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
        {
            services.Add(item);
        }

        public void Clear()
        {
            services.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            return services.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            Array.Copy(services.ToArray(), 0, array, arrayIndex, Count);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return services.GetEnumerator();
        }

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ServiceDescriptor item)
        {
            return services.Remove(item);
        }

        void IList<ServiceDescriptor>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
