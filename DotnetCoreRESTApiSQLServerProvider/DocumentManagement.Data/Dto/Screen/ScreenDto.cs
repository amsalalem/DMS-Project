using System;

namespace DocumentManagement.Data.Dto
{
    public class ScreenDto: ErrorStatusCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
