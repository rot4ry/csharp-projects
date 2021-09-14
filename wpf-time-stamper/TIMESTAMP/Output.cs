namespace TIMESTAMP
{
    /// <summary>
    /// JSON format for the timestamp data
    /// </summary>


    // {
    //      "filename" : "FILENAME"
    //      "timestamps" : [
    //      {
    //          "number" : 1,
    //          "timestamp" : time
    //      },
    //      {
    //          "number" : 2,
    //          "timestamp" : time
    //      }]
    //  }

    public class SingleStamp
    {
        public int ID { get; set; }
        public string TS { get; set; }
    }

    public class Output
    {
        public string Filename { get; set; }
        public SingleStamp[] Stamps { get; set; }
    }
}
