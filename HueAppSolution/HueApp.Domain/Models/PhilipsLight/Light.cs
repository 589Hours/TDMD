
namespace HueApp.Domain.Models.PhilipsLight
{
    public class Light
    {
        public State state { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return 
                $"Name: {name}\n" +
                $"State: {state}";
        }
    }
}
