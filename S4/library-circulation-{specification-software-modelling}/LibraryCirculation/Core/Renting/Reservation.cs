using System;
using System.Collections.Generic;
using LibraryCirculation.Application.Common;
using LibraryCirculation.DataManagement.Repository;
using LibraryCirculation.DataManagement.Serialize;

namespace LibraryCirculation.Core.Renting
{
    public enum ReservationStatus
    {
        Submitted,
        Approved,
        Declined
    }

    public class Reservation : IKey, ISerializable
    {
        public Reservation() : this(0, DateTime.MinValue, DateTime.MinValue, ReservationStatus.Submitted, "", "", 0)
        {
        }

        public Reservation(int id, DateTime submittingDate, DateTime copyAssignmentDate, ReservationStatus status,
            string bookIsbn, string readerUsername, int assignedCopyId)
        {
            Id = id;
            SubmittingDate = submittingDate;
            CopyAssignmentDate = copyAssignmentDate;
            Status = status;
            BookIsbn = bookIsbn;
            ReaderUsername = readerUsername;
            AssignedCopyId = assignedCopyId;
        }

        public int Id { get; set; }
        public DateTime SubmittingDate { get; set; }
        public DateTime CopyAssignmentDate { get; set; }
        public ReservationStatus Status { get; set; }
        public string BookIsbn { get; set; }
        public string ReaderUsername { get; set; }
        public int AssignedCopyId { get; set; }

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
            return new[]
            {
                Id.ToString(), Util.ToString(SubmittingDate), Util.ToString(CopyAssignmentDate),
                Status.ToString(), BookIsbn, ReaderUsername, AssignedCopyId.ToString()
            };
        }

        public void Deserialize(IReadOnlyList<string> values)
        {
            Id = int.Parse(values[0]);
            SubmittingDate = Util.ParseDateTime(values[1]);
            CopyAssignmentDate = Util.ParseDateTime(values[2]);
            Status = SerialUtil.Parse<ReservationStatus>(values[3]);
            BookIsbn = values[4];
            ReaderUsername = values[5];
            AssignedCopyId = int.Parse(values[6]);
        }
    }
}