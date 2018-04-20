using System;
using ENode.Commanding;

namespace Shop.Commands.Stores
{
    public class CreateStoreCommand: Command<Guid>
    {
        public Guid UserId { get;private set; }

        public string AccessCode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Region { get; private set; }
        public string Address { get; private set; }

        public string SubjectName { get; private set; }
        public string SubjectNumber { get; private set; }
        public string SubjectPic { get; private set; }

        public CreateStoreCommand() { }
        public CreateStoreCommand(
            Guid id,
            Guid userId,
            string accessCode,
            string name,
            string description,
            string region,
            string address,
            string subjectName,
            string subjectNumber,
            string subjectPic):base(id)
        {
            UserId = userId;
            AccessCode = accessCode;
            Name = name;
            Description = description;
            Region = region;
            Address = address;

            SubjectName = subjectName;
            SubjectNumber = subjectNumber;
            SubjectPic = subjectPic;
        }
    }
}
