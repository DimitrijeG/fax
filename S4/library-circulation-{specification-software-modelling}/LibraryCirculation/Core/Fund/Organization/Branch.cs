using System;
using System.Collections.Generic;
using LibraryCirculation.Application.Common;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Fund.Organization
{
    public class Branch : IKey, ISerializable
    {
        public Branch() : this(0, "", TimeOnly.MinValue, TimeOnly.MinValue)
        {
        }

        public Branch(int id, string address, TimeOnly workTimeStart, TimeOnly workTimeEnd)
        {
            Id = id;
            Address = address;
            WorkTimeStart = workTimeStart;
            WorkTimeEnd = workTimeEnd;
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public TimeOnly WorkTimeStart { get; set; }
        public TimeOnly WorkTimeEnd { get; set; }

        public object GetKey()
        {
            return Id;
        }

        public void SetKey(object key)
        {
            Id = (int)key;
        }

        public IReadOnlyList<string> Serialize()
        {
            return new[] { Id.ToString(), Address, Util.ToString(WorkTimeStart), Util.ToString(WorkTimeEnd) };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            Address = values[1];
            WorkTimeStart = Util.ParseTime(values[2]);
            WorkTimeEnd = Util.ParseTime(values[3]);
        }
    }
}