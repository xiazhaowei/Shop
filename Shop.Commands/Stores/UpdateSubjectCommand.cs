using System;
using ENode.Commanding;

namespace Shop.Commands.Stores
{
    public class UpdateSubjectCommand: Command<Guid>
    {
        public string SubjectName { get; private set; }
        public string SubjectNumber { get;private  set; }
        public string SubjectPic { get;private set; }

        public UpdateSubjectCommand() { }
        public UpdateSubjectCommand(string subjectName, string subjectNumber, string subjectPic)
        {
            SubjectName = subjectName;
            SubjectNumber = subjectNumber;
            SubjectPic = subjectPic;
        }
    }
}
