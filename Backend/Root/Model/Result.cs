using System;
namespace Root.Model
{
    public class Result
    {
        public Result()
        {
        }

        public int Id { get; set; }
        public int BaseMethodId { get; set; }
        public int SimMethodId { get; set; }
        public float SimRatio { get; set; }
        public string ResultDetail { get; set; }
    }
}
