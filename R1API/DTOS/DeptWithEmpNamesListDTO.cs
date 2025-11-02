namespace R1API.DTOS
{
    public class DeptWithEmpNamesListDTO
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public List<string> EmpsName { get; set; } = new List<string>();
    }
}
