using EquipmentWatcher.Models;

namespace EquipmentWatcher.Responses
{
    public class InteractionResponse : BaseResponse
    {
        public InteractionResponse(Interaction interaction) : base(true, null, string.Empty)
        {
            Result = new View(interaction);
        }

        public InteractionResponse(IEnumerable<Interaction> interactions) : base(true, null, string.Empty)
        {
            Result = interactions.Select(i => new View(i));
        }

        public class View
        {
            public int ID { get; set; }
            public string Token { get; set; }
            public DateTime Timestamp { get; set; }
            public string Result { get; set; }

            public View(Interaction interaction)
            {
                this.ID = interaction.InteractionID;
                this.Token = interaction.Token;
                this.Timestamp = interaction.Timestamp;
                this.Result = interaction.Result;
            }
        }
    }
}
