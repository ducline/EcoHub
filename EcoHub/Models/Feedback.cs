namespace EcoHub.Models
{
  
    public class Feedback
    {

        public Guid _guidFeedback;
        public string GuidFeedback
        {
            get { return _guidFeedback.ToString(); }
        }

        public string mensagem{ get; set; }

        public DateTime data_envio { get; set; }

        public Feedback()
        {
            _guidFeedback = Guid.NewGuid();
            mensagem = "";
            data_envio = DateTime.Now;
        }

        public Feedback(string GuidFeedback)
        {
            //_guidFeedback = GuidFeedback;
            Guid.TryParse(GuidFeedback, out _guidFeedback);
            mensagem = "";
            data_envio = DateTime.Now;
        }
    }
}
