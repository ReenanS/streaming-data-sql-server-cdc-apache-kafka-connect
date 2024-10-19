namespace Domain.Models
{
    public class StepFunctionInput
    {
        public string TipoOperacao { get; set; }
        public int CodigoExterno { get; set; }
        public string StateMachineArn { get; set; }
    }
}
