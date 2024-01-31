namespace DocumentManagement.MediatR.Queries.ManageWorkFlow
{
    public class GetFileNeedMyApprovalLibraryQuery : GetAllWorkFlowQuery
    {
        public string Email { get; set; }
    }
}
