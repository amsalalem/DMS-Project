namespace DocumentManagement.MediatR.Queries.ManageWorkFlow
{
    public class GetMyApprovedFileLibraryQuery : GetAllWorkFlowQuery
    {
        public string Email { get; set; }
    }
}
